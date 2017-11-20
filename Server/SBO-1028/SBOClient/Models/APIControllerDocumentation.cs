using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOClient.Models
{
    public class APIControllerDocumentation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSecured { get; set; }
        public string BaseURL { get; set; }

        public IEnumerable<APIActionDocumentation> Actions { get; set; }
    }
}