using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Session.Domain.Entities
{
    public class RevokedToken
    {
        public Guid Jti { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        private RevokedToken() { }

        public RevokedToken(Guid jti, DateTime expiresAt)
        {
            Jti = jti;
            ExpiresAt = expiresAt;
        }
    }
}
