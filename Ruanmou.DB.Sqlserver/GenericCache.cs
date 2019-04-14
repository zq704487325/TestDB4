using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruanmou.Model;
using Ruanmou.Common;
using Ruanmou.Common.Attributees;
using System.Reflection;


namespace Ruanmou.DB.Sqlserver
{
    /// <summary>
    /// 添加数据字符串的泛型缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheInsert<T> where T : BaseModel
    {
        /// <summary>
        /// 构造函数私有化
        /// </summary>
        private GenericCacheInsert()
        {
        }
        private static string _sqlString = null;
        /// <summary>
        /// 静态构造函数执行一次
        /// </summary>
        static GenericCacheInsert()
        {
            Type type = typeof(T);

            string proesStr = string.Join(",", type.GetProperties().
                Where(item => item.Name.ToLower() != "id").Select(item => "[" + item.Name + "]"));

            string proesValStr = string.Join(",", type.GetProperties().
                Where(item => item.Name.ToLower() != "id").Select(item => "@" + item.Name));

            string sqlStr = "Insert into " + "[" + type.Name + "](" + proesStr + ")values" + "(" + proesValStr + ")";
            _sqlString = sqlStr;

        }
        /// <summary>
        /// 获取类型T对应的插入字符串
        /// </summary>
        /// <returns></returns>
        public static string GetInsertSql()
        {
            return _sqlString;
        }
    }

    /// <summary>
    /// 获取修改的字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheUpdate<T> where T : BaseModel
    {
        private GenericCacheUpdate()
        {
        }

        private static string sqlUpdateStr = null;
        static GenericCacheUpdate()
        {
            Type type = typeof(T);
            string proesStr = string.Join(",", type.GetProperties().
                Where(item => item.Name.ToLower() != "id").Select(item => "[" + item.Name + "]=" + "@" + item.Name));
            string sqlStr = "update [" + type.Name + "] set " + proesStr + " where Id=@Id";
            sqlUpdateStr = sqlStr;
        }


        public static string GetUpdStr()
        {
            return sqlUpdateStr;
        }

    }

    /// <summary>
    /// 删除实体字符串泛型缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheDel<T> where T : BaseModel
    {
        private GenericCacheDel()
        {
        }
        private static string sqlDelStr = null;
        static GenericCacheDel()
        {
            Type type = typeof(T);
            string sqlStr = "delete " + "[" + type.Name + "]" + " where id=@Id";
            sqlDelStr = sqlStr;
        }

        public static string GetDelSql()
        {
            return sqlDelStr;
        }
    }


    /// <summary>
    /// 查找字符串缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheFind<T> where T : BaseModel
    {
        private GenericCacheFind()
        {
        }
        private static string sqlFindStr = null;
        static GenericCacheFind()
        {
            Type type = typeof(T);
            string proesStr = string.Join(",", type.GetProperties().Select(item => "[" + item.Name + "]"));
            string strSql = "Select" + proesStr + "from [" + type.Name + "]where [id]=@Id";
            sqlFindStr = strSql;
        }

        public static string GetFindSqlStr()
        {
            return sqlFindStr;
        }
    }

    /// <summary>
    /// 全查sql字符串缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheSelectAll<T> where T : BaseModel
    {
        private GenericCacheSelectAll()
        {
        }
        private static string sqlSelectAllStr = null;
        static GenericCacheSelectAll()
        {
            Type type = typeof(T);
            string proesStr = string.Join(",", type.GetProperties().Select(item => "[" + item.Name + "]"));
            string sqlStr = " select " + proesStr + " from " + "[" + type.Name + "]";
            sqlSelectAllStr = sqlStr;
        }

        public static string GetSelectAllSqlStr()
        {
            return sqlSelectAllStr;
        }

    }


    #region  第二次作业 作业2

    public class GetDBName<T>
    {

        private GetDBName()
        {

        }
        private static string tableName_ = "";
        private static List<string> proNames_ = new List<string>();
        static GetDBName()
        {

            Type type = typeof(T);
            string tableName = type.Name;

            if (type.IsDefined(typeof(DBNameAttribute)))
            {
                DBNameAttribute dBNameAttribute = (DBNameAttribute)type.GetCustomAttribute(typeof(DBNameAttribute));
                tableName = dBNameAttribute.Name;
            }
            tableName_ = tableName;

            List<string> proList = new List<string>();
            foreach (var pro in GetTypeProGenericCache<T>.GetTypeProInfos())
            {
                if (pro.IsDefined(typeof(DBNameAttribute), true))
                {
                    DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                    proList.Add(dBNameAttribute.Name);
                }
                else
                {
                    proList.Add(pro.Name);
                }

            }
            proNames_ = proList;

        }
        

