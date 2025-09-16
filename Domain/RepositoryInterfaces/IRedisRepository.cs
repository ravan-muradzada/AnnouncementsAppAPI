using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IRedisRepository
    {
        Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null);
        Task<string?> GetStringAsync(string key);
        Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T?> GetObjectAsync<T>(string key);
        Task<bool> DeleteAsync(string key);
        Task<bool> KeyExistsAsync(string key);
        Task<long> ListPushAsync(string key, string value);
        Task<string?> ListPopAsync(string key);
        Task<bool> HashSetAsync(string key, string field, string value);
        Task<string?> HashGetAsync(string key, string field);
        Task DeleteByPattern(string pattern);
        IEnumerable<string> GetByPattern(string pattern);
    }
}
