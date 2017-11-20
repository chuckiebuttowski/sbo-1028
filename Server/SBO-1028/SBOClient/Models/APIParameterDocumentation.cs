using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOClient.Models
{
    public class APIParameterDocumentation
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsOptional { get; set; }
    }
}