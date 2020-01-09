﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BierAlyzer.EntityModel;
using BierAlyzer.Web.Helper;

namespace BierAlyzer.Web.Models.Management
{
    public class ManageEventsModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ManageEventsModel()
        {
            Events = new List<Event>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the events. </summary>
        /// <value> The events. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<Event> Events { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the name of the event. </summary>
        /// <value> The name of the event. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "Ein Event braucht einen Namen")]
        public string EventName { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets information describing the event. </summary>
        /// <value> Information describing the event. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string EventDescription { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the Date/Time of the event start. </summary>
        /// <value> The event start. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "Ein Startzeitpunkt muss angegeben werden.")]
        [DataType(DataType.DateTime, ErrorMessage = "Das ist keine Zeitangabe")]
        public DateTime EventStart { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the Date/Time of the event end. </summary>
        /// <value> The event end. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "Ein Endzeitpunkt muss angegeben werden.")]
        [DataType(DataType.DateTime, ErrorMessage = "Das ist keine Zeitangabe")]
        [DateLaterThan("EventStart", ErrorMessage = "Das Event muss enden, nachdem es begonnen hat.")]
        public DateTime EventEnd { get; set; }
    }
}