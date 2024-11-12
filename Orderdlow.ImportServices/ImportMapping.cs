using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace EZOrders.ImportServices
{
    public class ImportMapping
    {

        private List<string> SheetColumns(ExcelWorksheet wrkSheet)
        {
            List<string> ColumnNames = new List<string>();
            for (int i = 1; i <= wrkSheet.Dimension.End.Column; i++)
            {
                ColumnNames.Add(wrkSheet.Cells[1, i].Value.ToString()); // 1 = First Row, i = Column Number
            }
            return ColumnNames;


        }
        public List<string> GetImportedMapping(string path, int importType, MemoryStream stream)
        {
            char csvDelimiter = ',';
            ImportMapping importMapping = new ImportMapping();
            using (var pck = new ExcelPackage())
            {
                ExcelWorksheet ws = null;
                if ((OrderFlowImportType)importType == OrderFlowImportType.CSV && !string.IsNullOrEmpty(path) && path.EndsWith(".csv"))
                {
                    ws = pck.Workbook.Worksheets.Add("Sheet1");
                    ExcelTextFormat format = new ExcelTextFormat()
                    {
                        Delimiter = csvDelimiter
                    };
                    ws.Cells[1, 1].LoadFromText(File.ReadAllText(path), format);

                    return SheetColumns(ws);
                }
                else
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.First(); ;
                        var rowCount = worksheet.Dimension.Rows;
                        return SheetColumns(worksheet);
                    }

                }

            }
        }


        public DataTable GetDataTableFromExcel(MemoryStream stream, bool hasHeader = true)
        {
            using (var pck = new ExcelPackage(stream))
            {

                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }

        public DataTable GetDataTableFromCSV(string path, MemoryStream stream, bool hasHeader = true)
        {
            char csvDelimiter = ',';
            using (var pck = new ExcelPackage())
            {
                ExcelWorksheet ws = null;
                ws = pck.Workbook.Worksheets.Add("Sheet1");
                ExcelTextFormat format = new ExcelTextFormat()
                {
                    Delimiter = csvDelimiter
                };
                ws.Cells[1, 1].LoadFromText(File.ReadAllText(path), format);

                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }

    }
}
