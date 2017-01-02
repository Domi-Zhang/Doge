using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doge
{
    public class CustBank
    {
        public string CustId { get; set; }
        public string OpenDate { get; set; }
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
