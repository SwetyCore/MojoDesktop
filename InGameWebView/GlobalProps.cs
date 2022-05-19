using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InGameWebView
{
    public static class GlobalProps
    {
        public static string server;
        public static string key;
        public static string home;

        public delegate void navigateTo(string url);

        public static navigateTo NavigateTo;
    }
}
