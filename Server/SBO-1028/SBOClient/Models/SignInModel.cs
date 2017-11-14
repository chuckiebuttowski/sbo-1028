using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SBOClient.Models
{
    public class SignInModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Msg { get; private set; }
        public User User { get; private set; }
        public string Token { get; private set; }

        public bool Login()
        {
            var repo = new RepositoryFactory().CreateUserRepository();
            User = repo.GetUser(Username, Password);
            if (User == null)
            {
                Msg = "Invalid Credentials";
                return false;
            }

            Token = CreateToken();
            Msg = "Success";
            return true;
        }

        public string CreateToken()
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var now = provider.GetNow();

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((now.AddHours(12) - unixEpoch).TotalSeconds); //12 hours expiry

            //add claims in payload for now we will leave it blank
            var payload = new Dictionary<string, object>
            {
                { "exp", expiry },
                { "user", User }
            };

            var secret = ConfigurationManager.AppSettings["jwtSecret"];
            return encoder.Encode(payload, secret);
        }
    }
}