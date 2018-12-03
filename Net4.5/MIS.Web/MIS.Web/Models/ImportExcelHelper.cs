using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MIS.Web.Models
{
    public class ImportExcelHelper
    {
        public static IEnumerable<T> ImportDataFromFile<T>(string fileName)
        {
            try
            {
                var targetFile = new FileInfo(fileName);

                if (!targetFile.Exists)
                {
                    return null;
                }


                var excelFile = new ExcelQueryFactory(fileName);
                var tsheet = excelFile.Worksheet<T>(0);

                var query = from p in tsheet
                            select p;

                return query;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //对应列头
            //excelFile.AddMapping<Spl_PersonModel>(x => x.Name, "Name");
            //excelFile.AddMapping<Spl_PersonModel>(x => x.Sex, "Sex");
            //excelFile.AddMapping<Spl_PersonModel>(x => x.Age, "Age");
            //excelFile.AddMapping<Spl_PersonModel>(x => x.IDCard, "IDCard");
            //excelFile.AddMapping<Spl_PersonModel>(x => x.Phone, "Phone");

            //SheetName


            //int rowIndex = 1;

            ////检查数据正确性
            //foreach (var row in excelContent)
            //{
            //    var errorMessage = new StringBuilder();
            //    var person = new Spl_PersonModel();

            //    person.Id =
            //    person.Name = row.Name;
            //    person.Sex = row.Sex;
            //    person.Age = row.Age;
            //    person.IDCard = row.IDCard;
            //    person.Phone = row.Phone;
            //    person.Email = row.Email;
            //    person.Address = row.Address;
            //    person.Region = row.Region;
            //    person.Category = row.Category;

            //    if (string.IsNullOrWhiteSpace(row.Name))
            //    {
            //        errorMessage.Append("Name - 不能为空. ");
            //    }

            //    if (string.IsNullOrWhiteSpace(row.IDCard))
            //    {
            //        errorMessage.Append("IDCard - 不能为空. ");
            //    }

            //    //=============================================================================
            //    if (errorMessage.Length > 0)
            //    {
            //        errors.Add(string.Format(
            //            "第 {0} 列发现错误：{1}{2}",
            //            rowIndex,
            //            errorMessage,
            //            "<br/>"));
            //    }
            //    personList.Add(person);
            //    rowIndex += 1;
            //}
            //if (errors.Count > 0)
            //{
            //    return false;
            //}
            //return true;
        }

        public static DataTable ExcelToDataTable(FileUpload fileUpload, string sheetName, ref string errMsg)
        {
            if (!fileUpload.HasFile)
            {
                errMsg = "请选择上传的Excel表格文件！";
                return null;
            }

            string path = HttpContext.Current.Server.MapPath(@"~/temp/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string newFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(fileUpload.FileName);
            fileUpload.SaveAs(path + newFileName);

            string strConn = string.Empty;
            switch (Path.GetExtension(fileUpload.FileName))
            {
                case ".xlsx":
                    strConn = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + path + newFileName + ";Extended Properties='Excel 12.0 Xml; HDR=YES; IMEX=1'";
                    break;
                case ".xls":
                    strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + path + newFileName + ";Extended Properties='Excel 8.0; HDR=YES; IMEX=1'";
                    break;
                default:
                    errMsg = "上传的文件不是Excel文件！";
                    return null;
            }

            try
            {
                DataTable dtTemp = new DataTable();//获取列名的表格
                string strExcelTemp = string.Format("select top 1 * from [{0}$]", sheetName);//获取列名的查询语句

                DataTable dt = new DataTable();
                string strExcel = string.Format("select * from [{0}$]", sheetName);
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcelTemp, strConn.Replace("HDR=YES", "HDR=NO")))
                    {
                        myCommand.Fill(dtTemp);
                    }

                    if (dtTemp != null && dtTemp.Rows.Count == 1)
                    {
                        //前三列不为空
                        strExcel += " where " + dtTemp.Rows[0][0] + " is not null and " + dtTemp.Rows[0][1] + " is not null and " + dtTemp.Rows[0][0] + " is not null ";
                        using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn))
                        {
                            myCommand.Fill(dt);
                        }
                    }
                    else
                    {
                        using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn))
                        {
                            myCommand.Fill(dt);
                        }
                    }
                }

                return dt;
            }
            catch (Exception e)
            {
                errMsg = e.Message.Replace("'", "");
                //throw (e);
                return null;
            }
            finally
            {
                if (File.Exists(path + newFileName))
                {
                    File.Delete(path + newFileName);
                }
            }
        }
    }
}