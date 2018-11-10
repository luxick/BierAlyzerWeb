using System.Net.Http;
using BierAlyzerApiClient.Helper;
using Contracts.Communication.Auth.Request;
using Contracts.Communication.Auth.Response;
using Contracts.Interface.Service;

namespace BierAlyzerApiClient.Service
{
    public class AuthController : IAuthController<TokenResponse>
    {
        private readonly HttpClient _client;

        #region Constructors

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="client">   The client. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public AuthController(HttpClient client)
        {
            _client = client;
        }

        #endregion

        #region Token

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Retrieves a token pair from the api </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="request">  The request. </param>
        /// <returns>   A TokenResponse. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenResponse Token(TokenRequest request)
        {
            return RequestHelper.ApiControllerPost<TokenResponse>(_client, "auth", "token", request);
        }

        #endregion

        #region Refresh

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets a fresh token pair from the api </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="refreshToken"> The refresh token. </param>
        /// <returns>   A TokenResponse. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public TokenResponse Refresh(RefreshTokenRequest refreshToken)
        {
            return RequestHelper.ApiControllerPost<TokenResponse>(_client, "auth", "refresh", refreshToken);
        }

        #endregion
    }
}
