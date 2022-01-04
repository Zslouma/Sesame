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
            // cette table contient toutes les informations des utilisateur de l'app
            database.CreateTableAsync<CookiesSave>().Wait();
            // cette table contients tout les pages suplémentaire programé par le client
            database.CreateTableAsync<StandartViewList>().Wait();

        }
        #region CookiesSave


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
        #endregion
        #region StandartViewList

        public Task<List<StandartViewList>> GetStandartsViewsAsync()
        {
            return database.Table<StandartViewList>().ToListAsync();
        }
        public Task<List<StandartViewList>> GetActiveStandartView(CookiesSave ActiveUser)
        {
            return database.Table<StandartViewList>().Where(i => i.Library == ActiveUser.Library && i.Username == ActiveUser.Username ).ToListAsync(); 
        }

        public async Task<List<int>> SaveItemAsync(List<StandartViewList> items)
        {

            List<int> idList = new List<int>();
            foreach (var item in items)
            {
                if (item.ID != 0)
                {
                    idList.Add(await database.UpdateAsync(item));
                }
                else
                {
                    idList.Add(await database.InsertAsync(item));
                }
            }
            return idList;
        }

        public Task<int> DeleteItemAsync(StandartViewList item)
        {
            return database.DeleteAsync(item);
        }
        #endregion
    }
}
