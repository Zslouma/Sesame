using System;
namespace Syracuse.Mobitheque.Core.Models
{
    public class BookingOptions
    {
        public String serviceCode { get; set; }

        public BookingOption[] bookings { get; set; }

        public String UserUniqueIdentifier { get; set; } = "";
    }

    public class BookingOption
    {
        public string HoldingId { get; set; }

        public string Id { get; set; }

        public string RecordId { get; set; }
    }
}
