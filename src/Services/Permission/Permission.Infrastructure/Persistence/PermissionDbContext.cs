using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Permission.Domain.Entities;
using Permission.Domain.ValueObjects;
using Shared.Application.Interfaces.Persistence;
using Shared.Domain.Abstractions;
using System.Reflection;

namespace Permission.Infrastructure.Persistence
{
    public class PermissionDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public PermissionDbContext(DbContextOptions<PermissionDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;
        }

        public DbSet<Domain.Entities.Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleToPermissionLink> RolePermissions { get; set; }
        public DbSet<UserPermissionAssignment> UserPermissionAssignments { get; set; }
        public DbSet<PermissionToScopeTypeLink> PermissionScopeTypeLinks { get; set; }

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
                .Entries<BaseEntity<object>>()
                .Select(e => e.Entity.GetDomainEvents())
                .SelectMany(e => e)
                .ToList();

            ChangeTracker.Entries<BaseEntity<object>>()
                .ToList()
                .ForEach(e => e.Entity.ClearDomainEvents());

            return domainEvents;
        }
    }
}
