using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SBOClient.Controllers
{
    [Authorize]
    [RoutePrefix("api/transaction-log")]
    public class TransactionLogController : ApiController
    {
        [HttpGet]
        [Route("get-all")]
        public IEnumerable<TransactionLog> GetAll()
        {
            try
            {
                var logs = new List<TransactionLog>();

                logs.Add(new TransactionLog() { Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.APV, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = true });
                logs.Add(new TransactionLog() { Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.BP, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = true });
                logs.Add(new TransactionLog() { Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.GI, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = true });
                logs.Add(new TransactionLog() { Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.GL, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = false });
                logs.Add(new TransactionLog() { Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.GR, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = false });
                logs.Add(new TransactionLog() { Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.GRPO, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = false });
                logs.Add(new TransactionLog() { Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.IP, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = false });

                return logs;

                //var repo = new RepositoryFactory().CreateTransactionLogRepository();

                //return repo.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
