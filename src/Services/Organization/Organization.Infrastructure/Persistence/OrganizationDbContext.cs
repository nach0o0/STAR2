using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Persistence;
using Shared.Domain.Abstractions;
using System.Reflection;

namespace Organization.Infrastructure.Persistence
{
    public class OrganizationDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;

        }

        public DbSet<Domain.Entities.Organization> Organizations { get; set; }
        public DbSet<EmployeeGroup> EmployeeGroups { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HourlyRate> HourlyRates { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

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
