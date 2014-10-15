namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;
    using System.Reflection;

    public sealed class SimpleObjectWriter
    {
        TextWriter writer;

        public SimpleObjectWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Next(object o)
        {
            WriteObject(o);
        }


        private void WriteObject(object o)
        {
            writer.Write("Instance of ");
            Type objectType = o.GetType();
            writer.WriteLine(objectType.FullName);
            
            foreach (PropertyInfo pi in objectType.GetProperties())
            {
                Type propType = pi.PropertyType;
                if (propType == typeof(string))
                {
                    writer.Write(pi.Name);
                    writer.Write("=\"");
                    writer.Write(pi.GetValue(o));
                    writer.Write("\"");
                    writer.WriteLine();
                }
                else if (propType == typeof(int))
                {
                    writer.Write(pi.Name);
                    writer.Write("=");
                    writer.Write(pi.GetValue(o));
                    writer.WriteLine();
                }
                else if (propType == typeof(double))
                {
                    writer.Write(pi.Name);
                    writer.Write("=");
                    writer.Write(string.Format("{0:F1}", pi.GetValue(o)));
                    writer.WriteLine();
                }
                else
                {
                    writer.Write(pi.Name);
                    writer.Write(" is a nested object...");
                    writer.WriteLine();
                    WriteObject(pi.GetValue(o));
                }
            }
            writer.WriteLine("End of instance");
        }
    }
}
