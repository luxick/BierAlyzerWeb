using System;
using System.Net.Http;
using System.Net.Http.Headers;
using BierAlyzerApiClient.Service;
using Contracts.Communication.Auth.Request;

namespace BierAlyzerApiClient
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   The BierAlyzer client. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class BierAlyzerClient : IDisposable
    {
        #region Private Properties

        private HttpClient Client { get; }

        #endregion

        #region Public Properties

        #region Api Controllers

        public AuthController AuthController { get; set; }

        #endregion

        #region AccessToken

        private string _accessToken;

        public string AccessToken
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
            }
        }

        #endregion

        public string RefreshToken { get; set; }

        #endregion

        #region Constructors

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="serverAddress">    The server address. </param>
        /// <param name="accessToken">      (Optional) The access token. </param>
        /// <param name="refreshToken">     (Optional) The refresh token. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public BierAlyzerClient(Uri serverAddress, string accessToken = null, string refreshToken = null)
        {
            Client = new HttpClient { BaseAddress = serverAddress };

            // Set refresh token
            RefreshToken = refreshToken;

            // Set access token
            if (accessToken != null)
            {
                AccessToken = accessToken;
            }
            else
            {
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            }

            AuthController = new AuthController(Client);
        }

        #endregion

        #region SignIn

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sign in method </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="mailAddress">  The mail address. </param>
        /// <param name="password">     The password. </param>
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SignIn(string mailAddress, string password)
        {
            var tokenRequest = new TokenRequest
            {
                Mail = mailAddress,
                Password = password
            };

            var result = AuthController.Token(tokenRequest);

            if (result != null)
            {
                AccessToken = result.AccessToken.Token;
                RefreshToken = result.RefreshToken.Token;

                return true;
            }

            return false;
        }

        #endregion

        #region Dispose

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            Client?.Dispose();
        }

        #endregion
    }
}
