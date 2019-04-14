using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruanmou.DB.Sqlserver;
using Ruanmou.Model;

namespace Ruanmou.BLL
{
    public class LinqToObjectBLL
    {
        private static SqlServerHelper sqlServerHelp = new SqlServerHelper();

        public static  void ShowResult()
        {

            List<Company> comList = sqlServerHelp.SelectAllGenericCacheDBDelegate<Company>();
            List<User> userList = sqlServerHelp.SelectAllGenericCacheDBDelegate<User>();


            //var result = comList.Where(item => item.Id > 3).Take(2);

            //var result = comList.Select(item => new {Name =item.Id+ item.Name });

            //var result = comList.TakeWhile(item=>item.Name=="百捷");

            //var result = comList.Skip(2).Take(2);

            //var result = comList.OrderBy(item => item.Name).ThenBy(item => item.Id);

            //var result = comList.Join(userList, item => item.CreatorId, item2 => item2.Id,(item,itemUser)=> item.CreatorId.Equals(itemUser.Id))???;


            //var result = comList.GroupBy(item => item.Id);

            //var result = comList.Distinct();

            //var result = comList.Union(comList);??

            //foreach (var key in result)
            //{
            //    Console.WriteLine(key.Name);
            //    //foreach (var item in key)
            //    //{
            //    //    Console.WriteLine(item.Name);
            //    //}
            //}
        }

    }
}
