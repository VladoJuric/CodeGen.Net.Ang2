using CodeGen.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace CodeGen.Web.Utility
{
    public class VmGenerator
    {
        public static dynamic GenerateIEntity(string contentRootPath)
        {
            string fileContent = string.Empty;

            string path = @"" + contentRootPath + "\\template\\Core\\iEntityModel.txt";

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent.ToString();
        }

        public static dynamic GenerateModifiedEntity(string contentRootPath)
        {
            string fileContent = string.Empty;

            string path = @"" + contentRootPath + "\\template\\Core\\modifiedEntityModel.txt";

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent.ToString();
        }

        public static dynamic GenerateEntities(List<vmColumn> tblColumns, string contentRootPath)
        {
            StringBuilder builderPrm = new StringBuilder();
            builderPrm.Clear();
            string fileContent = string.Empty; string queryPrm = string.Empty;
            string reginIEntity = string.Empty;
            List<string> otherTable = new List<string>();

            string tableName = SingleName(tblColumns[0].Tablename); string tableSchema = tblColumns[0].TableSchema;
            string path = @"" + contentRootPath + "\\template\\Core\\entitiesModel.txt";
            string pathEntity = @"" + contentRootPath + "\\template\\Core\\reginIEntity.txt";
            string className = SingleName(tableName.ToString());
            foreach (var item in tblColumns)
            {
                if (item.ColumnName.ToLower() == "id" || 
                    item.ColumnName.ToLower() == "datecreated" || 
                    item.ColumnName.ToLower() == "createdby" || 
                    item.ColumnName.ToLower() == "datemodified" ||
                    item.ColumnName.ToLower() == "modifiedby" ||
                    item.ColumnName.ToLower() == "rowversion" ||
                    item.ColumnName.ToLower() == "") continue;
                if (item.ColumnName.ToLower().EndsWith("id"))
                {
                    var otherTableName = item.ColumnName.Substring(0, item.ColumnName.Length - 2);
                    otherTable.Add("    public " + otherTableName + " " + otherTableName + " { get; set; }");
                }
                //parameter
                builderPrm.AppendLine();
                //if ((item.DataType.ToString() == "nvarchar") || (item.DataType.ToString() == "varchar"))
                //    builderPrm.Append("[Column(TypeName = \" VARCHAR(" + item.MaxLength + ")\")]");
                //if ((item.DataType.ToString() == "int") && (item.ColumnName.ToString().ToUpper() == "ID"))
                //    builderPrm.Append("[Key]");
                builderPrm.Append("    public " + TypeMap.GetClrType(item.DataType) + " " + item.ColumnName + " { get; set; }");
            }

            if (otherTable.Count > 0)
            {
                builderPrm.AppendLine();
                builderPrm.AppendLine();
                builderPrm.Append("    #region Navigation properties");
                foreach (var ot in otherTable)
                {
                    builderPrm.AppendLine();
                    builderPrm.Append(ot);
                }
                builderPrm.AppendLine();
                builderPrm.Append("    #endregion");
            }

            queryPrm = builderPrm.AppendLine().ToString();

            using (StreamReader sr = new StreamReader(pathEntity, Encoding.UTF8))
            {
                reginIEntity = sr.ReadToEnd();
            }

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#ClassName", className.ToString())
                    .Replace("#Properties", queryPrm.ToString())
                    .Replace("#reginIEntity", reginIEntity.ToString());
            }

            return fileContent.ToString();
        }

        public static dynamic GenerateDtos(List<vmColumn> tblColumns, string contentRootPath)
        {
            StringBuilder builderPrm = new StringBuilder();
            builderPrm.Clear();
            string fileContent = string.Empty; string queryPrm = string.Empty;
            List<string> otherTable = new List<string>();

            string tableName = SingleName(tblColumns[0].Tablename); string tableSchema = tblColumns[0].TableSchema;
            string path = @"" + contentRootPath + "\\template\\Core\\dtosModel.txt";
            string className = SingleName(tableName.ToString()) + "Dto";
            foreach (var item in tblColumns)
            {
                if (item.ColumnName.ToLower() == "id") continue;
                if (item.ColumnName.ToLower().EndsWith("id"))
                {
                    var otherTableName = item.ColumnName.Substring(0, item.ColumnName.Length - 2);
                    otherTable.Add("    public " + otherTableName + " " + otherTableName + " { get; set; }");
                }
                //parameter
                builderPrm.AppendLine();
                builderPrm.Append("    public " + TypeMap.GetClrType(item.DataType) + " " + item.ColumnName + " { get; set; }");
            }

            if (otherTable.Count > 0)
            {
                builderPrm.AppendLine();
                builderPrm.AppendLine();
                builderPrm.Append("    #region Navigation properties");
                foreach (var ot in otherTable)
                {
                    builderPrm.AppendLine();
                    builderPrm.Append(ot);
                }
                builderPrm.AppendLine();
                builderPrm.Append("    #endregion");
            }

            queryPrm = builderPrm.AppendLine().ToString();

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#ClassName", className.ToString())
                    .Replace("#Properties", queryPrm.ToString());
            }

            return fileContent.ToString();
        }

        public static dynamic GenerateIRepository(List<vmColumn> tblColumns, string contentRootPath)
        {
            StringBuilder builderPrm = new StringBuilder();
            builderPrm.Clear();
            string fileContent = string.Empty; string queryPrm = string.Empty;

            string tableName = SingleName(tblColumns[0].Tablename); string tableSchema = tblColumns[0].TableSchema;
            string path = @"" + contentRootPath + "\\template\\Core\\repositoryModel.txt";
            string className = "I" + SingleName(tableName.ToString()) + "Repository";
            string name = tableName;
            string allName = tableName + "s";
            string lowerName = tableName.ToLower();

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#ClassName", className.ToString())
                    .Replace("#Name", name.ToString())
                    .Replace("#AllName", allName.ToString())
                    .Replace("#LowerName", lowerName.ToString());
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
