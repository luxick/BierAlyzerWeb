using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace Contracts.Model
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string Origin { get; set; }

        public UserType Type { get; set; }

        public string Mail { get; set; }

        public string Hash { get; set; }

        public string Salt { get; set; }

        public bool Enabled { get; set; }

        public ICollection<UserEvent> UserEvents { get; set; }

        public ICollection<DrinkEntry> DrinkEntries { get; set; }

        #region ConsumedLiters

        [NotMapped]
        public double ConsumedLiters
        {
            get
            {
                try
                {
                    return DrinkEntries.Select(de => de.Drink.Amount).Sum(x => x);
                }
                catch (Exception)
                {
                    // ignored
                }

                return 0d;
            }
        }

        #endregion

        #region Created

        [NotMapped]
        public DateTime Created
        {
            get
            {
                if (DateTime.TryParse(CreatedString, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var parseDateTime)) return parseDateTime;
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
                if (DateTime.TryParse(ModifiedString, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var parseDateTime)) return parseDateTime;
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

        #region LastLogin

        [NotMapped]
        public DateTime LastLogin
        {
            get
            {
                if (DateTime.TryParse(LastLoginString, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var parseDateTime)) return parseDateTime;
                return DateTime.MinValue;
            }
            set => LastLoginString = value.ToString(CultureInfo.InvariantCulture);
        }

        private string _lastLoginString;

        public string LastLoginString
        {
            get => _lastLoginString;
            set
            {
                if (value != null && _lastLoginString == value) return;
                _lastLoginString = value;
            }
        }

        #endregion
    }
}