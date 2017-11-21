using SBOClient.Core.Abstracts;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOClient.Helpers
{
    public static class ErrorLogger
    {
        public static ErrorLog Log(ErrorLog err)
        {
            IErrorLogRepository repo = new RepositoryFactory().CreateErrorLogRepository();
            return repo.AddErrLog(err);
        }
    }
}