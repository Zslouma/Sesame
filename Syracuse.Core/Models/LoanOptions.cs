using System;
namespace Syracuse.Mobitheque.Core.Models
{
    public class LoanOptions
    {
        public String serviceCode { get; set; }

        public LoansOption[] loans { get; set; }

        public String UserUniqueIdentifier { get; set; } = "";
    }

    public class LoansOption
    {
        public string HoldingId { get; set; }

        public string Id { get; set; }

        public string RecordId { get; set; }
    }
}
