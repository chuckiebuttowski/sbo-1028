using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker.Model
{
    public class JEModelSub
    {
        public int groupid { get; set; }
        public int regionid { get; set; }
        public int areaid { get; set; }
        public string branchCode { get; set; }
        public int entry_trans { get; set; }
        public string entry_ccode { get; set; }
        public DateTime entry_date { get; set; }
        public int entry_pcode { get; set; }
        public string entry_acode { get; set; }
        public double entry_dramt { get; set; }
        public double entry_cramt { get; set; }
        public string entry_remar { get; set; }
        public string entry_group { get; set; }
        public int entry_ord { get; set; }
        public string status { get; set; }
    }
}
