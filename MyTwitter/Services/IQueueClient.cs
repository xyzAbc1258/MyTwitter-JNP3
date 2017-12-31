using MyTwitter.Models;

namespace MyTwitter.Services
{
    public interface IQueueClient
    {
        void Create(Post post);
        void Update(Post post);
    }
}
