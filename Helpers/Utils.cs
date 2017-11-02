using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Exceptions
{
    public class Utils
    {
        public static string RetornarEmbeddedResourceJson()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var strJson = string.Empty;

                using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("{0}.{1}", assembly.GetName().Name, "config.json"))))
                {
                    strJson = reader.ReadToEnd();
                }

                return strJson;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
    }
}
