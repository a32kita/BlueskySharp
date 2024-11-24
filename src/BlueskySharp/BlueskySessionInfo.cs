using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BlueskySharp
{
    public class BlueskySessionInfo
    {
        public string AccessJwt
        {
            get;
            set;
        }

        public string RefreshJwt
        {
            get;
            set;
        }


        private DateTimeOffset _getTokenExpiration(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jsonToken == null)
                throw new ArgumentException();

            var payload = jsonToken.Payload;
            var expiration = payload.Expiration;
            if (expiration != null)
                return DateTimeOffset.FromUnixTimeSeconds(payload.Expiration.Value);

            throw new ArgumentException();
        }

        public DateTimeOffset GetAccessJwtExpiration()
        {
            return this._getTokenExpiration(this.AccessJwt);
        }

        public DateTimeOffset RefreshAccessJwtExpiration()
        {
            return this._getTokenExpiration(this.RefreshJwt);
        }
    }
}
