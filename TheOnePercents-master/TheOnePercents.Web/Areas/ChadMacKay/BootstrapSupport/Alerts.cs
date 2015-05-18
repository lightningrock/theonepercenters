using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheOnePercents.Web.Areas.ChadMacKay.BootstrapSupport
{
    public static class Alerts
    {
        public const string SUCCESS = "success";
        public const string ATTENTION = "attention";
        public const string ERROR = "error";
        public const string INFORMATION = "info";
        public const string CLEAR = "";


        public static string[] ALL
        {
            get { return new[] { SUCCESS, ATTENTION, INFORMATION, ERROR, CLEAR }; }
        }
    }
}