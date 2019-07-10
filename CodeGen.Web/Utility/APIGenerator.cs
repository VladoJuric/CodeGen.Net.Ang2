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
    public class APIGenerator
    {
        public static dynamic GenerateAPIGet(List<vmColumn> tblColumns, string contentRootPath)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            StringBuilder builderPrm = new StringBuilder();
            StringBuilder builderSub = new StringBuilder();
            builderPrm.Clear(); builderSub.Clear();
            string fileContent = string.Empty;

            string tableName = SingleName(tblColumns[0].Tablename); string tableSchema = tblColumns[0].TableSchema;
            string path = @"" + contentRootPath + "\\template\\WebAPI\\APIController.txt";

            //Api Controller
            string routePrefix = "api/v1/" + textInfo.ToTitleCase(Conversion.RemoveSpecialCharacters(tableName.ToString()));
            string apiController = textInfo.ToTitleCase(Conversion.RemoveSpecialCharacters(tableName.ToString())) + "Controller";

            string iClassName = "I" + tableName + "Service";
            string varName = tableName.ToLower();
            string nameDto = tableName + "Dto";
            string dtoName = tableName.ToLower() + "Dto";

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                fileContent = sr.ReadToEnd()
                    .Replace("#RoutePrefix", routePrefix)
                    .Replace("#APiController", apiController)
                    .Replace("#IClassName", iClassName)
                    .Replace("#varName", varName)
                    .Replace("#NameDto", nameDto)
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
