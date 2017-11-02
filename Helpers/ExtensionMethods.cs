using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace eNotasGW.Client.Lib.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsJson(this string text)
        {
            return text.Trim().ToLower().Equals("application/json");
        }
    }
}
