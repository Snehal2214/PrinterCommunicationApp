
using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FileBrowserLibrary
{
    public class FileBrowserService
    {
        public string SelectedFileName { get; set; }

        public DataTable BrowseAndReadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls|CSV Files|*.csv|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;

                SelectedFileName = Path.GetFileName(filePath);

                var fileExtension = Path.GetExtension(filePath).ToLower();

                try
                {
                    if (fileExtension == ".xlsx" || fileExtension == ".xls")
                    {
                        return ReadExcelFile(filePath);
                    }
                    else if (fileExtension == ".csv")
                    {
                        return ReadCsvFile(filePath);
                    }
                    else
                    {
                        throw new NotSupportedException("File type not supported");
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception($"Error reading file: {ex.Message}");
                }
            }

            return null;
        }



        private DataTable ReadExcelFile(string filePath)
        {
            var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);
            return worksheet.RangeUsed().AsTable().AsNativeDataTable();
        }

        private DataTable ReadCsvFile(string filePath)
        {
            var dataTable = new DataTable();
            var reader = new StreamReader(filePath);

            // Reading header row
            var headers = reader.ReadLine()?.Split(',');
            if (headers != null)
            {
                foreach (var header in headers)
                    dataTable.Columns.Add(header);
            }

            // Reading data rows
            while (!reader.EndOfStream)
            {
                var rows = reader.ReadLine()?.Split(',');
                dataTable.Rows.Add(rows);
            }

            return dataTable;
        }
    }
}
