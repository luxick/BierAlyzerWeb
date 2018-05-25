using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Contracts.Model
{
    public class Drink
    {
        [Key]
        public Guid DrinkId { get; set; }

        public string Name { get; set; }

        public double Amount { get; set; }

        public double Percentage { get; set; }

        [NotMapped]
        public double AlcoholAmount => .8 * (Amount * 1000) * (Percentage / 100);

        public bool Visible { get; set; }

        public ICollection<DrinkEntry> DrinkEntries { get; set; }

        public Guid? OwnerId { get; set; }

        public User Owner { get; set; }

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
    }
}