using MyTwitter.Models;

namespace MyTwitter.Services
{ 
    public interface IUserIndexInsertionService
    {
        void Initialize();
        void InsertAllFromSource();
        void Insert(ApplicationUser user);
    }
}
