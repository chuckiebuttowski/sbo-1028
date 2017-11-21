using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.DAL.Entities
{
    public class ClientAPI : BaseEntity
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Action { get; set; }
        public string SboTransactionType { get; set; }
        public PostDataValueType ValueType { get; set; }

        public virtual List<APIParameter> Params { get; set; }

        public enum PostDataValueType
        {
            SingleEntity,
            MultipleEntities
        }
    }
}
