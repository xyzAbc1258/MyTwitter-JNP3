using System;

namespace MyTwitter.Services
{
    public class RedisClient : IRedisClient, IDisposable
    {
        private readonly ServiceStack.Redis.IRedisClient _redisClient;

        public RedisClient(ServiceStack.Redis.IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public T GetValue<T>(string key)
        {
            return _redisClient.Get<T>(key);
        }

        public long IncrementValue(string key)
        {
            return _redisClient.IncrementValue(key);
        }

        public void SetValue<T>(string key, T value)
        {
            _redisClient.Set(key, value);
        }

        public void Dispose()
        {
            _redisClient?.Dispose();
        }
    }
}
