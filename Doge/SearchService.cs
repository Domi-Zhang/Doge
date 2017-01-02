using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;
using Newtonsoft.Json;
using Doge.Utils;
using System.Windows;

namespace Doge
{
    class SearchService
    {
        public static readonly HttpExecutor HTTP_EXECUTOR = new HttpExecutor(null);

        public static CustBank searchCustBank(string account)
        {
            CustBank custBank = new CustBank();
            custBank.CustId = account;
            try
            {
                /*{
                "message": "成功",
                "result": [
                    [
                        "3023955",
                        "工商银行",
                        "6222024402054479260",
                        "人民币",
                        "正常",
                        "20130311",
                        "0",
                        "成都市一环路东五段证券营业部"
                    ]
                ],
                "totalCount": "1",
                "result_md": [
                    "资金账户",
                    "银行名称",
                    "银行账号",
                    "币种",
                    "账户状态",
                    "登记日期",
                    "注销日期",
                    "营业部"
                ],
                "code": "1"
                }*/
                string resp = HTTP_EXECUTOR.Post(
                                                string.Format(ConfigurationManager.AppSettings["url_cust_bank"], account),
                                                null
                                            );
                custBank.CustId = account;
                custBank.DebugInfo = resp;
                custBank.OpenDate = JsonConvert.DeserializeObject<dynamic>(resp)["result"][0][5].Value;
            }
            catch (WebException ex)
            {
                if ((ex.Response is HttpWebResponse) && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Forbidden)
                {
                    custBank.Error = "警告:登陆信息已失效，请重新启动程序";
                }
                else 
                {
                    custBank.Error = "错误：" + ex.ToString();
                }
            }
            catch (Exception ex)
            {
                custBank.Error = "查询失败: " + ex.ToString();
            }

            return custBank;
        }

        public static CustInfo searchCustInfo(string account)
        {
            CustInfo custInfo = new CustInfo();
            custInfo.CustId = account;
            try
            {
                string resp = HTTP_EXECUTOR.Post(
                                                ConfigurationManager.AppSettings["url_cust_info"],
                                                string.Format(ConfigurationManager.AppSettings["http_body_cust_info"], account)
                                            );
                custInfo.DebugInfo = resp;
                Match match = CustInfo.regexCustList.Match(resp);
                if (match.Success && match.Groups.Count > 0)
                {
                    string[] fields = match.Groups[1].Value.Split(new char[] { ',', '\"' }, StringSplitOptions.RemoveEmptyEntries);

                    if (fields.Length > 6)
                    {
                        custInfo.TotalAsset = float.Parse(fields[5]);
                        custInfo.MarketValue = float.Parse(fields[4]);
                        custInfo.TotalCash = float.Parse(fields[3]);
                        custInfo.AvgPostion = float.Parse(fields[6]);
                        custInfo.Manager = UnicodeUtils.FromUnicode(fields[7]);
                        fillCustDetail(custInfo);

                        return custInfo;
                    }
                }
                custInfo.Error = "客户列表返回数据格式错误";
            }
            catch (WebException ex)
            {
                if ((ex.Response is HttpWebResponse) && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Forbidden)
                {
                    custInfo.Error = "警告:登陆信息已失效，请重新启动程序";
                }
                else
                {
                    custInfo.Error = "错误：" + ex.ToString();
                }
            }
            catch (Exception ex)
            {
                custInfo.Error = "查询失败: " + ex.ToString();
            }

            return custInfo;
        }

        public static void fillCustDetail(CustInfo custInfo)
        {
            string detailResp = HTTP_EXECUTOR.Post(
                                                ConfigurationManager.AppSettings["url_cust_detail"],
                                                string.Format(ConfigurationManager.AppSettings["http_body_cust_detail"], custInfo.CustId)
                                            );
            Match match = CustInfo.regexCustDetail.Match(detailResp);
            if (match.Success && match.Groups.Count > 0)
            {
                custInfo.DebugInfo = match.Groups[1].Value;
                Dictionary<string, string> custDetail = JsonConvert.DeserializeObject<Dictionary<string, string>>(match.Groups[1].Value);
                if (custDetail != null)
                {
                    custInfo.Waiter = custDetail["服务人员"];
                    return;
                }
            }

            custInfo.Error = "客户详情返回数据格式错误";
        }

        public static CustTrade searchCustTrade(string account, string startMonth, string endMonth)
        {
            CustTrade custTrade = new CustTrade();
            custTrade.CustId = account;
            try
            {
                string resp = HTTP_EXECUTOR.Post(
                                                ConfigurationManager.AppSettings["url_cust_trade"],
                                                string.Format(ConfigurationManager.AppSettings["http_body_cust_trade"], account, startMonth, endMonth)
                                            );
                custTrade.DebugInfo = resp;
                custTrade.PureCommission = JsonConvert.DeserializeObject<dynamic>(resp)["result"][0][7].Value;
            }
            catch (WebException ex)
            {
                if ((ex.Response is HttpWebResponse) && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Forbidden)
                {
                    custTrade.Error = "警告:登陆信息已失效，请重新启动程序";
                }
                else
                {
                    custTrade.Error = "错误：" + ex.ToString();
                }
            }
            catch (Exception ex)
            {
                custTrade.Error = "查询失败: " + ex.ToString();
            }

            return custTrade;
        }

    }
}
