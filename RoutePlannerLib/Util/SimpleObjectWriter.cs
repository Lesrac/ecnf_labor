namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    public class SimpleObjectWriter
    {
        private TextWriter writer;

        public SimpleObjectWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Next(object obj)
        {
            adding(obj);
        }

        private void adding(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            BindingFlags bindingAttrs = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty;
            writer.Write("Instance of ");
            writer.Write(type.FullName);
            writer.Write("\r\n");
            foreach (PropertyInfo info in properties)
            {
                Type infoType = info.PropertyType;
                if (infoType.IsPrimitive || infoType == typeof(Decimal) || infoType == typeof(String))
                {
                    string name = info.Name;
                    object srcValue = type.InvokeMember(name, bindingAttrs, null, obj, null);
                    writer.Write(name);
                    writer.Write("=");
                    if (infoType == typeof(String))
                    {
                        writer.Write("\"");
                        writer.Write(srcValue);
                        writer.Write("\"");
                    }
                    else if (infoType == typeof(double))
                    {
                        writer.Write(string.Format("{0:F1}"), srcValue);
                    }
                    else
                    {
                        writer.Write(srcValue);
                    }
                    writer.Write("\r\n");
                }
                else
                {
                    writer.Write("Location is a nested object...\r\n");
                    adding(info.GetValue(obj));
                }
            }
            writer.Write("End of instance\r\n");
        }

    }
}
