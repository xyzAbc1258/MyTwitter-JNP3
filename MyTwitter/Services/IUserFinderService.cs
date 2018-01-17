using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyTwitter.Models;

namespace MyTwitter.Services
{
    public interface IUserFinderService
    {
        IList<ApplicationUser> GetUsers(string searchPhrase);
    }
}
