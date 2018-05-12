using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Contracts.Model
{
    public class Event
    {
        [Key]
        public Guid EventId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public EventType Type { get; set; }

        public string Code { get; set; }

        #region Start

        [NotMapped]
        public DateTime Start
        {
            get
            {
                if (DateTime.TryParse(StartString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parseDateTime)) return parseDateTime;
                return DateTime.MinValue;
            }
            set => StartString = value.ToString(CultureInfo.InvariantCulture);
        }

        private string _startString;
        public string StartString
        {
            get => _startString;
            set
            {
                if (value != null && _startString == value) return;
                _startString = value;
            }
        }

        #endregion

        #region End

        [NotMapped]
        public DateTime End
        {
            get
            {
                if (DateTime.TryParse(EndString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parseDateTime)) return parseDateTime;
                return DateTime.MinValue;
            }
            set => EndString = value.ToString(CultureInfo.InvariantCulture);
        }

        private string _endString;
        public string EndString
        {
            get => _endString;
            set
            {
                if (value != null && _endString == value) return;
                _endString = value;
            }
        }

        #endregion

        #region Status

        [NotMapped]
        public EventStatus Status
        {
            get
            {
                if (Start <= DateTime.Now && End >= DateTime.Now) return EventStatus.Open;
                if (Start >= DateTime.Now) return EventStatus.NotYet;
                return EventStatus.Closed;
            }
        }

        #endregion

        #region Created

        [NotMapped]
        public DateTime Created
        {
            get
            {
                if (DateTime.TryParse(CreatedString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parseDateTime)) return parseDateTime;
                return DateTime.MinValue;
            }
            set => CreatedString = value.ToString(CultureInfo.InvariantCulture);
        }

        private string _createdString;
        public string CreatedString
        {
            get => _createdString;
            set
            {
                if (value != null && _createdString == value) return;
                _createdString = value;
            }
        }

        #endregion

        #region Modified

        [NotMapped]
        public DateTime Modified
        {
            get
            {
                if (DateTime.TryParse(ModifiedString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parseDateTime)) return parseDateTime;
                return DateTime.MinValue;
            }
            set => ModifiedString = value.ToString(CultureInfo.InvariantCulture);
        }

        private string _modifiedString;
        public string ModifiedString
        {
            get => _modifiedString;
            set
            {
                if (value != null && _modifiedString == value) return;
                _modifiedString = value;
            }
        }

        #endregion

        public Guid OwnerId { get; set; }

        public User Owner { get; set; }

        public ICollection<UserEvent> EventUsers { get; set; }

        public ICollection<DrinkEntry> DrinkEntries { get; set; }
    }
}
