using SQLite;
using Syracuse.Mobitheque.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.Services.Database
{
    public class DocumentsDatabase
    {
        private SQLiteAsyncConnection database { get; set; }

        public Task<int> SaveItemAsync(DocumentSave item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public DocumentsDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<DocumentSave>().Wait();
        }

        public Task<int> DeleteItemAsync(DocumentSave item)
        {
            return database.DeleteAsync(item);
        }

        /// <summary>
        /// Récupére la liste des document sauvegarder en local sur le telephone
        /// </summary>
        /// <returns></returns>
        public Task<List<DocumentSave>> GetItemsAsync()
        {
            return database.Table<DocumentSave>().ToListAsync();
        }

        /// <summary>
        /// Recupére un document à partir de son id de base de donnée 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DocumentSave> GetDocumentsByID(int id)
        {
            return database.Table<DocumentSave>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Récupére une liste de document appartennant à un utilisateur spécifique
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Task<List<DocumentSave>> GetDocumentsByUserID(int userID)
        {
            return database.Table<DocumentSave>().Where(i => i.UserID == userID).ToListAsync();
        }
        /// <summary>
        ///  Récupére une liste de document appartennant à un utilisateur spécifique pour un affichage spécifique
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="classeDisplay"></param>
        /// <returns></returns>
        public Task<List<DocumentSave>> GetDocumentsByDisplay(int userID, string classeDisplay)
        {
            return database.Table<DocumentSave>().Where(i => i.UserID == userID && i.ClasseDisplay == classeDisplay).ToListAsync();
        }

    }
}
