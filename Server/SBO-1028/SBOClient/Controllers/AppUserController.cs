using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SBOClient.Controllers
{
    [Authorize]
    [RoutePrefix("api/app-user")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AppUserController : ApiController
    {
        [HttpGet]
        [Route("get-users")]
        public IEnumerable<User> GetUsers()
        {
            var repo = new RepositoryFactory().CreateUserRepository();
            return repo.GetAll();
        }

        [HttpPost]
        [Route("add-or-update")]
        public string AddOrUpdateUser(User user)
        {
            try
            {
                var repo = new RepositoryFactory().CreateUserRepository();
                repo.AddOrUpdate(user);

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to save user. Please contact administrator.";
            }
        }

        [HttpGet]
        [Route("delete")]
        public string DeleteUser(int id)
        {
            try
            {
                var repo = new RepositoryFactory().CreateUserRepository();
                var user = repo.Get(x => x.ID == id).FirstOrDefault();

                if (user != null)
                {
                    repo.Delete(user);
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to delete user. Please contact administrator.";
            }
        }
    }
}
