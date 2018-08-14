using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using BePreferenceCentre.DAL;
using System.Data;
using System.Reflection;

namespace BePreferenceCentre.Helpers
{
    public static class ExcelHelper
    {
        public static IEnumerable<InkeyAnswer> PopulateAttendees(ExcelWorksheet workSheet, bool firstRowHeader)
        {
            IList<InkeyAnswer> questions = new List<InkeyAnswer>();

            if (workSheet != null)
            {
                Dictionary<string, int> header = new Dictionary<string, int>();

                for (int rowIndex = workSheet.Dimension.Start.Row; rowIndex <= workSheet.Dimension.End.Row; rowIndex++)
                {
                    //Assume the first row is the header. Then use the column match ups by name to determine the index.
                    //This will allow you to have the order of the columns change without any affect.

                    if (rowIndex == 1 && firstRowHeader)
                    {
                        header = ExcelHelper.GetExcelHeader(workSheet, rowIndex);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ExcelHelper.ParseWorksheetValue(workSheet, header, rowIndex, "Answer")))
                        {
                            questions.Add(new InkeyAnswer
                            {
                                Question = ParseWorksheetValue(workSheet, header, rowIndex, "Question"),
                                Answer = ParseWorksheetValue(workSheet, header, rowIndex, "Answer"),
                                Segmentation = ParseWorksheetValue(workSheet, header, rowIndex, "SEGMENTATION"),
                                Concern = ParseWorksheetValue(workSheet, header, rowIndex, "CONCERN"),
                                ProductName = ParseWorksheetValue(workSheet, header, rowIndex, "PRODUCT"),
                                ProductLink = ParseWorksheetValue(workSheet, header, rowIndex, "ProductLink"),
                                InstructionsForUse = "NA",
                                ProductImageLink = "NA"

                            });
                        }

                    }
                }
            }

            return questions;
        }



        public static Dictionary<string, int> GetExcelHeader(ExcelWorksheet workSheet, int rowIndex)
        {
            Dictionary<string, int> header = new Dictionary<string, int>();

            if (workSheet != null)
            {
                for (int columnIndex = workSheet.Dimension.Start.Column; columnIndex <= workSheet.Dimension.End.Column; columnIndex++)
                {
                    if (workSheet.Cells[rowIndex, columnIndex].Value != null)
                    {
                        string columnName = workSheet.Cells[rowIndex, columnIndex].Value.ToString();

                        if (!header.ContainsKey(columnName) && !string.IsNullOrEmpty(columnName))
                        {
                            header.Add(columnName, columnIndex);
                        }
                    }
                }
            }

            return header;
        }

        ///<summary>
        /// Parse worksheet values based on the information given.
        /// </summary>

        /// <param name="workSheet"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string ParseWorksheetValue(ExcelWorksheet workSheet, Dictionary<string, int> header, int rowIndex, string columnName)
        {
            string value = string.Empty;
            int? columnIndex = header.ContainsKey(columnName) ? header[columnName] : (int?)null;

            if (workSheet != null && columnIndex != null && workSheet.Cells[rowIndex, columnIndex.Value].Value != null)
            {
                value = workSheet.Cells[rowIndex, columnIndex.Value].Value.ToString();
            }

            return value;
        }


        public static DataTable ToDataTable(this ExcelPackage package)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }
            for (var rowNumber = 3; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {

                var row = workSheet.Cells[rowNumber, 1, rowNumber, 4];

                if (!string.IsNullOrEmpty(row[row.Count(), 1].Value.ToString()))
                {
                    var newRow = table.NewRow();
                    foreach (var cell in row)
                    {
                        if (!string.IsNullOrEmpty(cell.Text))
                        {
                            newRow[cell.Start.Column - 1] = cell.Text;
                        }

                    }
                    table.Rows.Add(newRow);
                }

            }
            return table;
        }


        public static T ConvertToEntity<T>(this DataRow tableRow) where T : new()
        {
            // Create a new type of the entity I want
            Type t = typeof(T);
            T returnObject = new T();

            foreach (DataColumn col in tableRow.Table.Columns)
            {
                string colName = col.ColumnName;

                // Look for the object's property with the columns name, ignore case
                PropertyInfo pInfo = t.GetProperty(colName.ToLower(),
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // did we find the property ?
                if (pInfo != null)
                {
                    object val = tableRow[colName];

                    // is this a Nullable<> type
                    bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (IsNullable)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            // Convert the db type into the T we have in our Nullable<T> type
                            val = Convert.ChangeType
                    (val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                        }
                    }
                    else
                    {
                        // Convert the db type into the type of the property in our entity
                        val = Convert.ChangeType(val, pInfo.PropertyType);
                    }
                    // Set the value of the property with the value from the db
                    pInfo.SetValue(returnObject, val, null);
                }
            }

            // return the entity object with values
            return returnObject;
        }

        public static List<T> ConvertToList<T>(this DataTable table) where T : new()
        {
            Type t = typeof(T);

            // Create a list of the entities we want to return
            List<T> returnObject = new List<T>();

            // Iterate through the DataTable's rows
            foreach (DataRow dr in table.Rows)
            {
                // Convert each row into an entity object and add to the list
                T newRow = dr.ConvertToEntity<T>();
                returnObject.Add(newRow);
            }

            // Return the finished list
            return returnObject;
        }
    }
}