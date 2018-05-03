using System;
using System.Collections.Generic;

namespace BierAlyzerWeb.Helper
{
    public static class SharedProperties
    {
        public static List<Guid> OutdatedObjects = new List<Guid>();

        #region KnownOrigins

        /// <summary>   A List of known origins. </summary>
        public static List<string> KnownOrigins = new List<string>()
        {
            "FH Aachen",
            "RWTH Aachen",
            "Hochschule Augsburg",
            "TU Berlin",
            "HTW Berlin",
            "Ruhr-Uni Bochum",
            "TH Brandenburg",
            "TU Braunschweig",
            "Hochschule Bremen",
            "TU Chemnitz",
            "TU Cottbus",
            "TU Darmstadt",
            "Hochschule Darmstadt",
            "FH Dortmund",
            "TU Dortmund",
            "TU Dresden",
            "Uni Duisburg-Essen",
            "FH Düsseldorf",
            "HS Emden/Leer",
            "Uni Erlangen-Nürnberg",
            "FH Flensburg",
            "FH Frankfurt am Main",
            "Uni Freiburg",
            "Hochschule Fulda",
            "FH Gießen-Friedberg",
            "TU Graz",
            "TU Hamburg-Harburg",
            "Uni Hannover",
            "Hochschule Hannover",
            "Hochschule Heilbron",
            "TU Ilmenau",
            "FH Jena",
            "FH Kaiserslautern",
            "TU Kaiserslautern",
            "KIT",
            "Hochschule Kempten",
            "Uni Kiel",
            "Hochschule Kiel",
            "Hochschule OWL",
            "Hochschule OWL",
            "TU München",
            "Hochschule München",
            "Universität Paderborn",
            "OTH Regensburg",
            "HTW d. Saarlandes",
            "Hochschule Würzburg",
            "Uni Siegen",
            "Uni Stuttgart",
            "Hochschule Ulm",
            "Uni Ulm",
            "HS Ravensburg",
            "TU Wien",
            "FH Wolfenbüttel",
            "ETH Zürich"
        };

        #endregion
    }
}
