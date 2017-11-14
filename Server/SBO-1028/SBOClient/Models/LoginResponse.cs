using SBOClient.Core.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOClient.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
    }
}