using SQLite;
using Syracuse.Mobitheque.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.Services.Database
{
    public class CookiesDatabase
    {
        private SQLiteAsyncConnection database { get; set; }

        public CookiesDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<CookiesSave>().Wait();
        }

        public Task<List<CookiesSave>> GetItemsAsync()
        {
            return database.Table<CookiesSave>().ToListAsync();
        }

        public Task<CookiesSave> GetByUsernameAsync(string username)
        {
            return database.Table<CookiesSave>().Where(p => p.Username == username).FirstOrDefaultAsync();
        }

        public Task<CookiesSave> GetByIDAsync(string username = "")
        {
            return database.Table<CookiesSave>().Where(p => p.Active).FirstOrDefaultAsync();
        }


        public Task<List<CookiesSave>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<CookiesSave>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<CookiesSave> GetItemAsync(int id)
        {
            return database.Table<CookiesSave>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<CookiesSave> GetActiveUser()
        {
            return database.Table<CookiesSave>().Where(i => i.Active).FirstOrDefaultAsync();
        }

        public Task<int> AddSarchValueAsync(CookiesSave item, string searchValue)
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

        public Task<int> SaveItemAsync(CookiesSave item)
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

        public Task<int> DeleteItemAsync(CookiesSave item)
        {
            return database.DeleteAsync(item);
        }
    }
}
