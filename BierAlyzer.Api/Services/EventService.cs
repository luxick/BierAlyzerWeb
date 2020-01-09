using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using BierAlyzer.Api.Models;
using BierAlyzer.Contracts.Communication.Event;
using BierAlyzer.Contracts.Dto;
using BierAlyzer.Contracts.Model;
using BierAlyzer.EntityModel;

namespace BierAlyzer.Api.Services
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An event service. </summary>
    /// <remarks>   Andre Beging, 10.11.2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class EventService : BierAlyzerServiceBase
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        /// <remarks>   Andre Beging, 18.11.2018. </remarks>
        /// <param name="context">  The context. </param>
        /// <param name="mapper">   The mapper. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public EventService(BierAlyzerContext context, IMapper mapper) : base(context, mapper)
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the events. </summary>
        /// <remarks>   Andre Beging, 18.11.2018. </remarks>
        /// <returns>   The events. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public EventResponse GetEvents(IEnumerable<Claim> claims)
        {
            try
            {
                var response = new EventResponse();

                if (!claims.TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
                    response.Result.Status = RequestResultStatus.TokenError;

                if (!claims.TryGetValue<UserType>(BierAlyzerClaim.UserType, out var userType))
                    response.Result.Status = RequestResultStatus.TokenError;

                // Does anything went wrong?
                if (!response.Result.Success) return response;

                var contextEvents = Context.Event.Where(e => e.EventId != Guid.Empty);
                if (userType == UserType.User)
                {
                    // A user has to be the owner of or registered to the event
                    contextEvents = contextEvents.Where(e => e.OwnerId == userId || e.EventUsers.Any(eu => eu.UserId == userId));
                }

                response.Events.AddRange(Mapper.Map<List<EventDto>>(contextEvents));

                return response;
            }
            catch (Exception e)
            {
                return new EventResponse
                {
                    Result = new RequestResult(RequestResultStatus.ServerError, e.Message)
                };
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets an event. </summary>
        /// <remarks>   Andre Beging, 18.11.2018. </remarks>
        /// <param name="eventId">      Identifier for the event. </param>
        /// <param name="claims">   The user claims. </param>
        /// <returns>   The event. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public EventResponse GetEvent(Guid eventId, IEnumerable<Claim> claims)
        {
            try
            {
                var response = new EventResponse();

                if (eventId == Guid.Empty)
                    response.Result.Status = RequestResultStatus.InvalidParameter;

                if (!claims.ToList().TryGetValue<Guid>(BierAlyzerClaim.UserId, out var userId))
                    response.Result.Status = RequestResultStatus.TokenError;

                if (!claims.TryGetValue<UserType>(BierAlyzerClaim.UserType, out var userType))
                    response.Result.Status = RequestResultStatus.TokenError;

                // Does anything went wrong?
                if (!response.Result.Success) return response;

                // Try get event from database
                Event contextEvent;
                if (userType == UserType.Admin)
                    contextEvent = Context.Event.FirstOrDefault(x => x.EventId == eventId);
                else
                    contextEvent = Context.Event.Where(e => e.OwnerId == userId || e.EventUsers.Any(eu => eu.UserId == userId))
                        .FirstOrDefault(x => x.EventId == eventId);

                if (contextEvent != null)
                    response.Events.Add(Mapper.Map<EventDto>(contextEvent));
                else
                    response.Result.Status = RequestResultStatus.NoContent;


                return response;
            }
            catch (Exception e)
            {
                return new EventResponse
                {
                    Result = new RequestResult(RequestResultStatus.ServerError, e.Message)
                };
            }
        }
    }
}
