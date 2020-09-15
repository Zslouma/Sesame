using System;
using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{
    public class BookingResult
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public BookingData D { get; set; }
    }

    public class BookingData
    {
        [JsonProperty("Bookings")]
        public Booking[] Bookings { get; set; }
    }

    public class Booking
    {
        public string DefaultThumbnailUrl { get; set; }
        public string HoldingId { get; set; }
        public string Id { get; set; }
        public bool IsInUserBasket { get; set; }
        public string RecordId { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Title { get; set; }
        public string TitleLink { get; set; }
        public string TitleSort { get; set; }
        public string TypeOfDocument { get; set; }
        public object UserComment { get; set; }
        public object AvailabilityDate { get; set; }
        public object AvailableUntilDate { get; set; }
        public DateTime BookingDate { get; set; }
        public bool CanCancel { get; set; }
        public object CannotCancelReason { get; set; }
        public string HoldingPlace { get; set; }
        public bool IsAvailable { get; set; }
        public string LocationLabel { get; set; }
        public string Rank { get; set; }
        public int RankSort { get; set; }

        public string BookingDateString {
            get
            {
                String ret = "Le " + this.BookingDate.ToString(System.Globalization.CultureInfo.CurrentCulture);
                return ret;
            }
        }
    }
}
