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
using Attendance.Domain.Entities;

namespace Attendance.Infrastructure.Persistence
{
    public class AttendanceDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;

        }

        public DbSet<AttendanceEntry> AttendanceEntries { get; set; }
        public DbSet<AttendanceType> AttendanceTypes { get; set; }
        public DbSet<EmployeeWorkModel> EmployeeWorkModels { get; set; }
        public DbSet<PublicHoliday> PublicHolidays { get; set; }
        public DbSet<WorkModel> WorkModels { get; set; }

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
