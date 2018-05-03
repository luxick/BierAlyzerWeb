using System;
using System.Linq;
using BierAlyzerWeb.Models;
using Contracts.Model;

namespace BierAlyzerWeb.Helper
{
    public static class ContextHelper
    {
        #region Private Properties

        private static BierAlyzerContextFactory _factory;

        #endregion

        #region OpenContext

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Opens a new Datacontext </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        /// <returns>   A BierAlyzerContext. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static BierAlyzerContext OpenContext()
        {
            if (_factory == null)
                _factory = new BierAlyzerContextFactory();

            // ReSharper disable once AssignNullToNotNullAttribute
            return _factory.CreateDbContext(null);
        }

        #endregion

        #region GetUserEvents

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get the UserEvents by UserId </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="context">  The context to act on. </param>
        /// <param name="userId">   Identifier for the user. </param>
        /// <returns>   The user events. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static IQueryable<Event> GetUserEvents(this BierAlyzerContext context, Guid userId)
        {
            return context.User
                .Where(u => u.UserId == userId)
                .SelectMany(u => u.UserEvents)
                .Select(ue => ue.Event)
                .Where(e => e.Type != EventType.Hidden);
        }

        #endregion

        #region GetEventUsers

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Get the EventUsers by EventId </summary>
        /// <remarks>   Andre Beging, 27.04.2018. </remarks>
        /// <param name="context">  The context to act on. </param>
        /// <param name="eventId">  Identifier for the event. </param>
        /// <returns>   The event users. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static IQueryable<User> GetEventUsers(this BierAlyzerContext context, Guid eventId)
        {
            return context.Event
                .Where(u => u.EventId == eventId)
                .SelectMany(u => u.EventUsers)
                .Select(ue => ue.User);
        }

        #endregion
    }
}