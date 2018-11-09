using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BierAlyzerApi.Models;
using BierAlyzerApi.Services;
using Contracts.Communication.Token.Request;
using Contracts.Communication.Token.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BierAlyzerApi.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <inheritdoc />
    ///  <summary>   Manage API token </summary>
    ///  <remarks>   Andre Beging, 18.06.2018. </remarks>
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BierAlyzerService _service;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Token controller. </summary>
        /// <remarks>   Andre Beging, 18.06.2018. </remarks>
        /// <param name="service">          The service. </param>
        /// <param name="configuration">    The configuration. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public AuthController(BierAlyzerService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get API Token. </summary>
        /// <remarks>   Andre Beging, 18.06.2018. </remarks>
        /// <param name="request">  TokenRequest. </param>
        /// <returns>   Result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        [SwaggerResponse(200, typeof(TokenResponse), "Fine, here is your token")]
        [SwaggerResponse(400, null, "Invalid data")]
        [SwaggerResponse(401, null, "Incorrect credentials")]
        public IActionResult Post([FromBody] TokenRequest request)
        {
            if (request == null) return BadRequest("Invalid data");
            var userClaims = _service.ValidateCredentials(request.Mail, request.Password);

            if (userClaims != null)
            {
                var jwtConfiguration = _configuration.GetSection("Jwt");
                var secret = jwtConfiguration.GetValue<string>("Key");
                var issuer = jwtConfiguration.GetValue<string>("Issuer");
                var audience = jwtConfiguration.GetValue<string>("Audience");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expireDate = DateTime.Now.AddMinutes(10);

                var token = new JwtSecurityToken(
                    audience: audience,
                    issuer: issuer,
                    claims: userClaims,
                    expires: expireDate,
                    signingCredentials: creds);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expires = expireDate
                });
            }

            return Unauthorized();
        }
    }
}