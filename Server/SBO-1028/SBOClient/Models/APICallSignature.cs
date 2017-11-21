using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOClient.Models
{
    public class APICallSignature
    {
        public string CallOrigin { get; set; }
        public string CallKey { get; set; }
        public string CallObjCode { get; set; }
        public string Action { get; set; }

    }
}