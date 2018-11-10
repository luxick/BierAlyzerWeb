using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using BierAlyzerApiClient.Helper;
using Contracts.Communication.Token.Request;
using Contracts.Interface.Service;

namespace BierAlyzerApiClient.Service
{
    public class AuthController : IAuthController<bool>
    {
        private readonly HttpClient _client;

        public AuthController(HttpClient client)
        {
            _client = client;
        }

        public bool Token(TokenRequest request)
        {
            return RequestHelper.PostToApiController(_client, "auth", "token", request);
        }

        public bool Refresh(RefreshTokenRequest refreshToken)
        {
            return RequestHelper.PostToApiController(_client, "auth", "refresh", refreshToken);
        }
    }
}
