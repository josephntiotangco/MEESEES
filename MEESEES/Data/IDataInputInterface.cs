using MEESEES.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MEESEES.Data
{
    public interface IDataInputInterface
    {
        Task<IEnumerable<DataInput>> GetDataInputByUserId(int userid);
        Task<IEnumerable<DataInput>> GetDataAsync();
        Task<DataInput> GetDataInput(int id);
        Task AddDataInput(DataInput data);
        Task UpdateData(DataInput data);
        Task DeleteData(DataInput data);
    }
}
