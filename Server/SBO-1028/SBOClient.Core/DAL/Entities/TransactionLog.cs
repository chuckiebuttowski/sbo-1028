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
            JE = 30,
            APV = 18,
            OP = 46,
            IP = 24,
            GI = 60,
            GR = 59,
            GRPO = 20,
            ITM = 4,
            BP = 2,
            GL = 1
        }
    }
}
