namespace MyTwitter.Services
{
    public interface IRedisClient
    {
        T GetValue<T>(string key);
        long IncrementValue(string key);
        void SetValue<T>(string key, T value);
    }
}
