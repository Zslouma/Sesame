using System;
namespace Syracuse.Mobitheque.Core.Models
{
    public class HoldingItem
    {
        public string BaseName { get; set; } = "";
        public String HoldingId { get; set; } = "";
        public String RecordId { get; set; } = "";
    }

    public class PlaceReservationOptions
    {
        public HoldingItem HoldingItem { get; set; }
    }
}
