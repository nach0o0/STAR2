using Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Domain.Entities
{
    public class ActiveSession : BaseEntity<Guid>
    {
        public Guid UserId { get; private set; }

        public string RefreshTokenHash { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        public DateTime? LastUsedAt { get; private set; }

        public string ClientInfo { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        private ActiveSession() { }

        public ActiveSession(Guid userId, string refreshTokenHash, int lifetimeInDays, string clientInfo)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            RefreshTokenHash = refreshTokenHash;
            ExpiresAt = DateTime.UtcNow.AddDays(lifetimeInDays);
            ClientInfo = clientInfo;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsUsed()
        {
            LastUsedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
