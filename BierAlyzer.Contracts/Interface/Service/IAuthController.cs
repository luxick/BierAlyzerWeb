﻿using BierAlyzer.Contracts.Communication.Auth.Request;

namespace BierAlyzer.Contracts.Interface.Service
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Interface for authentication controller. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface IAuthController<T>
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Token method </summary>
        /// <param name="request">  The request. </param>
        /// <returns>   A T. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        T Token(TokenRequest request);

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Refresh token method </summary>
        /// <param name="refreshToken"> The refresh token. </param>
        /// <returns>   A T. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        T Refresh(RefreshTokenRequest refreshToken);
    }
}
