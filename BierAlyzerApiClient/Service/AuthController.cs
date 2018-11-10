using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using BierAlyzerApiClient.Helper;
using Contracts.Communication.Auth.Request;
using Contracts.Communication.Auth.Response;
using Contracts.Interface.Communication;
using Contracts.Interface.Service;

namespace BierAlyzerApiClient.Service
{
    public class AuthController : IAuthController<TokenResponse>
    {
        private readonly HttpClient _client;

        public AuthController(HttpClient client)
        {
            _client = client;
        }

        public TokenResponse Token(TokenRequest request)
        {
            return RequestHelper.ApiControllerPost<TokenResponse>(_client, "auth", "token", request);
        }

        public TokenResponse Refresh(RefreshTokenRequest refreshToken)
        {
            return RequestHelper.ApiControllerPost<TokenResponse>(_client, "auth", "refresh", refreshToken);
        }
    }
}
