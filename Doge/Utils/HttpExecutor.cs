using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Doge.Utils
{
    public class HttpExecutor
    {
        private CookieContainer cookieManager = new CookieContainer();
        private Encoding encode = Encoding.GetEncoding("utf-8");
        private static readonly string USER_AGENT = "User-Agent: Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.2; WOW64; Trident/6.0; .NET4.0E; .NET4.0C; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729; McAfee; Tablet PC 2.0; MAARJS)";

        public HttpExecutor(string encoding) 
        {
            if (!string.IsNullOrEmpty(encoding)) 
            {
                encode = Encoding.GetEncoding(encoding);
            }
        }

        public void Post(string url, string body, IHttpRespHandler respHandler)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.UserAgent = USER_AGENT;
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.CookieContainer = cookieManager;
            request.AllowAutoRedirect = false;

            if (!string.IsNullOrEmpty(body))
            {
                byte[] bodyBytes = encode.GetBytes(body);
                request.ContentLength = bodyBytes.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bodyBytes, 0, bodyBytes.Length);
                }
            }
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            resp.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
            respHandler.hanlde(resp);
        }

        public string Post(string url, string body)
        {
            StringHttpRespHandler respHandler = new StringHttpRespHandler(encode);
            Post(url, body, respHandler);
            return respHandler.getResult();
        }

        public void Get(string url, string[] kv, IHttpRespHandler respHandler)
        {
            if (kv != null && kv.Length > 1)
            {
                StringBuilder queryString = new StringBuilder();
                for (int i = 0; i < kv.Length; i += 2)
                {
                    queryString.Append("&").Append(kv[i]).Append("=").Append(kv[i + 1]);
                }
                if (!url.Contains("?"))
                {
                    queryString[0] = '?';
                }
                url += queryString.ToString();
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.Headers.Add("Accept-Encoding: gzip, deflate");
            request.UserAgent = USER_AGENT;
            request.CookieContainer = cookieManager;
            request.AllowAutoRedirect = false;
            request.Timeout = 3000;

            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            resp.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
            respHandler.hanlde(resp);
        }

        public string Get(string url, string[] kv)
        {
            StringHttpRespHandler respHandler=new StringHttpRespHandler(encode);
            Get(url, kv, respHandler);
            return respHandler.getResult();
        }

    }
}
