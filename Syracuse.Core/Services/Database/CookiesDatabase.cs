using SQLite;
using Syracuse.Mobitheque.Core.Models;
using System;
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


        public async Task<List<CookiesSave>> GetItemsAsync()
        {
            return await database.Table<CookiesSave>().ToListAsync();
        }

        public async Task<CookiesSave> GetByUsernameAsync(string username)
        {
            return await database.Table<CookiesSave>().Where(p => p.Username == username).FirstOrDefaultAsync();
        }

        public async Task<CookiesSave> GetByIDAsync(string username = "")
        {
            return await database.Table<CookiesSave>().Where(p => p.Active).FirstOrDefaultAsync();
        }


        public async Task<List<CookiesSave>> GetItemsNotDoneAsync()
        {
            return await database.QueryAsync<CookiesSave>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public async Task<CookiesSave> GetItemAsync(int id)
        {
            return await database.Table<CookiesSave>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<CookiesSave> GetActiveUser()
        {
            return await database.Table<CookiesSave>().Where(i => i.Active).FirstOrDefaultAsync();
        }

        public async Task<int> AddSarchValueAsync(CookiesSave item, string searchValue)
        {
            if (item.ID != 0)
            {
                return await database.UpdateAsync(item);
            }
            else
            {
                return await database.InsertAsync(item);
            }
        }

        public async Task<int> SaveItemAsync(CookiesSave item)
        {
            if (item.ID != 0)
            {
                return await database.UpdateAsync(item);
            }
            else
            {
                return await database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(CookiesSave item)
        {
            return await database.DeleteAsync(item);
        }
        #endregion
        #region StandartViewList

        public async Task<List<StandartViewList>> GetStandartsViewsAsync()
        {
            return await database.Table<StandartViewList>().ToListAsync();
        }
        public async Task<List<StandartViewList>> GetActiveStandartView(CookiesSave ActiveUser)
        {
            return await database.Table<StandartViewList>().Where(i => i.Username == ActiveUser.Username && i.Library == ActiveUser.Library).ToListAsync(); 
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
                    if (await database.Table<StandartViewList>().Where(i => i.Username == item.Username && i.Library == item.Library && i.ViewIcone == item.ViewIcone && i.ViewName == item.ViewName && i.ViewQuery == item.ViewQuery && i.ViewScenarioCode == item.ViewScenarioCode).CountAsync() <= 0)
                    {
                        idList.Add(await database.InsertAsync(item));
                    }    
                }
            }
            return idList;
        }

        public async Task<List<int>> DeleteItemAsync(List<StandartViewList> items)
        {
            List<int> idList = new List<int>();
            foreach (var item in items)
            {
                idList.Add(await database.DeleteAsync(item));
            }
            return idList;
        }

        public async Task<bool> UpdateItemsAsync(List<StandartViewList> items, CookiesSave ActiveUser)
        {
            try
            {
                List<StandartViewList> removeStandardList = await GetActiveStandartView(ActiveUser);
                await DeleteItemAsync(removeStandardList);
                await SaveItemAsync(items);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
