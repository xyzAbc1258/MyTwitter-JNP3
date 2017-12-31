using System.Threading.Tasks;
using MyTwitter.Models;

namespace MyTwitter.Services
{
    public interface IQueueClient
    {
        Task Create(Post post);
        Task Update(Post post);
    }
}
