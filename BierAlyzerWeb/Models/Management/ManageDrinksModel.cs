using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using BierAlyzerWeb.Helper;
using Contracts.Model;

namespace BierAlyzerWeb.Models.Management
{
    public class ManageDrinksModel
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        /// <remarks>   Andre Beging, 03.05.2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public ManageDrinksModel()
        {
            Drinks = new List<Drink>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the drinks. </summary>
        /// <value> The drinks. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<Drink> Drinks { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the name of the drink. </summary>
        /// <value> The name of the drink. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        [Required(ErrorMessage = "Das Feld darf nicht leer sein")]
        public string DrinkName { get; set; }

        #region DrinkAmount

        [Required(ErrorMessage = "Das Feld darf nicht leer sein")]
        [IsStringDouble]
        public string DrinkAmountString { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the drink amount. </summary>
        /// <value> The drink amount. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public double DrinkAmount
        {
            get => double.Parse(DrinkAmountString.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture);
            set => DrinkAmountString = value.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
        }

        #endregion

        #region DrinkPercentage

        [Required(ErrorMessage = "Das Feld darf nicht leer sein")]
        [IsStringDouble]
        public string DrinkPercentageString { get; set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the drink percentage. </summary>
        /// <value> The drink percentage. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public double DrinkPercentage
        {
            get => double.Parse(DrinkPercentageString.Replace(",", "."), NumberStyles.Any,
                CultureInfo.InvariantCulture);
            set => DrinkPercentageString = value.ToString(CultureInfo.InvariantCulture).Replace(".", ",");
        }

        #endregion
    }
}