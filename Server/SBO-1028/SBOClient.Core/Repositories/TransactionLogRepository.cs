using SBOClient.Core.Abstracts;
using SBOClient.Core.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.Repositories
{
    internal class TransactionLogRepository : BaseRepository<TransactionLog>, ITransactionLogRepository
    {
        public void RetryPosting(int id)
        {
            //get log by id here and try post it again
        }
    }
}
