using Domain.RepositoryInterfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }

        public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
            => await _database.StringSetAsync(key, value, expiry);

        public async Task<string?> GetStringAsync(string key)
            => await _database.StringGetAsync(key);

        public async Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            return await _database.StringSetAsync(key, json, expiry);
        }

        public async Task<T?> GetObjectAsync<T>(string key)
        {
            var json = await _database.StringGetAsync(key);
            return json.HasValue ? JsonSerializer.Deserialize<T>(json!) : default;
        }

        public async Task<bool> DeleteAsync(string key)
            => await _database.KeyDeleteAsync(key);

        public async Task<bool> KeyExistsAsync(string key)
            => await _database.KeyExistsAsync(key);

        public async Task<long> ListPushAsync(string key, string value)
            => await _database.ListRightPushAsync(key, value);

        public async Task<string?> ListPopAsync(string key)
            => await _database.ListLeftPopAsync(key);

        public async Task<bool> HashSetAsync(string key, string field, string value)
            => await _database.HashSetAsync(key, field, value);

        public async Task<string?> HashGetAsync(string key, string field)
            => await _database.HashGetAsync(key, field);
    }
}
