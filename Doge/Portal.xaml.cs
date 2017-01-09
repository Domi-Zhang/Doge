using Doge.Utils;
using Microsoft.Win32;
using Newtonsoft.Json;
using Noesis.Javascript;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;

namespace Doge
{
    public partial class Portal : Window
    {
        private static readonly Random random = new Random();
        private string login_passwd_salt = null;
        private static int sleepMin;
        private static int sleepMax;

        public Portal()
        {
            InitializeComponent();
            string[] sleepConfig = ConfigurationManager.AppSettings["sleep_range"].Split(',');
            sleepMin = int.Parse(sleepConfig[0]) * 1000;
            sleepMax = int.Parse(sleepConfig[1]) * 1000;

            // 登录准备
            txt_login_ready_msg.Visibility = Visibility.Visible;
            img_botcode.Visibility = Visibility.Hidden;
            ThreadPool.QueueUserWorkItem((state) => { ReadyLogin(); });
        }

        private void ReadyLogin()
        {
            //获取session_id和md5密码盐
            try
            {
                string loginPage = SearchService.HTTP_EXECUTOR.Get(ConfigurationManager.AppSettings["url_root"], null);
                int saltStart = loginPage.IndexOf(ConfigurationManager.AppSettings["passwd_salt_prefix"]);
                this.login_passwd_salt = loginPage.Substring(saltStart + ConfigurationManager.AppSettings["passwd_salt_prefix"].Length, 32);

                this.Dispatcher.Invoke(new Action(delegate
                {
                    //获取验证码
                    ImageHttpRespHandler botcodeRespHandler = new ImageHttpRespHandler();
                    SearchService.HTTP_EXECUTOR.Get(ConfigurationManager.AppSettings["url_botcode"], null, botcodeRespHandler);
                    txt_login_ready_msg.Visibility = Visibility.Hidden;
                    img_botcode.Visibility = Visibility.Visible;
                    img_botcode.Source = botcodeRespHandler.getBitmap();
                }));
            }
            catch (Exception e)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    txt_login_ready_msg.Text = "登录准备失败：" + e.Message;
                }));
            }
        }

        private void btn_tci_search_Click(object sender, RoutedEventArgs e)
        {
            btn_tci_search.Visibility = Visibility.Hidden;

            string[] allAccounts = txt_tci_accounts.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<CustInfo> results = new List<CustInfo>();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int i = 0; i < allAccounts.Length; i++)
                {
                    CustInfo custInfo = SearchService.searchCustInfo(allAccounts[i]);
                    results.Add(custInfo);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        pb_tci_main.Value = ((i + 1) * 100) / allAccounts.Length;
                    }));

                    if (i < allAccounts.Length - 1)
                    {
                        //休眠，降低查询频率
                        Thread.Sleep(random.Next(sleepMin, sleepMax));
                    }
                }
                this.Dispatcher.Invoke(new Action(() =>
                {
                    pb_tci_main.Value = 0;
                    lv_tci_main.ItemsSource = results;
                    btn_tci_search.Visibility = Visibility.Visible;
                }));
            });
        }

        private class ImageHttpRespHandler : IHttpRespHandler
        {
            private BitmapFrame bitMap = null;

            public void hanlde(HttpWebResponse response)
            {
                using (Stream respStream = response.GetResponseStream())
                {
                    if (response.Headers["Content-Encoding"] == "gzip")
                    {
                        GZipStream gzip = new GZipStream(respStream, CompressionMode.Decompress);
                        bitMap = BitmapFrame.Create(gzip);
                    }
                    else
                    {
                        bitMap = BitmapFrame.Create(respStream);
                    }
                }
            }

            public BitmapFrame getBitmap()
            {
                return bitMap;
            }
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            string passwd = null;
            using (JavascriptContext context = new JavascriptContext())
            {
                context.SetParameter("passwd_md5", "");
                context.SetParameter("passwd_salt", login_passwd_salt);
                context.SetParameter("passwd_original", txt_pwd.Password);
                context.Run(File.ReadAllText("statics/login.js"));
                passwd = context.GetParameter("passwd_md5").ToString();
            }
            string result = SearchService.HTTP_EXECUTOR.Post(
                                ConfigurationManager.AppSettings["url_login"],
                                string.Format(ConfigurationManager.AppSettings["http_body_login"], txt_username.Text, passwd, txt_botcode.Text)
                            );
            if (result.StartsWith(StringHttpRespHandler.CMD_REDIRECT))
            {
                Alert("信息", "登陆成功");
            }
            else
            {
                Alert("信息", "登陆信息有误");
            }
        }

        private static void Alert(string title, string content)
        {
            Window alert = new Window();
            alert.Title = title;
            alert.Content = content;
            alert.Width = 300;
            alert.Height = 120;
            alert.ShowDialog();
        }

        private void btn_tcb_search_Click(object sender, RoutedEventArgs e)
        {
            btn_tcb_search.Visibility = Visibility.Hidden;

            string[] allAccounts = txt_tcb_accounts.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<CustBank> results = new List<CustBank>();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int i = 0; i < allAccounts.Length; i++)
                {
                    CustBank custBank = SearchService.searchCustBank(allAccounts[i]);
                    results.Add(custBank);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        pb_tcb_main.Value = ((i + 1) * 100) / allAccounts.Length;
                    }));

                    if (i < allAccounts.Length - 1)
                    {
                        //休眠，降低查询频率
                        Thread.Sleep(random.Next(sleepMin, sleepMax));
                    }
                }
                this.Dispatcher.Invoke(new Action(() =>
                {
                    pb_tcb_main.Value = 0;
                    lv_tcb_main.ItemsSource = results;
                    btn_tcb_search.Visibility = Visibility.Visible;
                }));
            });
        }

        private void lv_copy_column(object sender, RoutedEventArgs e)
        {
            try
            {
                var table = ((ListView)sender).ItemsSource;
                string selectField = null;
                if (e.OriginalSource is GridViewColumnHeader)
                {
                    GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
                    if (clickedColumn != null && clickedColumn.DisplayMemberBinding is Binding)
                    {
                        selectField = ((Binding)clickedColumn.DisplayMemberBinding).Path.Path;
                    }
                }

                StringBuilder copyContent = new StringBuilder();
                foreach (Object row in table)
                {
                    Object value = row.GetType().GetProperty(selectField).GetValue(row, null);
                    copyContent.AppendLine(value != null ? value.ToString() : "");
                }
                Clipboard.SetDataObject(copyContent.ToString());
            }
            catch (Exception ex) 
            {
                Alert("错误", "复制内容失败：" + ex.ToString());
            }
        }

        private void btn_tct_search_Click(object sender, RoutedEventArgs e)
        {
            btn_tct_search.Visibility = Visibility.Hidden;

            string[] allAccounts = txt_tct_accounts.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<CustTrade> results = new List<CustTrade>();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                string lastMonth = string.Format("{0:yyyyMM}",DateTime.Now.AddMonths(-1));
                for (int i = 0; i < allAccounts.Length; i++)
                {
                    CustTrade custTrade = SearchService.searchCustTrade(allAccounts[i], lastMonth, lastMonth);
                    results.Add(custTrade);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        pb_tct_main.Value = ((i + 1) * 100) / allAccounts.Length;
                    }));

                    if (i < allAccounts.Length - 1)
                    {
                        //休眠，降低查询频率
                        Thread.Sleep(random.Next(sleepMin, sleepMax));
                    }
                }
                this.Dispatcher.Invoke(new Action(() =>
                {
                    pb_tct_main.Value = 0;
                    lv_tct_main.ItemsSource = results;
                    btn_tct_search.Visibility = Visibility.Visible;
                }));
            });
        }

    }
}
