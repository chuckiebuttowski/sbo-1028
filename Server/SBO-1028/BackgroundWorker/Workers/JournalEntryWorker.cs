using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker.Workers
{
    public class JournalEntryWorker
    {
        HttpClient webEndPoint;

        public JournalEntryWorker(HttpClient endPoint, string host)
        {
            this.webEndPoint = endPoint;
            webEndPoint.BaseAddress = new Uri(host);
            webEndPoint.DefaultRequestHeaders.Accept.Clear();
            webEndPoint.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public static void Work()
        {

        }
    }
}
