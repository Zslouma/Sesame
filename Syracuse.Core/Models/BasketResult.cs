using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    /// <summary>
    /// Cette Classe représente le resultat de la requéte SearchUserBasket qui pemert de recupérer les panniers user 
    /// </summary>
    public class BasketResult
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public D D { get; set; }
    }

    class BasketView
    {
        [JsonProperty("HtmlResult")]
        public string HtmlResult { get; set; }

        [JsonProperty("Results")]
        public BasketAdapter[] Results { get; set; }

        //[JsonProperty("SearchInfo")]
        //public SearchInfoAdapter SearchInfo { get; set; }

        [JsonProperty("Query")]
        public BasketQuery Query { get; set; }

        [JsonProperty("TotalBasketCount")]
        public int TotalBasketCount { get; set; }

        [JsonProperty("UserLabels")]
        public BasketLabel[] UserLabels { get; set; }

    }
    class BasketAdapter : Result
    {
        [JsonProperty("Labels")]
        public BasketLabel[] Labels { get; set; }
    }

    class BasketQuery
    {
        [JsonProperty("Page")]
        public int Page { get; set; }

        [JsonProperty("ResultSize")]
        public int ResultSize { get; set; }

        [JsonProperty("InjectFields")]
        public bool InjectFields { get; set; }

        [JsonProperty("UseCanvas")]
        public bool? UseCanvas { get; set; }

        [JsonProperty("XslPath")]
        public string XslPath { get; set; }

        [JsonProperty("LabelFilter")]
        public long[] LabelFilter { get; set; }

        [JsonProperty("TemplateParams")]
        public TemplateParams TemplateParams { get; set; }

        [JsonProperty("SearchInput")]
        public string SearchInput { get; set; }
    }

    
   //class SearchInfoAdapter
   // {
   //     [JsonProperty("AvailabilityScopes")]
   //     public AvailabilityScope[] AvailabilityScopes { get; set; }

   //     [JsonProperty("CanUseDsi")]
   //     public bool CanUseDsi { get; set; }

   //     [JsonProperty("MenuCollapsedByDefault")]
   //     public bool MenuCollapsedByDefault { get; set; }

   //     [JsonProperty("MenuCollapsible")]
   //     public bool MenuCollapsible { get; set; }

   //     [JsonProperty("ExportParamSets")]
   //     public ExportParamSetDTO[] ExportParamSets { get; set; }

   //     [JsonProperty("FacetListInfo")]
   //     public DecodedFacetList FacetListInfo { get; set; }

   //     [JsonProperty("GridFilters")]
   //     public DecodedFacetList GridFilters { get; set; }

   //     [JsonProperty("GroupingField")]
   //     public string GroupingField { get; set; }

   //     [JsonProperty("NBResults")]
   //     public int NBResults { get; set; }

   //     [JsonProperty("Page")]
   //     public int Page { get; set; }

   //     [JsonProperty("PageMax")]
   //     public int PageMax { get; set; }

   //     [JsonProperty("PageSizeResult")]
   //     public int[] PageSizeResult { get; set; }

   //     [JsonProperty("Pagination")]
   //     public PaginationItem[] Pagination { get; set; }
   // }

   class BasketLabel
    {
        [JsonProperty("Count")]
        public int? Count { get; set; }

        [JsonProperty("Culture")]
        public int? Culture { get; set; }

        [JsonProperty("IsSystem")]
        public bool IsSystem { get; set; }

        [JsonProperty("LabelUid")]
        public long LabelUid { get; set; }

        [JsonProperty("Site")]
        public int? Site { get; set; }

        [JsonProperty("User")]
        public object User { get; set; }

        [JsonProperty("WhenAdded")]
        public DateTimeOffset WhenAdded { get; set; }
    }


}

