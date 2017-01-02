using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Doge
{
    public class CustInfo
    {
        public static Regex regexCustList = new Regex("\"result\":\\[\\[(.+)\\]\\]");
        public static Regex regexCustDetail = new Regex("var data = (\\{.+\\});");

        public string CustId { get; set; }
        public float TotalAsset { get; set; }
        public float MarketValue { get; set; }
        public float TotalCash { get; set; }
        public float AvgPostion { get; set; }
        public string Waiter { get; set; }
        public string Manager { get; set; }
        public string Error { get; set; }

        private string debugInfo;
        public string DebugInfo
        {
            get
            {
                return debugInfo;
            }
            set
            {
                //如果是json的话，可能包含unicode字符，将其转化为utf8编码的json串
                if (value != null && value.StartsWith("{") && value.EndsWith("}"))
                    debugInfo = UnicodeUtils.FromUnicodeJson(value);
                else
                    debugInfo = value;
            }
        }

    }
}