        public static string GetTableName()
        {
            return tableName_;
        }

        public static List<string> GetProNames()
        {

            return proNames_;

        }


    }

    /// <summary>
    /// 全查sql字符串缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheSelectAllDB<T> where T : BaseModel
    {
        private GenericCacheSelectAllDB()
        {
        }
        private static string sqlSelectAllStr = null;
        static GenericCacheSelectAllDB()
        {
            Type type = typeof(T);
            string tableName = type.Name;
            if (type.IsDefined(typeof(DBNameAttribute)))
            {
                DBNameAttribute dBNameAttribute = (DBNameAttribute)type.GetCustomAttribute(typeof(DBNameAttribute));
                tableName = dBNameAttribute.Name;
            }

            List<string> proList = new List<string>() ;
            foreach (var pro in GetTypeProGenericCache<T>.GetTypeProInfos())
            {
                if (pro.IsDefined(typeof(DBNameAttribute),true))
                {
                    DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                    proList.Add(dBNameAttribute.Name);
                }
                else
                {
                    proList.Add(pro.Name);
                }
                
            }
            string proesStr = string.Join(",",proList.Select(item => "[" + item + "]"));
            string sqlStr = " select " + proesStr + " from " + "[" + tableName+ "]";
            sqlSelectAllStr = sqlStr;
        }

        public static string GetSelectAllSqlStr()
        {
            return sqlSelectAllStr;
        }
    }

    /// <summary>
    /// 查找字符串缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheFindDB<T> where T : BaseModel
    {
        private GenericCacheFindDB()
        {
        }
        private static string sqlFindStr = null;
        static GenericCacheFindDB()
        {
            //Type type = typeof(T);
            try
            {
                string proesStr = string.Join(",", GetDBName<T>.GetProNames().Select(item => "[" + item + "]"));
                string strSql = "Select" + proesStr + "from [" + GetDBName<T>.GetTableName() + "]where [id]=@Id";
                sqlFindStr = strSql;
            }
            catch (Exception e)
            {
                LogManager.WriteLog("GenericCacheFindDB", "GenericCacheFindDB(static)",e.Message);
                //throw e;
            }
        }

        public static string GetFindSqlStr()
        {
            return sqlFindStr;
        }
    }


    /// <summary>
    /// 添加数据字符串的泛型缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheInsertDB<T> where T : BaseModel
    {
        /// <summary>
        /// 构造函数私有化
        /// </summary>
        private GenericCacheInsertDB()
        {
        }
        private static string _sqlString = null;
        /// <summary>
        /// 静态构造函数执行一次
        /// </summary>
        static GenericCacheInsertDB()
        {
            Type type = typeof(T);

            string proesStr = string.Join(",", GetDBName<T>.GetProNames().
                Where(item => item.ToLower() != "id").Select(item => "[" + item + "]"));

            string proesValStr = string.Join(",", GetDBName<T>.GetProNames().
                Where(item => item.ToLower() != "id").Select(item => "@" + item));

            string sqlStr = "Insert into " + "[" + GetDBName<T>.GetTableName() + "](" + proesStr + ")values" + "(" + proesValStr + ")";
            _sqlString = sqlStr;

        }
        /// <summary>
        /// 获取类型T对应的插入字符串
        /// </summary>
        /// <returns></returns>
        public static string GetInsertSql()
        {
            return _sqlString;
        }
    }


    /// <summary>
    /// 获取修改的字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheUpdateDB<T> where T : BaseModel
    {
        private GenericCacheUpdateDB()
        {
        }

        private static string sqlUpdateStr = null;
        static GenericCacheUpdateDB()
        {
            Type type = typeof(T);
            string proesStr = string.Join(",", GetDBName<T>.GetProNames().
                Where(item => item.ToLower() != "id").Select(item => "[" + item + "]=" + "@" + item));
            string sqlStr = "update [" + GetDBName<T>.GetTableName() + "] set " + proesStr + " where Id=@Id";
            sqlUpdateStr = sqlStr;
        }


        public static string GetUpdStr()
        {
            return sqlUpdateStr;
        }

    }

    /// <summary>
    /// 删除实体字符串泛型缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCacheDelDB<T> where T : BaseModel
    {
        private GenericCacheDelDB()
        {
        }
        private static string sqlDelStr = null;
        static GenericCacheDelDB()
        {
            Type type = typeof(T);
            string sqlStr = "delete " + "[" + GetDBName<T>.GetTableName() + "]" + " where id=@Id";
            sqlDelStr = sqlStr;
        }

        public static string GetDelSql()
        {
            return sqlDelStr;
        }
    }


    #endregion 


}
