using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GES.Common.Models;

namespace GES.Common.Helpers
{
    /// <summary>
    ///     Helper class to read data from Excel file.
    /// </summary>
    public class ExcelReaderHelpers
    {
        private readonly bool _firstRowAsColumnName;

        public ExcelReaderHelpers(bool firstRowAsColumnName = true)
        {
            _firstRowAsColumnName = firstRowAsColumnName;
        }

        private DataSet GetDataReader(Stream fileStream, string fileExtension)
        {
            IExcelDataReader dataReader;

            if (fileExtension.Equals(".xls"))
            {
                dataReader = ExcelReaderFactory.CreateBinaryReader(fileStream);
            }
            else if (fileExtension.Equals(".xlsx"))
            {
                dataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
            }
            else
            {
                throw new ArgumentException("File is on an Excel file.");
            }

            dataReader.IsFirstRowAsColumnNames = _firstRowAsColumnName;

            return dataReader.AsDataSet();
        }

        private DataTable GetExcelWorksheet(string worksheetName, Stream fileStream, string fileExtension)
        {
            var dataset = GetDataReader(fileStream, fileExtension);

            var dataTable = string.IsNullOrWhiteSpace(worksheetName) ? dataset.Tables[0] : dataset.Tables[worksheetName];

            if (dataTable != null)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    col.ColumnName = col.ColumnName.Trim();
                }
            }
            else
            {
                throw new NoNullAllowedException(string.Format("The worksheet {0} does not contain data.", worksheetName));
            }
            return dataTable;
        }

        /// <summary>
        /// Parse Excel data using ExcelDataReader lib
        /// </summary>
        /// <param name="worksheetName">Worksheet name</param>
        /// <param name="fileStream"></param>
        /// <param name="fileExtension"></param>
        /// <returns>Parsed data into DataRow(s)</returns>
        public IEnumerable<DataRow> GetExcelReaderData(string worksheetName, Stream fileStream, string fileExtension)
        {
            var worksheetData = GetExcelWorksheet(worksheetName, fileStream, fileExtension);

            IEnumerable<DataRow> rows = from DataRow r in worksheetData.Rows select r;

            return rows;
        }

        public object GetRowValue(DataRow row, string colName, string datatype)
        {
            try
            {
                if (!row.IsNull(colName))
                {
                    switch (datatype.Trim().ToLower())
                    {
                        case "string":
                            return row.Field<string>(colName).Trim();
                        case "int16":
                            return Convert.ToInt16(row[colName]);
                        case "int32":
                            return Convert.ToInt32(row[colName]);
                        case "int64":
                            return Convert.ToInt64(row[colName]);
                        case "long":
                            return Convert.ToInt64(row[colName]);
                        case "decimal":
                            return Convert.ToDecimal(row[colName]);
                        case "datetime":
                            return Convert.ToDateTime(row[colName]);
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ExcelConfigModel> GetImportExcelConfigs(string filePath, string configName)
        {
            
            XDocument xdoc = XDocument.Load(filePath);

            var columnConfigs = (from x in xdoc.Descendants("Column")
                                 where x.Parent != null && x.Parent.Attribute("Name").Value == configName
                                 select new ExcelConfigModel
                                 {
                                     DatabaseField = x.Element("DatabaseField")?.Value,
                                     ExcelField = x.Element("ExcelField")?.Value,
                                     DataType = x.Element("DataType")?.Value
                                 }).ToList();

            return columnConfigs;
        }
    }
}