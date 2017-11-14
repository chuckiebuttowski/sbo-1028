using SBOClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SBOClient.Controllers
{
    public class AccountController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public LoginResponse SignIn([FromBody]SignInModel model)
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                return new LoginResponse() { Message = "Username is required." };
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                return new LoginResponse() { Message = "Password is required." };
            }

            if (!model.Login())
            {
                return new LoginResponse() { Message = model.Msg };
            }
            
            return new LoginResponse() { Message = "Success", Token = model.Token, User = model.User };
        }

        //[Authorize]
        //public string Test()
        //{
        //    return "Hello word";
        //}
    }
}
