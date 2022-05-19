using InGameWebView.Utils;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using static InGameWebView.Utils.APIHandler;
using System.Windows.Media;

namespace InGameWebView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string internalUrl = "https://gc-mojoconsole.github.io/?lang=zh-cn&device_type=pc&ext=%7b%22loc%22%3a%7b%22x%22%3a512.050537109375%2c%22y%22%3a198.72210693359376%2c%22z%22%3a171.29444885253907%7d%2c%22platform%22%3a%22WinST%22%7d&game_version=CNRELWin2.6.0_R6708157_S7320343_D6731353&plat_type=pc";
        ResourceHandler resourceHandler;
        Routes routes = new Routes();
        public MainWindow()
        {
            InitializeComponent();

            webview.CoreWebView2InitializationCompleted += WebView2_CoreWebView2InitializationCompleted;
        }
        private void WebView2_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {

            initWebView2();
            webview.Source = new Uri(internalUrl);
            //webview.DefaultBackgroundColor = Color.Transparent;

        }
        void initWebView2()
        {
            webview.CoreWebView2.Settings.IsPinchZoomEnabled = false;
            webview.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;
            webview.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            resourceHandler = new ResourceHandler(webview.CoreWebView2.Environment);
            APIHandler.env = webview.CoreWebView2.Environment;
            webview.CoreWebView2.WebResourceRequested += WebView_WebResourceRequested;
            //webview.CoreWebView2.NavigationStarting += NavigationStarting;



            //internalApi = new InternalApi(resourceHandler);

        }
        private void WebView_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            e.Response = handleRequest(e.Request);
            e.GetDeferral().Complete();
        }
        public CoreWebView2WebResourceResponse handleRequest(CoreWebView2WebResourceRequest request)
        {
            var host = "https://gc-mojoconsole.github.io/";
            var nhost = "https://127.0.0.1:25565/";
            if (request.Uri.StartsWith(host))
            {
                string path = request.Uri.Substring(host.Length, request.Uri.Length - host.Length);

                if (path.StartsWith("mojoplus/api"))
                {
                    //request.Uri = $"https://{nhost}{path}";


                    return routes.DoAction(path,request);
                    //return internalApi.ApiReqHandler(request);
                }

            }


            return resourceHandler.FromFilePath("", "", true);
        }


        private bool _show;

        public bool Show
        {
            get { return _show; }
            set 
            {
                _show = value;
                if (value)
                {
                    Background = new SolidColorBrush( Color.FromArgb(31, 250, 250, 250));
                    btn.Visibility = Visibility.Hidden;
                    mask.Visibility = Visibility.Visible;

                    this.Activate();
                }
                else
                {
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    btn.Visibility = Visibility.Visible;
                    mask.Visibility = Visibility.Hidden;


                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Show = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Escape)
            {
                Show = false;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Show = false;
            new WindowHelper(this).Enable();
            GlobalProps.NavigateTo = Navigate;
        }
        private void Navigate(string url)
        {
            webview.CoreWebView2.Navigate(url);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GlobalProps.home = "https://gc-mojoconsole.github.io/zh-cn/console.html#";
            GlobalProps.key = key.Text;
            GlobalProps.server = addr.Text;

            GlobalProps.NavigateTo(GlobalProps.home);



        }
    }
}
