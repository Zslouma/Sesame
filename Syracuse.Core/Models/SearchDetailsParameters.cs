
namespace Syracuse.Mobitheque.Core.Models
{
    public class SearchDetailsParameters
    {
        public SearchResult[] parameter { get; set; }
        public SearchOptions searchOptions { get; set; }
        public string nbrResults { get; set; }
    }
}
