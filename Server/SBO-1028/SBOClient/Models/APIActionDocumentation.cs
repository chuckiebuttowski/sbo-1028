using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOClient.Models
{
    public class APIActionDocumentation
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }

        public APIResponseDocumentation Response { get; set; }

        public IEnumerable<APIParameterDocumentation> Parameters { get; set; }
    }
}