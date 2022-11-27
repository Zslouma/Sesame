using SQLite;

namespace Syracuse.Mobitheque.Core.Models
{
    /// <summary>
    /// Cette class représente un document sauvegardé
    /// </summary>
    public class DocumentSave
    {

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int UserID { get; set; }

        public string DocumentID { get; set; } 

        public string ClasseDisplay { get; set; }

        public string ImagePath { get; set; }

        public string Title { get; set; }

        public string ShortDesc { get; set; }

        public string DocumentPath { get; set; }

        public string JsonValue { get; set; }
    }
}
