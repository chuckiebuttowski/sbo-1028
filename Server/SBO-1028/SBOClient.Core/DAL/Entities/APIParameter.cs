using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.DAL.Entities
{
    public class APIParameter : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int ApiID { get; set; }

        [ForeignKey("ApiID")]
        public virtual ClientAPI API { get; set; }
    }
}
