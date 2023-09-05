using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServerExample
{
    public class HttpRequest
    {
        public string Method = String.Empty;
        public string Url = String.Empty;
        public string Host = String.Empty;
        public string UserAgent = String.Empty;
        public string Accept = String.Empty;
        private string Other { get; set; }
        private string RequestStringRaw { get; set; }

        public HttpRequest()
        {
            Method = String.Empty;
            Url = String.Empty;
            Host = String.Empty;
            UserAgent = String.Empty;
            Accept = String.Empty;
            Other = String.Empty;
            RequestStringRaw = String.Empty;
        }

        public void AddOtherHeadres(string remainingData)
        {
            /*Other = remainingData;
            RequestStringRaw = Method + Url + Host + UserAgent + Accept + Other;*/
            RequestStringRaw = remainingData;
        }

    }
}
