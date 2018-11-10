using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Contracts.Interface.Communication;
using ProtoBuf;

namespace BierAlyzerApiClient.Extensions
{
    public static class ProtoBufExtensions
    {
        public static byte[] ProtoSerialize(this IApiRequestParameter parameter)
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

        public static T ProtoDeserialize<T>(this byte[] data) where T : class, IApiResponseParameter
        {
            if (data == null) return null;

            try
            {
                using (var stream = new MemoryStream(data))
                {
                    return Serializer.Deserialize<T>(stream);
                }
            }
            catch (Exception e)
            {
                throw new SerializationException($"Error while deserializing: {e}");
            }
        }
    }
}
