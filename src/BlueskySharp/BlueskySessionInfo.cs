using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Text;

namespace BlueskySharp
{
    public class BlueskySessionInfo
    {
        private string _accessJwt;
        private string _refreshJwt;



        public string AccessJwt
        {
            get => this._accessJwt;
            set
            {
                this._accessJwt = value;
                this.AccessJwtExpiration = this._getTokenExpiration(value);
            }
        }

        public DateTimeOffset AccessJwtExpiration
        {
            get;
            private set;
        }

        public string RefreshJwt
        {
            get => this._refreshJwt;
            set
            {
                this._refreshJwt = value;
                this.RefreshJwtExpiration = this._getTokenExpiration(value);
            }
        }

        public DateTimeOffset RefreshJwtExpiration
        {
            get;
            private set;
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
    }
}
