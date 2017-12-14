using SBOClient.Core.Abstracts;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.Repositories
{
    internal class ErrorLogRepository: BaseRepository<ErrorLog>, IErrorLogRepository
    {
        public ErrorLog AddErrLog(ErrorLog err)
        {
            try
            {
                var repo = new RepositoryFactory().CreateRepository<ErrorLog>();
                ErrorLog _err = new ErrorLog();
                _err.Message = err.Message;
                _err.ErrorCode = err.ErrorCode;
                _err.StackTrace = err.StackTrace;
                _err.CreatedOn = DateTime.Now;
                _err.LastModifiedOn = DateTime.Now;
                repo.AddOrUpdate(err);

                return repo.Get(x => x.ID == _err.ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
