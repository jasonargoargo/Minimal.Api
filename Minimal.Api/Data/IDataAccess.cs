using System.Data;

namespace Minimal.Api.Data
{
    public interface IDataAccess
    {
        Task<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
        Task<IEnumerable<T>> LoadAllData<T, U>(string storedProcedure, U parameters, string connectionId = "Default");
        Task<bool> SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default");
    }
}
