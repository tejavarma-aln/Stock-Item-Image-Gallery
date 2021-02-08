using System;
using System.IO;
using System.Net;
using System.Text;

namespace ImageGallery
{
    class NetworkHandle
    {

        private string payLoad;
        private string url;

        public NetworkHandle(string payLoad, string url)
        {
            this.payLoad = payLoad;
            this.url = url;
        }

        public string DoPost()
        {
            string response = null;
            try
            {
                var data = Encoding.UTF8.GetBytes(payLoad);
                var req = WebRequest.Create(url) as HttpWebRequest;
                req.Method = "POST";
                req.ContentType = "text/xml";
                req.ContentLength = data.Length;
                using (var req_stream = req.GetRequestStream())
                {
                    req_stream.Write(data, 0, data.Length);
                }
                using (var res_stream = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    response = res_stream.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                //log.Error(e.StackTrace);
                Console.WriteLine(e.Message);
            }

            return response;
        }
    }

}

