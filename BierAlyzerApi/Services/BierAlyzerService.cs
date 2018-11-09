using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var contextEvents = _context.Event.Where(e => /*e.EventUsers.Any(eu => eu.UserId == userId) &&*/ e.Type != EventType.Hidden);

            return contextEvents.ToList();
        }

    }
}
