using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker.Model
{
    public class JEModel
    {
        public int groupId { get; set; }
        public int regionId { get; set; }
        public int areaId { get; set; }
        public string branchCode { get; set; }
        public int trans_id { get; set; }
        public string trans_refno { get; set; }
        public string trans_sub_code { get; set; }
        public string trans_sub_name { get; set; }
        public string trans_inv_code { get; set; }
        public string trans_inv_name { get; set; }
        public string trans_bank { get; set; }
        public string trans_checkno { get; set; }
        public DateTime trans_date { get; set; }
        public DateTime trans_due { get; set; }
        public string trans_ccode { get; set; }
        public int trans_qtyno { get; set; }
        public double trans_cost { get; set; }
        public double trans_fixed { get; set; }
        public string trans_remar { get; set; }
        public string status { get; set; }

        public List<JEModelSub> JESubs { get; set; }
    }
}
