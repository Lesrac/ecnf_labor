namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Runtime.InteropServices;
    using Microsoft.Office.Interop.Excel;

    public sealed class ExcelExchange
    {
        public void WriteToFile(String fileName, City from, City to, List<Link> links)
        {
            Application app = new Application();
            if (app == null)
            {
                throw new InvalidComObjectException("Excel is not properly installed");
            }
            Workbook wb = app.Workbooks.Add();
            Worksheet ws = wb.Worksheets.get_Item(0);
            ws.Range["A1"].Value = "From";
            ws.Range["B1"].Value = "To";
            ws.Range["C1"].Value = "Distance";
            ws.Range["D1"].Value = "Transport Mode";

            Range formatRange;
            formatRange = ws.Range["A1", "D1"];
            formatRange.EntireRow.Font.Bold = true;
            formatRange.EntireRow.Font.Size = 14;

            formatRange.BorderAround2(XlLineStyle.xlContinuous, XlBorderWeight.xlThin);

            int row = 2;
            links.ForEach( l => {
                int localRow = row;
                ws.Range["A" + localRow].Value = l.FromCity.Name;
                ws.Range["B" + localRow].Value = l.ToCity.Name;
                ws.Range["C" + localRow].Value = l.Distance;
                ws.Range["D" + localRow].Value = l.TransportMode;
            });
        }
    }
}
