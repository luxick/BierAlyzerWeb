using System.Net.Http;
using System.Net.Http.Headers;
using BierAlyzerApiClient.Extensions;
using Contracts.Interface.Communication;

namespace BierAlyzerApiClient.Helper
{
    public class RequestHelper
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Send a POST request to the API </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <typeparam name="T">    Generic type parameter (Response Parameter). </typeparam>
        /// <param name="client">           The client. </param>
        /// <param name="apiController">    The API controller. </param>
        /// <param name="methodName">       Name of the method. </param>
        /// <param name="parameter">        The parameter. </param>
        /// <returns>   An Request Parameter. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static T ApiControllerPost<T>(HttpClient client, string apiController, string methodName, IApiRequestParameter parameter) where T : class, IApiResponseParameter
        {
            var targetAddress = $"{apiController}/{methodName}";
            var response = client.PostAsync(targetAddress, SerializeForRequest(parameter)).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var rawBytes = response.Content.ReadAsByteArrayAsync().Result;
            return rawBytes.ProtoDeserialize<T>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Serialize the data via ProtoBuf for an HTTP request </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <param name="parameter">    The parameter. </param>
        /// <returns>   A ByteArrayContent. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static ByteArrayContent SerializeForRequest(IApiRequestParameter parameter)
        {
            var byteArrayContent = new ByteArrayContent(parameter.ProtoSerialize());
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            return byteArrayContent;
        }
    }
}
