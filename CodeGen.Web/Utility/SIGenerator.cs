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
    public class SIGenerator
    {
        public static dynamic GenerateInterfaces(List<vmColumn> tblColumns, string contentRootPath)
        {
            string fileContent = string.Empty;

            string tableName = SingleName(tblColumns[0].Tablename); string tableSchema = tblColumns[0].TableSchema;
            string path = @"" + contentRootPath + "\\template\\ServiceInterface\\interfaceModel.txt";
            string className = "I" + SingleName(tableName.ToString()) + "Service";
            string nameDto = SingleName(tableName.ToString()) + "Dto";

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#ClassName", className.ToString())
                    .Replace("#NameDto", nameDto.ToString());
            }

            return fileContent.ToString();
        }

        public static dynamic GenerateService(List<vmColumn> tblColumns, string contentRootPath)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            StringBuilder builderPrm = new StringBuilder();
            StringBuilder builderSub = new StringBuilder();
            builderPrm.Clear(); builderSub.Clear();
            string fileContent = string.Empty; string queryPrm = string.Empty; string submitPrm = string.Empty;

            string tableName = SingleName(tblColumns[0].Tablename); string tableSchema = tblColumns[0].TableSchema;
            string path = @"" + contentRootPath + "\\template\\ServiceInterface\\serviceModel.txt";

            string className = SingleName(tableName.ToString()) + "Service";
            string iClassName = "I" + SingleName(tableName.ToString()) + "Service";
            string mapper = "IMaper";
            string iRepoName = "I" + SingleName(tableName.ToString()) + "Repository";
            string iUnitWork = "IUnitOfWork";
            string name = tableName;
            string allName = tableName + "s";
            string nameDto = SingleName(tableName.ToString()) + "Dto";
            string varName = tableName.ToLower();
            string varAllName = tableName.ToLower() + "s";
            string dtoName = tableName.ToLower() + "Dto";


            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#ClassName", className)
                    .Replace("#IClassName", iClassName)
                    .Replace("#Mapper", mapper)
                    .Replace("#IRepoName", iRepoName)
                    .Replace("#IUnitWork", iUnitWork)
                    .Replace("#Name", name)
                    .Replace("#AllName",allName)
                    .Replace("#NameDto", nameDto)
                    .Replace("#varName", varName)
                    .Replace("#varAllName", varAllName)
                    .Replace("#DtoName", dtoName);
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
