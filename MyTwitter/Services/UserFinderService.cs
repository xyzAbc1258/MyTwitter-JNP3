using System;
using System.Collections.Generic;
using System.Linq;
using MyTwitter.Data;
using MyTwitter.Models;
using Nest;
using MyTwitter.Common;

namespace MyTwitter.Services
{
    public class UserFinderService : IUserFinderService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUserIndexInsertionService _userIndexInsertionService;

        public UserFinderService(IElasticClient elasticClient, ApplicationDbContext applicationDbContext,
            IUserIndexInsertionService userIndexInsertionService)
        {
            _elasticClient = elasticClient;
            _applicationDbContext = applicationDbContext;
            _userIndexInsertionService = userIndexInsertionService;
        }
        public IList<ApplicationUser> GetUsers(string searchPhrase)
        {
            _userIndexInsertionService.Initialize();

            var indices = _elasticClient.Search<UserDTO>(q =>
                q.Query(d => d.Term(u => u.Verbatim().Field(x => x.Name).Value(searchPhrase)))).Documents.Select(x => x.Id).ToArray();
            var users = _applicationDbContext.ApplicationUsers.Where(x => indices.Contains(x.Id)).ToList();
            return users;
        }
    }
}
