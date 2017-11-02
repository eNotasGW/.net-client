using eNotasGW.Client.Lib.Data;
using eNotasGW.Client.Lib.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Exceptions.Base
{
    public abstract class GWLibException : Exception
    {
        protected string Summary { get; set; }

        public GWLibException()
        {
        }

        public GWLibErro[] Errors { get; protected set; }

        public GWLibException(string message)
            : base(message)
        {
        }

        public GWLibException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public GWLibException(GWLibErro[] errors)
        {
            this.Errors = errors;
        }

        public override string Message
        {
            get
            {
                var sb = new StringBuilder();

                sb.AppendLine(this.Summary);

                if (this.Errors != null)
                {
                    if (this.Errors.Length > 0)
                    {
                        sb.AppendFormat("\r\n{0}:\r\n", GWLibMessages.Errors);

                        foreach (var error in this.Errors)
                        {
                            sb.AppendLine(error.ToString());
                        }
                    }
                }

                return sb.ToString();
            }
        }
    }
}
