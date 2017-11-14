using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.DAL.Entities
{
    public class User : BaseEntity
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Image { get; set; }
        public Boolean IsActive { get; set; }
    }
}
