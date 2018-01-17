using System.Linq;
using MyTwitter.Common;
using MyTwitter.Data;
using MyTwitter.Models;
using Nest;

namespace MyTwitter.Services
{
    public class UserIndexInsertionService : IUserIndexInsertionService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ApplicationDbContext _applicationDbContext;

        public UserIndexInsertionService(IElasticClient elasticClient, ApplicationDbContext applicationDbContext)
        {
            _elasticClient = elasticClient;
            _applicationDbContext = applicationDbContext;
        }

        private static bool _initialized = false;
        public void Initialize()
        {
            if (!_initialized)
                InsertAllFromSource();
        }

        public void InsertAllFromSource()
        {
            var dtos = _applicationDbContext.ApplicationUsers.Select(x => new UserDTO
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.UserName
            }).ToList();
            _elasticClient.IndexMany(dtos);
            _initialized = true;
        }

        public void Insert(ApplicationUser user)
        {
            Initialize();
            if(! _elasticClient.Search<UserDTO>(q => q.Query(p => p.Ids(i => i.Values(new Id(user.Id))))).Documents.Any())
                _elasticClient.Index(new UserDTO {Id = user.Id, Email = user.Email, Name = user.UserName});
        }
    }
}
