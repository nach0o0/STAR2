using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Session.Domain.Entities;
using Shared.Application.Interfaces.Persistence;
using Shared.Domain.Abstractions;
using System.Reflection;

namespace Session.Infrastructure.Persistence
{
    public class SessionDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public SessionDbContext(DbContextOptions<SessionDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;
        }

        public DbSet<ActiveSession> ActiveSessions { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = GetAndClearDomainEvents();
            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }

        private List<INotification> GetAndClearDomainEvents()
        {
            var domainEvents = ChangeTracker
                .Entries<BaseEntity<Guid>>()
                .Select(e => e.Entity.GetDomainEvents())
                .SelectMany(e => e)
                .ToList();

            ChangeTracker.Entries<BaseEntity<Guid>>()
                .ToList()
                .ForEach(e => e.Entity.ClearDomainEvents());

            return domainEvents;
        }
    }
}
