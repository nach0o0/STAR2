using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Interfaces.Persistence;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Domain.Entities;

namespace TimeTracking.Infrastructure.Persistence
{
    public class TimeTrackingDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public TimeTrackingDbContext(DbContextOptions<TimeTrackingDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;

        }

        public DbSet<TimeEntry> TimeEntries { get; set; }

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
