using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace Doge.Utils
{
    public interface IHttpRespHandler{
        void hanlde(HttpWebResponse response);
    }

    public class StringHttpRespHandler : IHttpRespHandler
    {
        public static readonly string CMD_REDIRECT = "{redirect}";
        private string result = null;
        private Encoding encode = null;

        public StringHttpRespHandler(Encoding encode)
        {
            this.encode = encode;
        }

        public string getResult()
        {
            return result;
        }

        public void hanlde(HttpWebResponse response)
        {
            string redirect = response.Headers["Location"];
            if (!string.IsNullOrEmpty(redirect))
            {
                result = CMD_REDIRECT + redirect;
            }
            else
            {
                using (Stream respStream = response.GetResponseStream())
                {
                    if (response.Headers["Content-Encoding"] == "gzip")
                    {
                        MemoryStream msTemp = new MemoryStream();
                        int count = 0;
                        GZipStream gzip = new GZipStream(respStream, CompressionMode.Decompress);
                        byte[] buf = new byte[2048];

                        while ((count = gzip.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }
                        result = encode.GetString(msTemp.ToArray());
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(respStream))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
