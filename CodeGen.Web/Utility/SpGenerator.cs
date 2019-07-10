using Microsoft.AspNetCore.Hosting;
using CodeGen.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Web.Utility
{
    public class SpGenerator
    {
        //CREATE
        public static dynamic GenerateInsertSP(List<vmColumn> tblColumns, string contentRootPath)
        {
            StringBuilder builderPrm = new StringBuilder();
            StringBuilder builderBody = new StringBuilder();
            StringBuilder builderCheck = new StringBuilder();
            builderPrm.Clear(); builderBody.Clear(); builderCheck.Clear();

            string path = @"" + contentRootPath + "\\template\\StoredProcedure\\InsertSP.txt";
            string fileContent = string.Empty; string fileld = string.Empty; string fileldPrm = string.Empty; string queryPrm = string.Empty;
            string tableName = SingleName(tblColumns[0].Tablename.ToString()); string tableSchema = tblColumns[0].TableSchema;
            string dateTime = DateTime.Now.ToString(); string genName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string genVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string spName = ("[" + tableSchema + "].[Insert" + tableName + "]").ToString();

            builderPrm.AppendLine();
            builderPrm.Append("  @Id INT OUTPUT,");
            foreach (var item in tblColumns)
            {
                fileld = fileld + item.ColumnName + ",";
                fileldPrm = fileldPrm + "@" + item.ColumnName + ",";

                //parameter
                builderPrm.AppendLine();
                if ((item.DataType.ToString() == "nvarchar") || (item.DataType.ToString() == "varchar"))
                    builderPrm.Append("  @" + item.ColumnName + " VARCHAR(" + item.MaxLength + "),");
                else
                    builderPrm.Append("  @" + item.ColumnName + " " + item.DataType.ToUpper() + ",");
            }

            queryPrm = builderPrm.Remove((builderPrm.Length - 1), 1).AppendLine().ToString();
            //queryPrm = builderPrm.ToString().TrimEnd(',');

            //Check
            builderCheck.Append("SELECT * FROM [" + tableSchema + "].[" + tableName + "]");

            //Body
            builderBody.Append("INSERT INTO [" + tableSchema + "].[" + tableName + "](");
            builderBody.Append(fileld.TrimEnd(',') + ") ");
            //builderBody.AppendLine();
            builderBody.Append("VALUES (" + fileldPrm.TrimEnd(',') + ")");
            builderBody.AppendLine();
            builderBody.Append("                SELECT @Id = Id FROM [" + tableSchema + "].[" + tableName + "] WHERE Id = SCOPE_IDENTITY();");

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#Name", spName.ToString())
                    .Replace("#Param", queryPrm.ToString())
                    .Replace("#TableCheck", builderCheck.ToString())
                    .Replace("#Body", builderBody.ToString())
                    .Replace("#OnlyName", "Insert" + tableName)
                    .Replace("#DateTime", dateTime)
                    .Replace("#GenName", genName)
                    .Replace("#GenVersion", genVersion);
            }

            return fileContent.ToString();
        }

        //READ
        public static dynamic GenerateSelectSP(List<vmColumn> tblColumns, string contentRootPath)
        {
            StringBuilder builderPrm = new StringBuilder();
            StringBuilder builderBody = new StringBuilder();
            StringBuilder builderCheck = new StringBuilder();
            builderPrm.Clear(); builderBody.Clear(); builderCheck.Clear();

            string path = @"" + contentRootPath + "\\template\\StoredProcedure\\ReadSP.txt";
            string fileContent = string.Empty; string fileld = string.Empty; string fileldPrm = string.Empty; string queryPrm = string.Empty;
            string tableName = SingleName(tblColumns[0].Tablename.ToString()); string tableSchema = tblColumns[0].TableSchema;
            string dateTime = DateTime.Now.ToString(); string genName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string genVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string spName = ("[" + tableSchema + "].[Select" + tableName + "]").ToString();
            foreach (var item in tblColumns)
            {
                fileld = fileld + item.ColumnName + ",";
                fileldPrm = fileldPrm + item.ColumnName + ",";
            }

            //Check
            builderCheck.Append("SELECT * FROM [" + tableSchema + "].[" + tableName + "]");

            //Body
            builderBody.Append("SELECT " + fileldPrm.TrimEnd(',') + " FROM [" + tableSchema + "].[" + tableName + "]");

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#Name", spName.ToString())
                    .Replace("#TableCheck", builderCheck.ToString())
                    .Replace("#Body", builderBody.ToString())
                    .Replace("#OnlyName", "Select" + tableName)
                    .Replace("#OrdPrm", fileldPrm.TrimEnd(',').ToString())
                    .Replace("#DateTime", dateTime)
                    .Replace("#GenName", genName)
                    .Replace("#GenVersion", genVersion);
            }

            return fileContent.ToString();
        }

        //READ BY ID
        public static dynamic GenerateSelectByIDSP(List<vmColumn> tblColumns, string contentRootPath)
        {
            StringBuilder builderPrm = new StringBuilder();
            StringBuilder builderBody = new StringBuilder();
            StringBuilder builderCheck = new StringBuilder();
            builderPrm.Clear(); builderBody.Clear(); builderCheck.Clear();

            string path = @"" + contentRootPath + "\\template\\StoredProcedure\\ReadByID.txt";
            string fileContent = string.Empty; string fileld = string.Empty; string fileldPrm = string.Empty; string queryPrm = string.Empty;
            string tableName = SingleName(tblColumns[0].Tablename.ToString()); string tableSchema = tblColumns[0].TableSchema;
            string dateTime = DateTime.Now.ToString(); string genName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string genVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string spName = ("[" + tableSchema + "].[Select" + tableName + "ById]").ToString();
            foreach (var item in tblColumns)
            {
                fileld = fileld + item.ColumnName + ",";
                fileldPrm = fileldPrm + item.ColumnName + ",";
            }

            //Check
            builderCheck.Append("SELECT * FROM [" + tableSchema + "].[" + tableName + "]");

            //Body
            builderBody.Append("SELECT " + fileldPrm.TrimEnd(',') + " FROM [" + tableSchema + "].[" + tableName + "]");

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#Name", spName.ToString())
                    .Replace("#TableCheck", builderCheck.ToString())
                    .Replace("#Body", builderBody.ToString())
                    .Replace("#OnlyName", "Select" + tableName + "ById")
                    .Replace("#OrdPrm", fileldPrm.TrimEnd(',').ToString())
                    .Replace("#DateTime", dateTime)
                    .Replace("#GenName", genName)
                    .Replace("#GenVersion", genVersion);
            }

            return fileContent.ToString();
        }

        //UPDATE
        public static dynamic GenerateUpdateSP(List<vmColumn> tblColumns, string contentRootPath)
        {
            StringBuilder builderPrm = new StringBuilder();
            StringBuilder builderBody = new StringBuilder();
            StringBuilder builderCheck = new StringBuilder();
            builderPrm.Clear(); builderBody.Clear(); builderCheck.Clear();

            string path = @"" + contentRootPath + "\\template\\StoredProcedure\\UpdateSP.txt";
            string fileContent = string.Empty; string fileld = string.Empty; string fileldPrm = string.Empty; string queryPrm = string.Empty;
            string tableName = SingleName(tblColumns[0].Tablename.ToString()); string tableSchema = tblColumns[0].TableSchema;
            string dateTime = DateTime.Now.ToString(); string genName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string genVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string spName = ("[" + tableSchema + "].[Update" + tableName + "]").ToString();

            builderPrm.AppendLine();
            builderPrm.Append("  @Id INT OUTPUT,");
            builderPrm.AppendLine();
            builderPrm.Append("  @RowVersion [TIMESTAMP] OUTPUT,");
            builderPrm.AppendLine();
            builderPrm.Append("  @Id INT,");
            foreach (var item in tblColumns)
            {
                if (item.ColumnName.ToLower() == "id") continue;
                fileld = fileld + item.ColumnName + ",";
                fileldPrm = fileldPrm + item.ColumnName + " = @" + item.ColumnName + ", ";

                //parameter
                builderPrm.AppendLine();
                if ((item.DataType.ToString() == "nvarchar") || (item.DataType.ToString() == "varchar"))
                    builderPrm.Append("  @" + item.ColumnName + " VARCHAR(" + item.MaxLength + "),");
                else
                    builderPrm.Append("  @" + item.ColumnName + " " + item.DataType.ToUpper() + ",");
            }

            queryPrm = builderPrm.Remove((builderPrm.Length - 1), 1).AppendLine().ToString();

            //Check
            builderCheck.Append("SELECT * FROM [" + tableSchema + "].[" + tableName + "]");

            //Body
            builderBody.Append("UPDATE [" + tableSchema + "].[" + tableName + "] SET " + fileldPrm.TrimEnd(',') + " WHERE Id = @Id");

            //Return Values
            builderBody.AppendLine();
            builderBody.Append("                SELECT @Id = Id, @RowVersion = RowVersion FROM [" + tableSchema + "].[" + tableName + "] WHERE Id = SCOPE_IDENTITY();");

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#Name", spName.ToString())
                    .Replace("#Param", queryPrm.ToString())
                    .Replace("#TableCheck", builderCheck.ToString())
                    .Replace("#Body", builderBody.ToString())
                    .Replace("#OnlyName", "Update" + tableName)
                    .Replace("#OrdPrm", fileldPrm.ToString())
                    .Replace("#DateTime", dateTime)
                    .Replace("#GenName", genName)
                    .Replace("#GenVersion", genVersion);
            }

            return fileContent.ToString();
        }

        //DELETE
        public static dynamic GenerateDeleteSP(List<vmColumn> tblColumns, string contentRootPath)
        {
            StringBuilder builderPrm = new StringBuilder();
            StringBuilder builderBody = new StringBuilder();
            StringBuilder builderCheck = new StringBuilder();
            builderPrm.Clear(); builderBody.Clear(); builderCheck.Clear();

            string path = @"" + contentRootPath + "\\template\\StoredProcedure\\DeleteSP.txt";
            string fileContent = string.Empty; string fileld = string.Empty; string fileldPrm = string.Empty; string queryPrm = string.Empty;
            string tableName = SingleName(tblColumns[0].Tablename.ToString()); string tableSchema = tblColumns[0].TableSchema;
            string dateTime = DateTime.Now.ToString(); string genName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string genVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string spName = ("[" + tableSchema + "].[Delete" + tableName + "]").ToString();

            builderPrm.AppendLine();
            builderPrm.Append("  @Id INT OUTPUT,");
            builderPrm.AppendLine();
            builderPrm.Append("  @RowVersion [TIMESTAMP] OUTPUT,");
            builderPrm.AppendLine();
            builderPrm.Append("  @Id INT,");
            foreach (var item in tblColumns)
            {
                if(item.ColumnName.ToLower() == "id") continue;
                fileld = fileld + item.ColumnName + ",";
                fileldPrm = fileldPrm + item.ColumnName + ",";

                //parameter
                builderPrm.AppendLine();
                if ((item.DataType.ToString() == "nvarchar") || (item.DataType.ToString() == "varchar"))
                    builderPrm.Append("  @" + item.ColumnName + " VARCHAR(" + item.MaxLength + "),");
                else
                    builderPrm.Append("  @" + item.ColumnName + " " + item.DataType.ToUpper() + ",");
            }

            queryPrm = builderPrm.Remove((builderPrm.Length - 1), 1).AppendLine().ToString();

            //Check
            builderCheck.Append("SELECT * FROM [" + tableSchema + "].[" + tableName + "]");

            //Body
            builderBody.Append("DELETE FROM [" + tableSchema + "].[" + tableName + "] WHERE Id = @Id");

            //Return Values
            builderBody.AppendLine();
            builderBody.Append("                SELECT @Id = Id, @RowVersion = RowVersion FROM [" + tableSchema + "].[" + tableName + "] WHERE Id = SCOPE_IDENTITY();");

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#Name", spName.ToString())
                    .Replace("#Param", queryPrm.ToString())
                    .Replace("#TableCheck", builderCheck.ToString())
                    .Replace("#Body", builderBody.ToString())
                    .Replace("#OnlyName", "Delete" + tableName)
                    .Replace("#OrdPrm", fileldPrm.ToString())
                    .Replace("#DateTime", dateTime)
                    .Replace("#GenName", genName)
                    .Replace("#GenVersion", genVersion);
            }

            return fileContent.ToString();
        }

        //VIEW
        public static dynamic GenerateViewSP(List<vmColumn> tblColumns, string contentRootPath)
        {
            StringBuilder builderPrm = new StringBuilder();
            StringBuilder builderBody = new StringBuilder();
            StringBuilder builderCheck = new StringBuilder();
            builderPrm.Clear(); builderBody.Clear(); builderCheck.Clear();

            string path = @"" + contentRootPath + "\\template\\StoredProcedure\\ViewSP.txt";
            string fileContent = string.Empty; string fileld = string.Empty; string fileldPrm = string.Empty; string queryPrm = string.Empty;
            string tableName = SingleName(tblColumns[0].Tablename.ToString()); string tableSchema = tblColumns[0].TableSchema;
            string dateTime = DateTime.Now.ToString(); string genName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string genVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string spName = ("[" + tableSchema + "].[View" + tableName + "]").ToString();
            foreach (var item in tblColumns)
            {
                fileld = fileld + item.ColumnName + ",";
                fileldPrm = fileldPrm + item.ColumnName + ",";

                //parameter
                builderPrm.AppendLine();
                if ((item.DataType.ToString() == "nvarchar") || (item.DataType.ToString() == "varchar"))
                    builderPrm.Append("  @" + item.ColumnName + " VARCHAR(" + item.MaxLength + "),");
                else
                    builderPrm.Append("  @" + item.ColumnName + " " + item.DataType + ",");
            }

            queryPrm = builderPrm.Remove((builderPrm.Length - 1), 1).AppendLine().ToString();

            //Check
            builderCheck.Append("SELECT * FROM [" + tableSchema + "].[" + tableName + "]");

            //Body
            builderBody.Append("SELECT " + fileldPrm.TrimEnd(',') + " FROM [" + tableSchema + "].[" + tableName + "]");

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#Name", spName.ToString())
                    .Replace("#Param", queryPrm.ToString())
                    .Replace("#TableCheck", builderCheck.ToString())
                    .Replace("#Body", builderBody.ToString())
                    .Replace("#OnlyName", "View" + tableName)
                    .Replace("#OrdPrm", fileldPrm.ToString())
                    .Replace("#DateTime", dateTime)
                    .Replace("#GenName", genName)
                    .Replace("#GenVersion", genVersion);
            }

            return fileContent.ToString();
        }

        public static string SingleName(string tableName)
        {
            // if finish on ies replace with y
            if (tableName.Substring(tableName.Length - 3) == "ies")
                tableName = tableName.Substring(0, tableName.Length - 3) + "y";
            else
            // if finish on xes replace with es
            if (tableName.Substring(tableName.Length - 3) == "xes" ||
                tableName.Substring(tableName.Length - 3) == "oes" ||
                tableName.Substring(tableName.Length - 3) == "ses"
            )
                tableName = tableName.Substring(0, tableName.Length - 2);
            else
            // if finish on 's' replace it
            if (tableName.Substring(tableName.Length - 1) == "s" &&
                tableName.Substring(tableName.Length - 2) != "is")
                tableName = tableName.Substring(0, tableName.Length - 1);

            return tableName;
        }
    }
}
