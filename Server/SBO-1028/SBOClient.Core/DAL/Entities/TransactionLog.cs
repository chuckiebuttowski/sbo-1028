using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.DAL.Entities
{
    public class TransactionLog : BaseEntity
    {
        public string Origin { get; set; }
        public string TransactionNo { get; set; }
        public SBOType Type { get; set; }
        public DateTime LogDate { get; set; }
        public bool IsPosted { get; set; }
        public int TransactionDataID { get; set; }
        public int? ErrorID { get; set; }

        [ForeignKey("TransactionDataID")]
        public virtual TransactionData RawData { get; set; }

        [ForeignKey("ErrorID")]
        public virtual ErrorLog Error { get; set; }

        public enum SBOType
        {

        }
    }
}
