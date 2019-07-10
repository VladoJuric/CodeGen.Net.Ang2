using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Web.Models.ViewModels
{
    public class vmGenerator
    {
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public List<vmColumn> Columns { get; set; }
    }
}
