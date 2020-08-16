using MEESEES.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MEESEES.Data
{
    public class SQLiteUser : IUserInterface
    {
        private SQLiteAsyncConnection _connection;
        public SQLiteUser(ISQLiteDb db)
        {
            _connection = db.GetConnection();
            _connection.CreateTableAsync<User>();
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _connection.Table<User>().ToListAsync();
        }
        public async Task<IEnumerable<User>> GetUserByUsername(string username)
        {
            return await _connection.Table<User>().Where(c => c.Username == username).ToListAsync();
        }
        public async Task<IEnumerable<User>> GetUserById(int id)
        {
            return await _connection.Table<User>().Where(c => c.Id == id).ToListAsync();
        }
        public async Task DeleteUser(User user)
        {
            await _connection.DeleteAsync(user);
        }
        public async Task AddUser(User user)
        {
            await _connection.InsertAsync(user);
        }
        public async Task UpdateUser(User user)
        {
            await _connection.UpdateAsync(user);
        }
        public async Task<User> GetUser(int id)
        {
            return await _connection.FindAsync<User>(id);
        }
    }
}
