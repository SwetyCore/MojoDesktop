using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace InGameWebView
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private class MojoParam
        {
            public string d = "";
            public string k2 = "";
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 1)
            {
                Console.WriteLine(HttpUtility.UrlDecode(e.Args[0], System.Text.Encoding.GetEncoding(936)));


                Uri arg_url;
                var arg = Uri.TryCreate(e.Args[0], UriKind.Absolute, out arg_url);
                GlobalProps.MojoServer = arg_url.GetComponents(UriComponents.Fragment, UriFormat.Unescaped);
                MojoParam mp = JsonConvert.DeserializeObject<MojoParam>(GlobalProps.MojoServer);
                GlobalProps.key = mp.k2;
                GlobalProps.server = mp.d + "/mojoplus/api";
                GlobalProps.home = "https://gc-mojoconsole.github.io/zh-cn/console.html";

                MessageBox.Show(GlobalProps.key, GlobalProps.server);
            }
        }
    }
}
