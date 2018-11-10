using BierAlyzerApi.Services;
using Contracts.Communication.Event;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BierAlyzerApi.Controllers
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Manage events. </summary>
    /// <remarks>   Andre Beging, 18.06.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    [Route("api/event")]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Events controller. </summary>
        /// <remarks>   Andre Beging, 18.06.2018. </remarks>
        /// <param name="eventService"> The service. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get your Events. </summary>
        /// <remarks>   Andre Beging, 20.06.2018. </remarks>
        /// <returns>   Result. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [SwaggerResponse(200, typeof(EventsResponse))]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}