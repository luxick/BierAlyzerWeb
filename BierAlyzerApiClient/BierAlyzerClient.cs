using System;
using System.Net.Http;
using System.Net.Http.Headers;
using BierAlyzerApiClient.Service;
using Contracts.Communication.Token.Request;

namespace BierAlyzerApiClient
{
    public class BierAlyzerClient : IDisposable
    {
        private HttpClient Client { get; }

        private string _accessToken;

        public AuthController AuthController { get; set; }

        public string AccessToken
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", value);
            }
        }

        public string RefreshToken { get; set; }

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
            }

            AuthController = new AuthController(Client);
        }

        public void SignIn(string mailAddress, string password)
        {
            var tokenRequest = new TokenRequest
            {
                Mail = mailAddress,
                Password = password
            };

            var result = AuthController.Token(tokenRequest);
        }

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
    }
}
