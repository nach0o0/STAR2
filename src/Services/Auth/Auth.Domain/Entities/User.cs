using Auth.Domain.Events.Users;
using Shared.Domain.Abstractions;

namespace Auth.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsActive { get; private set; }

        private User() { }

        public User(string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new UserPasswordChangedEvent(Id));
        }

        public void PrepareForDeletion()
        {
            AddDomainEvent(new UserAccountDeletedEvent(Id));
        }
    }
}
