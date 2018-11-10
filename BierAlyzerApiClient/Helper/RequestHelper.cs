using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Contracts.Interface.Communication;
using ProtoBuf;

namespace BierAlyzerApiClient.Helper
{
    public class RequestHelper
    {
        public static bool PostToApiController(HttpClient client, string apiController, string methodName, IApiRequestParameter parameter)
        {
            var targetAddress = $"{apiController}/{methodName}";
            var response = client.PostAsync(targetAddress, SerializeForPost(parameter));

            if (response.IsCompletedSuccessfully)
            {
                return true;
            }

            return false;
        }

        private static ByteArrayContent SerializeForPost(IApiRequestParameter parameter)
        {
            var byteArrayContent = new ByteArrayContent(ProtoSerialize(parameter));
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            return byteArrayContent;
        }

        private static byte[] ProtoSerialize(IApiRequestParameter parameter)
        {
            if (null == parameter) return null;
            try
            {
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, parameter);
                    return stream.ToArray();
                }
            }
            catch (Exception e)
            {
                throw new SerializationException($"Error while serializing {parameter}: {e}");
            }
        }
    }
}
