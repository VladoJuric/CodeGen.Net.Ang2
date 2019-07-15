using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using CodeGen.Web.Models;
using CodeGen.Web.Models.ViewModels;
using CodeGen.Web.Utility;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodeGen.Web.Controllers
{
    [EnableCors("AllowCors"), Produces("application/json"), Route("api/Codegen")]
    public class CodegenController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string conString;
        

        public CodegenController(IHostingEnvironment hostingEnvironment, IOptions<AppSettings> appSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            conString = appSettings.Value.CodeGenConnectionString;
        }

        #region ++++++ Database +++++++
        // api/Codegen/GetDatabaseList
        [HttpGet, Route("GetDatabaseList"), Produces("application/json")]
        public List<vmDatabase> GetDatabaseList()
        {
            var data = new List<vmDatabase>();
            using (var con = new SqlConnection(conString))
            {
                var count = 0; con.Open();
                using (var cmd = new SqlCommand("SELECT name from sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY create_date", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            count++;
                            data.Add(new vmDatabase()
                            {
                                DatabaseId = count,
                                DatabaseName = dr[0].ToString()
                            });
                        }
                    }
                }
            }
            return data.ToList();
        }

        // api/Codegen/v
        [HttpPost, Route("GetDatabaseTableList"), Produces("application/json")]
        public List<vmTable> GetDatabaseTableList([FromBody]vmParam model)
        {
            var data = new List<vmTable>();
            var conString_ = conString + " Database=" + model.DatabaseName + ";";
            using (var con = new SqlConnection(conString_))
            {
                var count = 0; con.Open();
                var schema = con.GetSchema("Tables");
                foreach (DataRow row in schema.Rows)
                {
                    count++;
                    data.Add(new vmTable()
                    {
                        TableId = count,
                        TableName = row[2].ToString()
                    });
                }
            }

            return data.ToList();
        }

        // api/Codegen/GetDatabaseTableColumnList
        [HttpPost, Route("GetDatabaseTableColumnList"), Produces("application/json")]
        public List<vmColumn> GetDatabaseTableColumnList([FromBody]vmParam model)
        {            
            return GetTableColumns(model.DatabaseName, model.TableName).ToList();
        }

        #endregion

        #region +++++ CodeGeneration +++++
        // api/Codegen/GenerateCode
        [HttpPost, Route("GenerateCode"), Produces("application/json")]
        public IActionResult GenerateCode([FromBody]vmGenerator data)
        {
            var spCollection = new List<string>();
            try
            {
                var webRootPath = _hostingEnvironment.WebRootPath; //From wwwroot
                var contentRootPath = _hostingEnvironment.ContentRootPath; //From Others

                var tblColumns = data.Columns; 
                var fullTblColumns = GetTableColumns(data.DatabaseName, data.TableName); 

                var fileContentSet = string.Empty;
                var fileContentGet = string.Empty;
                var fileContentGetByID = string.Empty;
                var fileContentPut = string.Empty;
                var fileContentDelete = string.Empty;
                var fileContentViewSp = string.Empty;
                var fileContentIEntity = string.Empty;
                var fileContentModifiedEntity = string.Empty;
                var fileContentEntities = string.Empty;
                var fileContentDtos = string.Empty;
                var fileContentCoreRepo = string.Empty;
                var fileContentView = string.Empty;
                var fileContentAPIGet = string.Empty;
                var fileContentInterface = string.Empty;
                var fileContentService = string.Empty;
                var fileContentDbContext = string.Empty;
                var fileContentUnitOfWork = string.Empty;
                var fileContentCrud = string.Empty;
                var fileModelDapper = string.Empty;
                var fileIUnitDapper = string.Empty;
                var fileRepoDapper = string.Empty;

                //SP
                fileContentSet = SpGenerator.GenerateInsertSP(tblColumns, webRootPath);
                fileContentGet = SpGenerator.GenerateSelectSP(tblColumns, webRootPath);
                fileContentGetByID = SpGenerator.GenerateSelectByIDSP(tblColumns, webRootPath);
                fileContentPut = SpGenerator.GenerateUpdateSP(tblColumns, webRootPath);
                fileContentDelete = SpGenerator.GenerateDeleteSP(tblColumns, webRootPath);
                fileContentViewSp = SpGenerator.GenerateViewSP(tblColumns, webRootPath);
                spCollection.Add(fileContentSet);
                spCollection.Add(fileContentGet);
                spCollection.Add(fileContentGetByID);
                spCollection.Add(fileContentPut);
                spCollection.Add(fileContentDelete);
                spCollection.Add(fileContentViewSp);

                //CORE
                fileContentIEntity = VmGenerator.GenerateIEntity(webRootPath);
                fileContentModifiedEntity = VmGenerator.GenerateModifiedEntity(webRootPath);
                fileContentEntities = VmGenerator.GenerateEntities(fullTblColumns, webRootPath);
                fileContentDtos = VmGenerator.GenerateDtos(tblColumns, webRootPath);
                fileContentCoreRepo = VmGenerator.GenerateIRepository(tblColumns, webRootPath);
                spCollection.Add(fileContentIEntity);
                spCollection.Add(fileContentModifiedEntity);
                spCollection.Add(fileContentEntities);
                spCollection.Add(fileContentDtos);
                spCollection.Add(fileContentCoreRepo);

                //VU
                fileContentView = ViewGenerator.GenerateForm(tblColumns, webRootPath);
                spCollection.Add(fileContentView);

                //API
                fileContentAPIGet = APIGenerator.GenerateAPIGet(tblColumns, webRootPath);
                spCollection.Add(fileContentAPIGet);

                //API SERVICE/INTERFACE
                fileContentInterface = SIGenerator.GenerateInterfaces(tblColumns, webRootPath);
                fileContentService = SIGenerator.GenerateService(tblColumns, webRootPath);
                spCollection.Add(fileContentInterface);
                spCollection.Add(fileContentService);

                //EF REPO
                fileContentDbContext = RepoEFGenerator.GenerateDbContext(tblColumns, webRootPath);
                fileContentUnitOfWork = RepoEFGenerator.GenerateUnitOfWork(webRootPath);
                fileContentCrud = RepoEFGenerator.GenerateCrud(tblColumns, webRootPath);
                spCollection.Add(fileContentDbContext);
                spCollection.Add(fileContentUnitOfWork);
                spCollection.Add(fileContentCrud);

                //DAPPER REPO
                fileModelDapper = DapperGenerator.GenerateModel(tblColumns, webRootPath);
                fileIUnitDapper = DapperGenerator.GenerateUnitOfWork(tblColumns, webRootPath);
                fileRepoDapper = DapperGenerator.GenerateRepository(tblColumns, webRootPath);
                spCollection.Add(fileModelDapper);
                spCollection.Add(fileIUnitDapper);
                spCollection.Add(fileRepoDapper);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return Json(new
            {
                spCollection
            });
        }

        #endregion

        #region PRIVATE METHODS

        private List<vmColumn> GetTableColumns(string databaseName, string tableName)
        {
            var data = new List<vmColumn>();
            var conString_ = conString + " Database=" + databaseName + ";";
            using (var con = new SqlConnection(conString_))
            {
                var count = 0;
                con.Open();
                using (var cmd =
                    new SqlCommand(
                        "SELECT COLUMN_NAME, DATA_TYPE, ISNULL(CHARACTER_MAXIMUM_LENGTH,0), IS_NULLABLE, TABLE_SCHEMA FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" +
                        tableName + "' ORDER BY ORDINAL_POSITION", con))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            count++;
                            data.Add(new vmColumn()
                            {
                                ColumnId = count,
                                ColumnName = dr[0].ToString(),
                                DataType = dr[1].ToString(),
                                MaxLength = dr[2].ToString(),
                                IsNullable = dr[3].ToString(),
                                Tablename = tableName,
                                TableSchema = dr[4].ToString()
                            });
                        }
                    }
                }

                return data;
            }
        }

        #endregion
    }
}
