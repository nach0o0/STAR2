using Auth.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shared.Application.Interfaces.Persistence;

namespace Auth.Infrastructure.Persistence
{
    public class AuthDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public AuthDbContext(DbContextOptions<AuthDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Wendet alle Konfigurationen aus dieser Assembly an (z.B. UserConfiguration)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. Domain Events aus allen getrackten Entitäten sammeln
            var domainEvents = GetAndClearDomainEvents();

            // 2. Änderungen in der Datenbank speichern
            var result = await base.SaveChangesAsync(cancellationToken);

            // 3. Nach erfolgreichem Speichern die Events veröffentlichen
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
