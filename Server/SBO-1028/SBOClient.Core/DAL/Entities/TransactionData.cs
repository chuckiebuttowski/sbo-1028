using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.DAL.Entities
{
    public class TransactionData : BaseEntity
    {
        public DateTime PostedOn { get; set; }
        public string RawData { get; set; }
    }
}
