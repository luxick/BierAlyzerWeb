using System;
using System.Collections.Generic;
using System.Linq;
using BierAlyzerApi.Helper;
using BierAlyzerApi.Models;
using Contracts.Model;

namespace BierAlyzerApi.Services
{
    public class BierAlyzerService
    {
        private readonly BierAlyzerContext _context;

        public BierAlyzerService(BierAlyzerContext context)
        {
            _context = context;
        }

        public List<Event> GetEvents(Guid userId)
        {
            var contextEvents = _context.Event.Where(e => e.EventUsers.Any(eu => eu.UserId == userId) && e.Type != EventType.Hidden);

            return contextEvents.ToList();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Validate user credentials </summary>
        ///
        /// <remarks>   Andre Beging, 17.06.2018. </remarks>
        ///
        /// <param name="mail">     The mail. </param>
        /// <param name="password"> The password. </param>
        ///
        /// <returns>   True if credentials are correct. False is not </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool ValidateCredentials(string mail, string password)
        {
            var contextUser = _context.User.FirstOrDefault(x => x.Mail.Equals(mail, StringComparison.InvariantCultureIgnoreCase) && x.Enabled);
            if (contextUser == null) return false;

            var hash = AuthenticationHelper.CalculatePasswordHash(contextUser.Salt, password);

            if (hash.Equals(contextUser.Hash, StringComparison.InvariantCulture))
                return true;

            return false;
        }
    }
}
