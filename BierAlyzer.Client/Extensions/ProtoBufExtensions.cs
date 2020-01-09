using System;
using System.IO;
using System.Runtime.Serialization;
using BierAlyzer.Contracts.Interface.Communication;
using ProtoBuf;

namespace BierAlyzer.Client.Extensions
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Protocol buffer extensions. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class ProtoBufExtensions
    {
        #region ProtoSerialize

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   An IApiRequestParameter extension method that prototype serialize. </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <exception cref="SerializationException">   Thrown when a Serialization error condition
        ///                                             occurs. </exception>
        /// <param name="parameter">    The parameter to act on. </param>
        /// <returns>   A byte[]. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        #endregion

        #region ProtoDeserialize

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   A byte[] extension method that prototype deserialize. </summary>
        /// <remarks>   Andre Beging, 10.11.2018. </remarks>
        /// <exception cref="SerializationException">   Thrown when a Serialization error condition
        ///                                             occurs. </exception>
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="data"> The data to act on. </param>
        /// <returns>   A T. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
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

        #endregion
    }
}
