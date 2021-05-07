using System;
using System.Globalization;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Services.Requests;

namespace Syracuse.Mobitheque.Core.Models
{
    public class UTCDateTimeConverter : Newtonsoft.Json.JsonConverter
    {
        private TimeZoneInfo pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            var pacificTime = DateTime.Parse(reader.Value.ToString());
            return TimeZoneInfo.ConvertTimeToUtc(pacificTime, pacificZone);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(TimeZoneInfo.ConvertTimeFromUtc((DateTime)value, pacificZone));
        }
    }
    public class LoansResult
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public DataLoans D { get; set; }
    }


    public class DataLoans
    {
        [JsonProperty("Loans")]
        public Loans[] Loans { get; set; }
    }
    public class DateTest
    {
        public string Date { get; set; }
    }

    public class Loans
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("RecordId")]
        public string RecordId { get; set; }

        [JsonProperty("HoldingId")]
        public string HoldingId { get; set; }

        [JsonProperty("Title")]
        public String Title { get; set; }

        [JsonProperty("CanRenew")]
        public bool CanRenew { get; set; }

        [JsonProperty("CannotRenewReason")]
        public string CannotRenewReason { get; set; }

        [JsonProperty("TypeOfDocument")]
        public String TypeOfDocument { get; set; }

        [JsonProperty("IsLate")]
        public bool IsLate { get; set; }

        [JsonProperty("IsSoonLate")]
        public bool IsSoonLate { get; set; }

        [JsonProperty("State")]
        public String State { get; set; }

        [JsonProperty("Location")]
        public String Location { get; set; }

        //[JsonProperty("WhenBack")]
        //public string CustomDate
        //{
        //    get { return WhenBack.ToString("DDMMYY"); }
        //    set
        //    {
        //        var pacificZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        //        var dateObj = new DateTest { Date = value };
        //        var jsonDate = JsonConvert.SerializeObject(dateObj.Date);
        //        var output = JsonConvert.DeserializeObject<DateTime>(jsonDate);
        //        var outputuniversal = TimeZoneInfo.ConvertTimeFromUtc((DateTime)output, pacificZone);
        //    }
        //}
        [JsonProperty("WhenBack")]
        public DateTimeOffset WhenBack { get; set; }
        [JsonIgnore]
        public string WhenBack_String
        {
            get
            {
                Console.WriteLine("Value in datetime format : " + WhenBack);
                return (DateTime.Now > WhenBack)
                    ? ApplicationResource.LoansDateLate + WhenBack.Date.ToShortDateString()
                    : ApplicationResource.LoansDateBefore + WhenBack.Date.ToShortDateString();
            }
        }

        [JsonProperty("DefaultThumbnailUrl")]
        public String DefaultThumbnailUrl { get; set; }

        [JsonProperty("ThumbnailUrl")]
        public String ThumbnailUrl { get; set; }
        [JsonIgnore]
        public String dateColor {
            get
            {
                if (DateTime.Now > WhenBack)
                    return "#FF0000"; //red
                else if (DateTime.Now > WhenBack.AddDays(7.0))
                    return "#F8CD8C"; // yellow
                return "#4FBFC3"; // vert
            }
        }

        [JsonIgnore]
        public String buttonColor
        {
            get
            {
                return (CanRenew) ? "#0066ff": "#EAEAEA";
            }
        }

        public IRequestService RequestService { get; set; }
    }
}
