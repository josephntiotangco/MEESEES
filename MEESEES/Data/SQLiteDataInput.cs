using MEESEES.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MEESEES.Data
{
    public class SQLiteDataInput : IDataInputInterface
    {
        private SQLiteAsyncConnection _connection;
        public SQLiteDataInput(ISQLiteDb db, string mode)
        {
            _connection = db.GetConnection();
            if(mode == "reset")
            {
                _connection.DropTableAsync<DataInput>();
                _connection.CreateTableAsync<DataInput>();
            }
            else
            {
                _connection.CreateTableAsync<DataInput>();
            }

        }
        public async Task<IEnumerable<DataInput>> GetDataInputByUserId(int userid)
        {
            return await _connection.Table<DataInput>().Where(x => x.UserId == userid).ToListAsync();
        }
        public async Task<IEnumerable<DataInput>> GetDataAsync()
        {
            return await _connection.Table<DataInput>().ToListAsync();
        }
        public async Task DeleteData(DataInput data)
        {
            await _connection.DeleteAsync(data);
        }
        public async Task AddDataInput(DataInput data)
        {
            await _connection.InsertAsync(data);
        }
        public async Task UpdateData(DataInput data)
        {
            await _connection.UpdateAsync(data);
        }
        public async Task<DataInput> GetDataInput(int id)
        {
            return await _connection.FindAsync<DataInput>(id);
        }
    }
}
