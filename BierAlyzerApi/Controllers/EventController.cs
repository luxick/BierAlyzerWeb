using System;
using BierAlyzerApi.Services;
using Contracts.Communication.Event;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BierAlyzerApi.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Manage events </summary>
    ///
    /// <remarks>   Andre Beging, 18.06.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    [Route("api/event")]
    public class EventController : ControllerBase
    {
        private readonly BierAlyzerService _service;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Events controller </summary>
        ///
        /// <remarks>   Andre Beging, 18.06.2018. </remarks>
        ///
        /// <param name="service">  The service. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public EventController(BierAlyzerService service)
        {
            _service = service;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get your Events. </summary>
        ///
        /// <remarks>   Andre Beging, 20.06.2018. </remarks>
        ///
        /// <returns>   Result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [SwaggerResponse(200, typeof(EventsResponse))]
        public IActionResult Get()
        {
            var events = _service.GetEvents(Guid.Empty);

            return Ok(new EventsResponse
            {
                Events = events
            });
        }
    }
}