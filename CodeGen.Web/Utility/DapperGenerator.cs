using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGen.Web.Models;

namespace CodeGen.Web.Utility
{
    public class DapperGenerator
    {
        //DAPPER I UNIT OF WORK
        public static dynamic GenerateUnitOfWork(List<vmColumn> tblColumns, string contentRootPath)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string fileContent = string.Empty;

            string dbTableName = SingleName(tblColumns[0].Tablename);
            string path = @"" + contentRootPath + "\\template\\DAPPERRepository\\UnitOfWorkModel.txt";
            string iUnitName = dbTableName + "Repository";
            string iUnitNameLower = dbTableName.ToLower() + "Repository";

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#iUnitName", iUnitName)
                    .Replace("#iUnitNmLower", iUnitNameLower);

            }

            return fileContent.ToString();
        }

        //DAPPER REPOSIORY
        public static dynamic GenerateRepository(List<vmColumn> tblColumns, string contentRootPath)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            StringBuilder builderPrm = new StringBuilder();
            builderPrm.Clear();

            string fileContent = string.Empty; string fileld = string.Empty; string fileldPrm = string.Empty; string queryPrm = string.Empty;
            string prmInputOutput = string.Empty; string tableSchema = tblColumns[0].TableSchema;

            string tableName = SingleName(tblColumns[0].Tablename);
            string path = @"" + contentRootPath + "\\template\\DAPPERRepository\\RepositoryModel.txt";
            string iUnitName = tableName + "Repository";

            foreach (var item in tblColumns)
            {
                fileld = fileld + item.ColumnName + ",";
                fileldPrm = fileldPrm + "@" + item.ColumnName + ",";

                //parameter
                builderPrm.AppendLine();
                if ((item.DataType.ToString() == "nvarchar") || (item.DataType.ToString() == "varchar"))
                    builderPrm.Append("        queryParams.Add(@\"" + item.ColumnName + "\", entity." + item.ColumnName + ", DbType.AnsiString, ParameterDirection.Input);");
                else if (item.DataType.ToString() == "int")
                    builderPrm.Append("        queryParams.Add(@\"" + item.ColumnName + "\", entity." + item.ColumnName + ", DbType.Int32, ParameterDirection.Input);");
                else if (item.DataType.ToString() == "datetime")
                    builderPrm.Append("        queryParams.Add(@\"" + item.ColumnName + "\", entity." + item.ColumnName + ", DbType.DateTime, ParameterDirection.Input);");
                else if (item.DataType.ToString() == "timestamp")
                    builderPrm.Append("        queryParams.Add(@\"" + item.ColumnName + "\", entity." + item.ColumnName + ", DbType.Binary, ParameterDirection.Input);");
                else if (item.DataType.ToString() == "bit")
                    builderPrm.Append("        queryParams.Add(@\"" + item.ColumnName + "\", entity." + item.ColumnName + ", DbType.Bool, ParameterDirection.Input);");
                else
                    builderPrm.Append("        queryParams.Add(@\"" + item.ColumnName + "\", entity." + item.ColumnName + ", DbType.[NeedChange], ParameterDirection.Input);");
            }

            builderPrm.AppendLine();
            builderPrm.Append("        queryParams.Add(\"@Id\", DbType.Int32, direction: ParameterDirection.Output);");

            prmInputOutput =
                "queryParams.Add(\"@RowVersion\", entity.RowVersion, DbType.Binary, direction: ParameterDirection.InputOutput);";

            queryPrm = builderPrm.ToString();
            //queryPrm = builderPrm.ToString().TrimEnd(',');

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#tableSchema", tableSchema)
                    .Replace("#tableName", tableName)
                    .Replace("#iUnitName", iUnitName)
                    .Replace("#Param", queryPrm)
                    .Replace("#InOut", prmInputOutput)
                    .Replace("#RepoName", tableName);
            }

            return fileContent.ToString();
        }

        //
        public static dynamic GenerateModel(List<vmColumn> tblColumns, string contentRootPath)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            StringBuilder builderPrm = new StringBuilder();
            builderPrm.Clear();

            string fileContent = string.Empty; string fileld = string.Empty; string fileldPrm = string.Empty; string queryPrm = string.Empty;

            string dbTableName = SingleName(tblColumns[0].Tablename);
            string path = @"" + contentRootPath + "\\template\\DAPPERRepository\\Model.txt";

            foreach (var item in tblColumns)
            {
                fileld = fileld + item.ColumnName + ",";
                fileldPrm = fileldPrm + "@" + item.ColumnName + ",";

                builderPrm.AppendLine();
                builderPrm.Append("    public " + TypeMap.GetClrType(item.DataType) + " " + item.ColumnName + " { get; set; }");
            }

            queryPrm = builderPrm.AppendLine().ToString();

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#Param", queryPrm)
                    .Replace("#RepoName", dbTableName);
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
