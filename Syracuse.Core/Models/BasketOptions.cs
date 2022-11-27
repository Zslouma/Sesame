using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Syracuse.Mobitheque.Core.Models
{
    public class BasketOptions
    {
        [JsonProperty("query")]
        public BasketOptionsDetails Query { get; set; } = new BasketOptionsDetails();
    }

    public class BasketOptionsDetails
    {
        /// <summary>
        /// Page à afficher (débute à 0).
        /// </summary>
        public int Page { get; internal set; } = 0;

        /// <summary>
        /// Nombre de résultats à afficher.
        /// </summary>
        public int ResultSize { get; internal set; } = 15;

        /// <summary>
        /// Conditionne l'injection de la propriété "FieldList" dans les résultats.
        /// </summary>
        public bool InjectFields { get; set; } = true;

        /// <summary>
        /// Conditionne l'utilisation d'un canvas CF4.
        /// </summary> 
        public bool? UseCanvas { get; set; } = false;

        /// <summary>
        /// Chemin du Canvas Xsl à utiliser pour la transformation (optionnel, et uniquement dans le cas d'une recherche sur CanvasService).
        /// </summary>
        public string XslPath { get; set; } = "";

        /// <summary>
        /// Filtre par label (optionnel).
        /// </summary>
        public IList<long> LabelFilter { get; internal set; } = new List<long>();

        /// <summary>
        /// livre à rechercher dans le panier
        /// </summary>
        public String SearchInput { get; set; } = "";
    }

}
