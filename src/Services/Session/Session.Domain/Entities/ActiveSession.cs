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

        public string Selector { get; private set; }

        public string VerifierHash { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        public DateTime? LastUsedAt { get; private set; }

        public string ClientInfo { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        private ActiveSession() { }

        public ActiveSession(Guid userId, string selector, string verifierHash, int lifetimeInDays, string clientInfo)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Selector = selector;
            VerifierHash = verifierHash;
            ExpiresAt = DateTime.UtcNow.AddDays(lifetimeInDays);
            ClientInfo = clientInfo;
            CreatedAt = DateTime.UtcNow;
        }

        public void RotateRefreshToken(string newSelector, string newVerifierHash, int lifetimeInDays)
        {
            Selector = newSelector;
            VerifierHash = newVerifierHash;
            ExpiresAt = DateTime.UtcNow.AddDays(lifetimeInDays);
            LastUsedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void MarkAsUsed()
        {
            LastUsedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
