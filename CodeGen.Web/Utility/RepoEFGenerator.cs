using CodeGen.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Web.Utility
{
    public class RepoEFGenerator
    {
        public static dynamic GenerateUnitOfWork(string contentRootPath)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string fileContent = string.Empty;

            string path = @"" + contentRootPath + "\\template\\EFRepository\\UnitOfWorkModel.txt";
            string name = "[NAME]DbContext";

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd().Replace("#Name", name);

            }

            return fileContent.ToString();
        }

        public static dynamic GenerateDbContext(List<vmColumn> tblColumns, string contentRootPath)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string fileContent = string.Empty;

            string dbTableName = SingleName(tblColumns[0].Tablename);
            string path = @"" + contentRootPath + "\\template\\EFRepository\\DbContextModel.txt";
            string name = "[NAME]DbContext";
            string tableName = dbTableName + "s";

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#Name", name)
                    .Replace("#TableName", tableName)
                    .Replace("#dbTableName", dbTableName);
            }

            return fileContent.ToString();
        }
        
        public static dynamic GenerateCrud(List<vmColumn> tblColumns, string contentRootPath)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string fileContent = string.Empty;

            string dbTableName = SingleName(tblColumns[0].Tablename);
            string path = @"" + contentRootPath + "\\template\\EFRepository\\CrudModel.txt";
            string dbContextName = "[NAME]DbContext";
            string tableName = dbTableName + "s";
            string className = SingleName(dbTableName.ToString()) + "Repository";
            string iClassName = "I" + SingleName(dbTableName.ToString()) + "Repository";
            string varAllName = tableName.ToLower();
            string varName = dbTableName.ToLower();

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#ClassName", className)
                    .Replace("#IClassName", iClassName)
                    .Replace("#DbContextName", dbContextName)
                    .Replace("#Name", dbTableName)
                    .Replace("#varAllName", varAllName)
                    .Replace("#TableName", tableName)
                    .Replace("#varName", varName);
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
