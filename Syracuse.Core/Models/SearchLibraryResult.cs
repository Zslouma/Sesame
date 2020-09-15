using System.Collections.Generic;
using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{
    public class SearchLibraryResult
    {
        public IList<object> errors { get; set; }
        public string message { get; set; }
        public bool success { get; set; }

        [JsonProperty("d")]
        public Dataa Dataa { get; set; }
    }
    public class HoldingColumn
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string Style { get; set; }
        public List<KeyValue> Labels { get; set; }
        public string ColumnLabel { get; set; }
        public string ColumnLabelKey { get; set; }
        public bool DesktopVisible { get; set; }
        public bool MobileVisible { get; set; }
        public bool CanSort { get; set; }
        public bool IsDefaultSort { get; set; }
        public string ColumnType { get; set; }


    }

    public class HoldingsStatementColumn
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string Style { get; set; }
        public List<KeyValue> Labels { get; set; }
        public string ColumnLabel { get; set; }
        public string ColumnLabelKey { get; set; }
        public bool DesktopVisible { get; set; }
        public bool MobileVisible { get; set; }
        public bool CanSort { get; set; }
        public bool IsDefaultSort { get; set; }
        public string ColumnType { get; set; }
    }

    public class ItemHoldingsData
    {
        public int Availability { get; set; }
        public object Docbase { get; set; }
        public bool Error { get; set; }
        public object ErrorMessage { get; set; }
        public string HoldingLabel { get; set; }
        public string Id { get; set; }
        public object Mode { get; set; }
        public string RecordId { get; set; }
        public object WhenHoldBack { get; set; }
    }

    public class FieldListData
    {
        public IList<string> Identifier { get; set; }
        public IList<string> Title { get; set; }
        public IList<string> Title_sort { get; set; }
        public IList<string> DateOfPublication_sort { get; set; }
        public IList<string> TypeOfDocument { get; set; }
        public IList<string> TypeOfDocument_idx { get; set; }
        public IList<string> TypeOfDocument_ils { get; set; }
        public IList<string> sys_support { get; set; }
        public IList<string> Popularity_sort { get; set; }
        public IList<string> sys_base { get; set; }
    }

    public class Holdings
    {
        [JsonProperty("Barcode")]
        public string Barcode { get; set; } = "";

        [JsonProperty("BookingTooltip")]
        public string BookingTooltip { get; set; } = "";

        [JsonProperty("Cote")]
        public string Cote { get; set; } = "";

        [JsonProperty("Site")]
        public string Site { get; set; } = "";

        [JsonProperty("Type")]
        public string Type { get; set; } = "";

        [JsonProperty("Statut")]
        public string Statut { get; set; } = "";

        [JsonProperty("Holdingid")]
        public string Holdingid { get; set; } = "";

        [JsonProperty("RecordId")]
        public string RecordId { get; set; } = "";

        [JsonProperty("BaseName")]
        public string BaseName { get; set; } = "";

        [JsonProperty("isReservable")]
        public bool isReservable { get; set; } = false;

        public string StatusColor {
                        get {
                if (Statut == "En rayon")
                {
                    return "#97c67d";
                }
                return "#fdc76b";
            }
}
    }

    public class Dataa
    {
        public IList<HoldingColumn> HoldingColumns { get; set; }
        public IList<object> HoldingPlaces { get; set; }

        [JsonProperty("Holdings")]

        public List<Holdings> Holdings { get; set; }
        public IList<HoldingsStatementColumn> HoldingsStatementColumns { get; set; }
        public IList<object> HoldingsStatements { get; set; }
        public object HtmlView { get; set; }
        public ItemHoldingsData ItemHoldingsData { get; set; }
        public string ItemHoldinsDataHtmlView { get; set; }
        public object LocationSiteRestriction { get; set; }
        public object MailFormUrl { get; set; }
        public bool MultiStepReservation { get; set; }
        public object ReservationAlerts { get; set; }
        public int ReservationMode { get; set; }
        public string SearchType { get; set; }
        public int SourceType { get; set; }
        public bool TitleReservable { get; set; }
        public bool UserSubscriptionAvailable { get; set; }

        [JsonProperty("FieldList")]
        public FieldListData fieldList { get; set; }
    }
}