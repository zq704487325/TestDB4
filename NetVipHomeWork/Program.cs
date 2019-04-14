using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruanmou.Model;
using Ruanmou.DB.Sqlserver;
using Ruanmou.DB.SimpleFactory;
using Ruanmou.DB.Interface;
using Ruanmou.Common;
using Ruanmou.BLL;



namespace NetVipHomeWork
{
    class Program
    {
        private static SqlServerHelper SqlServerHelp = new SqlServerHelper();
        //private static LinqToObjectBLL linqToObject = new LinqToObjectBLL();


        static void Main(string[] args)
        {
            #region 作业3
            //Company company = sqlServerHelp.Find<Company>(1);
            //User user = sqlServerHelp.Find<User>(1);
            //Console.WriteLine(company.Name + ";" + user.Name);

            //List<Company> companies = SqlServerHelp.SelectAll<Company>();
            //foreach (Company company in companies)
            //{
            //    Console.WriteLine(company.Name);
            //}

            #endregion

            #region 作业4

            //Company company = new Company();
            //company.Id = -1;
            //company.Name = "zq";
            //company.CreatorId = -1;
            //company.CreateTime = DateTime.Now;
            //company.LastModifierId = -1;
            //company.LastModifyTime = DateTime.Now;
            //// sqlServerHelp.ShowProNameAndVal<Company>(company);
            //company.ShowProNameAndVal();




            #endregion




            #region 作业5
            //Company company = new Company();
            //company.Id = 6;
            //company.Name = "zq";
            //company.CreatorId = 1;
            //company.CreateTime = DateTime.Now;
            //company.LastModifierId = 1;
            //company.LastModifyTime = DateTime.Now;

            //sqlServerHelp.Insert<Company>(company);

            //sqlServerHelp.Update<Company>(company);


            //sqlServerHelp.Delete<Company>(6);
            #endregion

            #region 作业7
            //DBSimpleFactory dBFactory = new DBSimpleFactory();
            //IDBHelper sqlserHelp = dBFactory.CreateInstanceFactory("sqlserver");
            //List<Company> companies = sqlserHelp.SelectAll<Company>();
            //foreach (var company in companies)
            //{
            //    Console.WriteLine(company.Name);
            //}
            #endregion


            #region 作业8
            //Company company = new Company();
            ////company.Id = 6;
            //company.Name = "zq";
            //company.CreatorId = 1;
            //company.CreateTime = DateTime.Now;
            //company.LastModifierId = 1;
            //company.LastModifyTime = DateTime.Now;
            //SqlServerHelp.InsertGenericCache<Company>(company);


            //Company company = new Company();
            //company.Id = 7;
            //company.Name = "华康达";
            //company.CreatorId = 1;
            //company.CreateTime = DateTime.Now;
            //company.LastModifierId = 1;
            //company.LastModifyTime = DateTime.Now;

            //SqlServerHelp.UpdateGenericCache<Company>(company);


            //SqlServerHelp.DeleteGenericCache<Company>(8);

            //Company company = SqlServerHelp.FindGenericCache<Company>(7);
            //Console.WriteLine(company.Name);

            //List<Company> companies = SqlServerHelp.SelectAllGenericCache<Company>();
            //foreach (var company in companies)
            //{
            //    Console.WriteLine(company.Name);  
            //}

            #endregion



            #region 第二次作业 作业1
            //Company company = new Company();
            //company.Id = -1;
            //company.Name = "zq";
            //company.CreatorId = -1;
            //company.CreateTime = DateTime.Now;
            //company.LastModifierId = -1;
            //company.LastModifyTime = DateTime.Now;

            //company.ShowChiProNameAndVal();
            #endregion

            #region 第二次作业 作业2
            //List<CompanyModel> cpyModList = SqlServerHelp.SelectAllGenericCacheDB<CompanyModel>();
            //foreach (var cpyMod in cpyModList)
            //{
            //    Console.WriteLine(cpyMod.Name_);
            //}

            //CompanyModel cpyMod = SqlServerHelp.FindGenericCacheDB<CompanyModel>(1);
            //Console.WriteLine(cpyMod.Name_);



            //CompanyModel companyModel = new CompanyModel();
            //companyModel.Name_ = "zq";
            //companyModel.CreatorId_ = -1;
            //companyModel.CreateTime_ = DateTime.Now;
            //companyModel.LastModifierId_ = -1;
            //companyModel.LastModifyTime_ = DateTime.Now;

            //SqlServerHelp.InsertGenericCacheDB<CompanyModel>(companyModel);


            //CompanyModel companyModel = new CompanyModel();
            //companyModel.Id = 11;
            //companyModel.Name_ = "京东";
            //companyModel.CreatorId_ = 1;
            //companyModel.CreateTime_ = DateTime.Now;
            //companyModel.LastModifierId_ = 1;
            //companyModel.LastModifyTime_ = DateTime.Now;
            //SqlServerHelp.UpdateGenericCacheDB<CompanyModel>(companyModel);


            //SqlServerHelp.DeleteGenericCacheDB<CompanyModel>(10);
            #endregion

            #region 第二次作业 作业3
            //User user = new User();
            //user.Id = 0;
            //user.Account = "111";
            //user.CompanyId = 1;
            //user.CompanyName = "百捷";
            //user.CreateTime = DateTime.Now;
            //user.CreatorId = 1;
            //user.Email = "";
            //user.LastLoginTime = DateTime.Now;
            //user.LastModifierId = 1;
            //user.Mobile = "";
            //user.Name = "zq";
            //user.Password = "123";
            //user.State = 1;
            //user.UserType = 1;
            //user.Validate<User>(out List<string> errors);


            //foreach (var error in errors)
            //{
            //    Console.WriteLine(error);
            //}
            #endregion


            #region 第四次作业 作业1

            Company company = SqlServerHelp.FindGenericCacheDBDelegate<Company>(1);

            //Console.WriteLine(company.Name);

            //List<Company> comList = SqlServerHelp.SelectAllGenericCacheDBDelegate<Company>();
            //foreach(var item in comList)
            //{
            //    Console.WriteLine(item.Name);
            //}

            // Company company = new Company();
            // //company.Id = 12;
            // company.Name = "zq";
            // company.CreatorId = 1;
            // company.CreateTime = DateTime.Now;
            // company.LastModifierId = 1;
            // company.LastModifyTime = DateTime.Now;

            //bool b= SqlServerHelp.InsertGenericCacheDBDelegate<Company>(company);

            //Company company = new Company();
            //company.Id = 12;
            //company.Name = "天猫";
            //company.CreatorId = 1;
            //company.CreateTime = DateTime.Now;
            //company.LastModifierId = 1;
            //company.LastModifyTime = DateTime.Now;
            //bool b = SqlServerHelp.UpdateGenericCacheDBDelegate<Company>(company);

            //Console.WriteLine(b);


            bool b = SqlServerHelp.DeleteGenericCacheDBDelegate<Company>(12);
            Console.WriteLine(b);
            #endregion



            #region 第四次作业 作业2
            //Company company = SqlServerHelp.FindByExp<Company>(1);
            //Console.WriteLine(company.Name);

            #endregion

            #region 第四次作业 作业3

            //Company company = SqlServerHelp.FindExprSql<Company>(1);
            //Console.WriteLine(company.Name);

            #endregion


            #region 第四次作业 作业4

            //LinqToObjectBLL.ShowResult();

            #endregion


            #region 第四次作业 作业6


            //SqlServerHelp.GetExprSqlStr();
            #endregion


            #region 第四次作业 作业7

            //LogManager.WriteLog();
            #endregion 
            Console.ReadKey();
        }
    }
}
