using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using Ruanmou.Model;
using System.Data.SqlClient;
using Ruanmou.DB.Interface;
using Ruanmou.Common;
using Ruanmou.Common.Attributees;
using System.Linq.Expressions;
 

namespace Ruanmou.DB.Sqlserver
{
    public class SqlServerHelper : IDBHelper
    {
        private static string ConnStrNetVipClsTweDB =
            ConfigurationManager.ConnectionStrings["NetVipClsTweDB"].ConnectionString;

        #region 作业3

        /// <summary>
        /// 按照id查询一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体的id值</param>
        /// <returns></returns>
        public T Find<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            string proesStr = string.Join(",", type.GetProperties().Select(item => "[" + item.Name + "]"));
            string strSql = "Select" + proesStr + "from [" + type.Name + "]where [id]=" + id;
            object o = Activator.CreateInstance(type);
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(strSql, conn);
                conn.Open();
                try
                {
                    SqlDataReader reader = comm.ExecuteReader();


                    if (!reader.Read())
                    {
                        return null;
                    }

                    while (reader.Read())
                    {
                        foreach (var pro in type.GetProperties())
                        {
                            pro.SetValue(o, reader[pro.Name] is DBNull ? null : reader[pro.Name]);
                        }
                    }

                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }


            }
            return (T)o;

            //return default(T);
        }


        /// <summary>
        /// 全部查询
        /// </summary>
        /// <typeparam name="T">查询的实体类型</typeparam>
        /// <returns></returns>
        public List<T> SelectAll<T>() where T : BaseModel
        {
            Type type = typeof(T);
            string proesStr = string.Join(",", type.GetProperties().Select(item => "[" + item.Name + "]"));
            string sqlStr = " select " + proesStr + " from " + "[" + type.Name + "]";

            List<T> Ts = new List<T>();
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                conn.Open();
                try
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        object o = Activator.CreateInstance(type);
                        foreach (var pro in type.GetProperties())
                        {
                            pro.SetValue(o, reader[pro.Name] is DBNull ? null : reader[pro.Name]);
                        }
                        T t = (T)o;

                        Ts.Add(t);
                    }
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }

            }

            return Ts;

        }


        #endregion

        #region 作业4
        /// <summary>
        /// 输出任意实体的属性名和属性值
        /// </summary>
        public void ShowProNameAndVal<T>(T t)
        {
            Type type = t.GetType();
            foreach (var pro in type.GetProperties())
            {
                Console.WriteLine(pro.Name + "=" + pro.GetValue(t));
            }
        }

        #endregion

        #region 作业5

        /// <summary>
        /// 给数据插入一行数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">数据实体</param>
        /// <returns></returns>
        public bool Insert<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);

            string proesStr = string.Join(",", type.GetProperties().
                Where(item => item.Name.ToLower() != "id").Select(item => "[" + item.Name + "]"));

            string proesValStr = string.Join(",", type.GetProperties().
                Where(item => item.Name.ToLower() != "id").Select(item => "@" + item.Name));

            string sqlStr = "Insert into " + "[" + type.Name + "] (" + proesStr + ")values" + "(" + proesValStr + ")";

            int rows = 0;

            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                foreach (var pro in type.GetProperties().Where(item => item.Name.ToLower() != "id"))
                {
                    SqlParameter para = new SqlParameter("@" + pro.Name, pro.GetValue(t));
                    comm.Parameters.Add(para);
                }
                conn.Open();
                try
                {
                    rows = comm.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }

            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }




        /// <summary>
        /// 更新数据库中的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Update<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);
            string proesStr = string.Join(",", type.GetProperties().
                Where(item => item.Name.ToLower() != "id").Select(item => "[" + item.Name + "]=" + "@" + item.Name));
            string sqlStr = "update [" + type.Name + "] set " + proesStr + " where id=" + type.GetProperty("Id").GetValue(t);

            int rows = 0;
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                foreach (var pro in type.GetProperties())
                {
                    SqlParameter para = new SqlParameter("@" + pro.Name, pro.GetValue(t));
                    comm.Parameters.Add(para);
                }
                conn.Open();
                try
                {
                    rows = comm.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }
            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 删除数据库中的一条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="id">数据id</param>
        /// <returns></returns>
        public bool Delete<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            string sqlStr = "delete " + "[" + type.Name + "]" + " where id=" + id;
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                conn.Open();
                try
                {
                    rows = comm.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }

            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        #endregion

        #region 作业8
        /// <summary>
        /// 利用泛型缓存插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool InsertGenericCache<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);

            //string proesStr = string.Join(",", type.GetProperties().
            //    Where(item => item.Name.ToLower() != "id").Select(item => "[" + item.Name + "]"));

            //string proesValStr = string.Join(",", type.GetProperties().
            //    Where(item => item.Name.ToLower() != "id").Select(item => "@" + item.Name));

            //string sqlStr = "Insert into " + "[" + type.Name + "]" + "values" + "(" + proesValStr + ")";


            string sqlStr = GenericCacheInsert<T>.GetInsertSql();

            int rows = 0;

            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                foreach (var pro in type.GetProperties().Where(item => item.Name.ToLower() != "id"))
                {
                    SqlParameter para = new SqlParameter("@" + pro.Name, pro.GetValue(t));
                    comm.Parameters.Add(para);
                }
                conn.Open();
                try
                {
                    rows = comm.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }

            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 更新数据库中的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool UpdateGenericCache<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);
            //string proesStr = string.Join(",", type.GetProperties().
            //    Where(item => item.Name.ToLower() != "id").Select(item => "[" + item.Name + "]=" + "@" + item.Name));
            //string sqlStr = "update [" + type.Name + "] set " + proesStr + " where id=@Id";


            string sqlStr = GenericCacheUpdate<T>.GetUpdStr();
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                foreach (var pro in type.GetProperties())
                {
                    SqlParameter para = new SqlParameter("@" + pro.Name, pro.GetValue(t));
                    comm.Parameters.Add(para);
                }
                conn.Open();
                try
                {
                    rows = comm.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }
            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 删除数据库中的一条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="id">数据id</param>
        /// <returns></returns>
        public bool DeleteGenericCache<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            //string sqlStr = "delete " + "[" + type.Name + "]" + " where id=@Id";

            string sqlStr = GenericCacheDel<Company>.GetDelSql();
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                SqlParameter para = new SqlParameter("@Id", id);
                comm.Parameters.Add(para);
                conn.Open();
                try
                {
                    rows = comm.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }

            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 按照id查询一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体的id值</param>
        /// <returns></returns>
        public T FindGenericCache<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            //string proesStr = string.Join(",", type.GetProperties().Select(item => "[" + item.Name + "]"));
            //string strSql = "Select" + proesStr + "from [" + type.Name + "]where [id]=@Id" ;
            string strSql = GenericCacheFind<T>.GetFindSqlStr();
            object o = Activator.CreateInstance(type);
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(strSql, conn);
                SqlParameter para = new SqlParameter("@Id", id);
                comm.Parameters.Add(para);
                conn.Open();
                try
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        foreach (var pro in type.GetProperties())
                        {
                            pro.SetValue(o, reader[pro.Name]);
                        }
                    }
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }


            }
            return (T)o;

            //return default(T);
        }


        /// <summary>
        /// 全部查询
        /// </summary>
        /// <typeparam name="T">查询的实体类型</typeparam>
        /// <returns></returns>
        public List<T> SelectAllGenericCache<T>() where T : BaseModel
        {
            Type type = typeof(T);
            //string proesStr = string.Join(",", type.GetProperties().Select(item => "[" + item.Name + "]"));
            //string sqlStr = " select " + proesStr + " from " + "[" + type.Name + "]";
            string sqlStr = GenericCacheSelectAll<T>.GetSelectAllSqlStr();
            List<T> Ts = new List<T>();
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                conn.Open();
                try
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        object o = Activator.CreateInstance(type);
                        foreach (var pro in type.GetProperties())
                        {
                            pro.SetValue(o, reader[pro.Name]);
                        }
                        T t = (T)o;

                        Ts.Add(t);
                    }
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }

            }

            return Ts;

        }

        #endregion


        #region 第二次作业 作业2





        /// <summary>
        /// 全部查询
        /// </summary>
        /// <typeparam name="T">查询的实体类型</typeparam>
        /// <returns></returns>
        public List<T> SelectAllGenericCacheDB<T>() where T : BaseModel
        {
            Type type = typeof(T);
            //string proesStr = string.Join(",", type.GetProperties().Select(item => "[" + item.Name + "]"));
            //string sqlStr = " select " + proesStr + " from " + "[" + type.Name + "]";
            string sqlStr = GenericCacheSelectAllDB<T>.GetSelectAllSqlStr();
            List<T> Ts = new List<T>();
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                conn.Open();
                try
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        object o = Activator.CreateInstance(type);
                        foreach (var pro in GetTypeProGenericCache<T>.GetTypeProInfos())
                        {
                            if (pro.IsDefined(typeof(DBNameAttribute), true))
                            {
                                DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                                pro.SetValue(o, reader[dBNameAttribute.Name]);
                            }
                            else
                            {
                                pro.SetValue(o, reader[pro.Name]);
                            }
                        }
                        T t = (T)o;

                        Ts.Add(t);
                    }
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }

            }

            return Ts;

        }


        /// <summary>
        /// 按照id查询一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体的id值</param>
        /// <returns></returns>
        public T FindGenericCacheDB<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            //string proesStr = string.Join(",", type.GetProperties().Select(item => "[" + item.Name + "]"));
            //string strSql = "Select" + proesStr + "from [" + type.Name + "]where [id]=@Id" ;
            string strSql = GenericCacheFindDB<T>.GetFindSqlStr();
            object o = Activator.CreateInstance(type);
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(strSql, conn);
                SqlParameter para = new SqlParameter("@Id", id);
                comm.Parameters.Add(para);
                conn.Open();
                try
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        foreach (var pro in GetTypeProGenericCache<T>.GetTypeProInfos())
                        {
                            if (pro.IsDefined(typeof(DBNameAttribute), true))
                            {
                                DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                                pro.SetValue(o, reader[dBNameAttribute.Name]);
                            }
                            else
                            {
                                pro.SetValue(o, reader[pro.Name]);
                            }
                        }
                        return (T)o;

                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }


            }




            //return default(T);
        }


        /// <summary>
        /// 利用泛型缓存插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool InsertGenericCacheDB<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);

            //string proesStr = string.Join(",", type.GetProperties().
            //    Where(item => item.Name.ToLower() != "id").Select(item => "[" + item.Name + "]"));

            //string proesValStr = string.Join(",", type.GetProperties().
            //    Where(item => item.Name.ToLower() != "id").Select(item => "@" + item.Name));

            //string sqlStr = "Insert into " + "[" + type.Name + "]" + "values" + "(" + proesValStr + ")";


            string sqlStr = GenericCacheInsertDB<T>.GetInsertSql();

            int rows = 0;

            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                var proes = GetTypeProGenericCache<T>.GetTypeProInfos().Where(item => item.Name.ToLower() != "id");
                foreach (var pro in proes)
                {
                    SqlParameter para = new SqlParameter();
                    if (pro.IsDefined(typeof(DBNameAttribute), true))
                    {
                        DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                        para = new SqlParameter("@" + dBNameAttribute.Name, pro.GetValue(t));
                    }
                    else
                    {
                        para = new SqlParameter("@" + pro.Name, pro.GetValue(t));
                    }

                    comm.Parameters.Add(para);
                }
                conn.Open();
                try
                {
                    rows = comm.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }

            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 更新数据库中的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool UpdateGenericCacheDB<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);
            //string proesStr = string.Join(",", type.GetProperties().
            //    Where(item => item.Name.ToLower() != "id").Select(item => "[" + item.Name + "]=" + "@" + item.Name));
            //string sqlStr = "update [" + type.Name + "] set " + proesStr + " where id=@Id";


            string sqlStr = GenericCacheUpdateDB<T>.GetUpdStr();
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                foreach (var pro in GetTypeProGenericCache<T>.GetTypeProInfos())
                {
                    SqlParameter para = new SqlParameter();
                    if (pro.IsDefined(typeof(DBNameAttribute), true))
                    {
                        DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                        para = new SqlParameter("@" + dBNameAttribute.Name, pro.GetValue(t));
                    }
                    else
                    {
                        para = new SqlParameter("@" + pro.Name, pro.GetValue(t));
                    }

                    comm.Parameters.Add(para);
                }
                conn.Open();
                try
                {
                    rows = comm.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }
            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 删除数据库中的一条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="id">数据id</param>
        /// <returns></returns>
        public bool DeleteGenericCacheDB<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            //string sqlStr = "delete " + "[" + type.Name + "]" + " where id=@Id";

            string sqlStr = GenericCacheDel<Company>.GetDelSql();
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                SqlParameter para = new SqlParameter("@Id", id);
                comm.Parameters.Add(para);
                conn.Open();
                try
                {
                    rows = comm.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }

            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion



        #region 第二次作业 作业4  第四次作业 作业1



        /// <summary>
        /// 全部查询
        /// </summary>
        /// <typeparam name="T">查询的实体类型</typeparam>
        /// <returns></returns>
        public List<T> SelectAllGenericCacheDBDelegate<T>() where T : BaseModel
        {
            Type type = typeof(T);
            string sqlStr = GenericCacheSelectAllDB<T>.GetSelectAllSqlStr();
            List<T> Ts = new List<T>();
            Func<SqlCommand, List<T>> func = comm =>
             {

                 SqlDataReader reader = comm.ExecuteReader();
                 //SqlDataReader reader = ExexuteSql(sqlStr, SeletFunc);
                 while (reader.Read())
                 {
                     object o = Activator.CreateInstance(type);
                     foreach (var pro in GetTypeProGenericCache<T>.GetTypeProInfos())//缓存
                     {
                         if (pro.IsDefined(typeof(DBNameAttribute), true))
                         {
                             DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                             pro.SetValue(o, reader[dBNameAttribute.Name]);
                         }
                         else
                         {
                             pro.SetValue(o, reader[pro.Name]);
                         }
                     }
                     T t = (T)o;

                     Ts.Add(t);
                 }
                 return Ts;

             };

            return this.ExexuteSql(sqlStr, func);
            //return Ts;

        }


        /// <summary>
        /// 按照id查询一个实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体的id值</param>
        /// <returns></returns>
        public T FindGenericCacheDBDelegate<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            //string proesStr = string.Join(",", type.GetProperties().Select(item => "[" + item.Name + "]"));
            //string strSql = "Select" + proesStr + "from [" + type.Name + "]where [id]=@Id" ;
            string strSql = GenericCacheFindDB<T>.GetFindSqlStr();
            object o = Activator.CreateInstance(type);
            SqlParameter para = new SqlParameter("@Id", id);

            Func<SqlCommand, T> func = comm =>
            {
                comm.Parameters.Add(para);
                SqlDataReader reader = comm.ExecuteReader();
                //SqlDataReader reader = ExexuteSql(strSql, SeletFunc);  
                if (reader.Read())
                {
                    foreach (var pro in GetTypeProGenericCache<T>.GetTypeProInfos())
                    {
                        if (pro.IsDefined(typeof(DBNameAttribute), true))
                        {
                            DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                            pro.SetValue(o, reader[dBNameAttribute.Name]);
                        }
                        else
                        {
                            pro.SetValue(o, reader[pro.Name]);
                        }
                    }
                    return (T)o;

                }
                else
                {
                    return default(T);
                }
                //return default(T);
            };

            return this.ExexuteSql(strSql, func);

        }





        /// <summary>
        /// 利用泛型缓存插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool InsertGenericCacheDBDelegate<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);
            string sqlStr = GenericCacheInsertDB<T>.GetInsertSql();

            int rows = 0;


            Func<SqlCommand, int> func = comm =>
            {
                var proes = GetTypeProGenericCache<T>.GetTypeProInfos().Where(item => item.Name.ToLower() != "id");
                foreach (var pro in proes)
                {
                    SqlParameter para = new SqlParameter();
                    if (pro.IsDefined(typeof(DBNameAttribute), true))
                    {
                        DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                        para = new SqlParameter("@" + dBNameAttribute.Name, pro.GetValue(t));
                    }
                    else
                    {
                        para = new SqlParameter("@" + pro.Name, pro.GetValue(t));
                    }
                    comm.Parameters.Add(para);

                }
                return comm.ExecuteNonQuery();

            };

            rows = this.ExexuteSql(sqlStr, func);

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        /// <summary>
        /// 更新数据库中的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool UpdateGenericCacheDBDelegate<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);

            string sqlStr = GenericCacheUpdateDB<T>.GetUpdStr();
            int rows = 0;

            Func<SqlCommand, int> func = comm =>
            {
                foreach (var pro in GetTypeProGenericCache<T>.GetTypeProInfos())
                {
                    SqlParameter para = new SqlParameter();
                    if (pro.IsDefined(typeof(DBNameAttribute), true))
                    {
                        DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                        para = new SqlParameter("@" + dBNameAttribute.Name, pro.GetValue(t));
                    }
                    else
                    {
                        para = new SqlParameter("@" + pro.Name, pro.GetValue(t));
                    }

                    comm.Parameters.Add(para);
                }
                return comm.ExecuteNonQuery();

            };

            rows = this.ExexuteSql(sqlStr, func);

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 删除数据库中的一条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="id">数据id</param>
        /// <returns></returns>
        public bool DeleteGenericCacheDBDelegate<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            //string sqlStr = "delete " + "[" + type.Name + "]" + " where id=@Id";

            string sqlStr = GenericCacheDel<T>.GetDelSql();
            int rows = 0;
            //using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            //{
            //    SqlCommand comm = new SqlCommand(sqlStr, conn);
            //    SqlParameter para = new SqlParameter("@Id", id);
            //    comm.Parameters.Add(para);
            //    conn.Open();
            //    try
            //    {
            //        rows = comm.ExecuteNonQuery();
            //    }
            //    catch (Exception e)
            //    {
            //        conn.Close();
            //        throw e;
            //    }

            //}

            Func<SqlCommand, int> func = comm =>
            {
                SqlParameter para = new SqlParameter("@Id", id);
                comm.Parameters.Add(para);
                return comm.ExecuteNonQuery();

            };

            rows = this.ExexuteSql(sqlStr, func);


            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 数据库读取通用封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStr"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public T ExexuteSql<T>(string sqlStr, Func<SqlCommand, T> fun)
        {
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(sqlStr, conn);
                conn.Open();
                try
                {
                    return fun.Invoke(comm);
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }


            }

        }

        #endregion



        #region 第四次作业 作业2
        Expression<Func<SqlDataReader, Company>> func = reader => new Company { Id = (int)reader["Id"], Name = (string)reader["Name"] };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private T ReaderTurnToObject<T>(SqlDataReader reader)
        {
            Type redType = typeof(SqlDataReader);
            Type comType = typeof(T);
            List<MemberBinding> memBindList = new List<MemberBinding>();
            ParameterExpression parameterExpression = Expression.Parameter(typeof(SqlDataReader), "reader");
            foreach (var pro in comType.GetProperties())
            {
                ConstantExpression conEpr1 = Expression.Constant(pro.Name, typeof(string));
                MethodInfo metIfoRder = redType.GetProperty("Item", new Type[] { typeof(string) }).GetGetMethod();
                MethodCallExpression metCalExpr = Expression.Call(parameterExpression, metIfoRder, new Expression[] { conEpr1 });
                UnaryExpression unExpress = Expression.Convert(metCalExpr, pro.PropertyType);

                //MethodInfo comSetMethod =pro.SetMethod;
                MemberAssignment memAss = Expression.Bind(pro, unExpress);

                memBindList.Add(memAss);
            }

            MemberInitExpression memInitExpr = Expression.MemberInit(Expression.New(typeof(T)), memBindList.ToArray());
            Expression<Func<SqlDataReader, T>> func = Expression.Lambda<Func<SqlDataReader, T>>(memInitExpr, new ParameterExpression[] { parameterExpression });
            return func.Compile().Invoke(reader);

        }
        /// <summary>
        /// 通过
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T FindByExp<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            string strSql = GenericCacheFindDB<T>.GetFindSqlStr();
            object o = Activator.CreateInstance(type);
            using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
            {
                SqlCommand comm = new SqlCommand(strSql, conn);
                SqlParameter para = new SqlParameter("@Id", id);
                comm.Parameters.Add(para);
                conn.Open();
                try
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                    {

                        return ReaderToObjectGenericCache<T>.GetReaderToObjectDelegate().Invoke(reader);

                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    conn.Close();
                    throw e;
                }
            }

        }




        #endregion


        #region 第四次作业 作业3
        //DefinedExpressionVisitor vistor = new DefinedExpressionVisitor();
        ///// <summary>
        ///// 表达式目录树查找
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public T FindExprSql<T>(int id) where T : BaseModel
        //{
        //    Type type = typeof(T);
        //    Expression<Func<T, bool>> funcExpr = x => x.Id == id;

        //    string sqlStr = vistor.GetSelectSqlStr<T>(funcExpr);
        //    object o = Activator.CreateInstance(type);
        //    using (SqlConnection conn = new SqlConnection(ConnStrNetVipClsTweDB))
        //    {
        //        SqlCommand comm = new SqlCommand(sqlStr, conn);
        //        conn.Open();
        //        try
        //        {
        //            SqlDataReader reader = comm.ExecuteReader();
        //            //if (!reader.Read())
        //            //{
        //            //    return default(T);
        //            //}

        //            while (reader.Read())
        //            {
        //                foreach (var pro in type.GetProperties())
        //                {
        //                    pro.SetValue(o, reader[pro.Name] is DBNull ? null : reader[pro.Name]);
        //                }

        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            conn.Close();
        //            throw e;
        //        }


        //    }
        //    return (T)o;


        //}


        #endregion

        #region  第四次作业 作业6
        DefinedExpressionVisitor vistor = new DefinedExpressionVisitor();
        /// <summary>
        /// 解析表达式目录树 获取字符串
        /// </summary>
        /// <returns></returns>
        public string GetExprSqlStr()
        {
            //Expression<Func<Company, bool>> expr = x => x.Name.Equals("华康达");
            //Expression<Func<Company, bool>> expr = x => x.Name.Length>0;

            Expression<Func<Company, bool>> expr = x => x.Name.Substring(0, 2).Equals("东风");

            string sqlStr = vistor.GetSelectSqlStr(expr);

            return null;
        }

        /// <summary>
        /// 解析表达式目录树 完成批量修改的功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="exp"></param>
        /// <param name="updateAction"></param>
        public void UpdateSqlStrExpr<T>(T t,Expression<Func<T, bool>> exp, Action<T> updateAction)where T:BaseModel
        {
            string sqlStrRequ = vistor.GetSqlStrRequire<T>(exp);
            updateAction = x =>
            {
                Type type = typeof(T);
                string proesStr = string.Join(",", GetDBName<T>.GetProNames().
                    Where(item => item.ToLower() != "id").Select(item => "[" + item + "]=" + "@" + item));
                string sqlStr = "update [" + GetDBName<T>.GetTableName() + "] set " + proesStr +" where "+ sqlStrRequ;
                Func<SqlCommand, int> func = comm =>
                {
                    foreach (var pro in GetTypeProGenericCache<T>.GetTypeProInfos())
                    {
                        SqlParameter para = new SqlParameter();
                        if (pro.IsDefined(typeof(DBNameAttribute), true))
                        {
                            DBNameAttribute dBNameAttribute = (DBNameAttribute)pro.GetCustomAttribute(typeof(DBNameAttribute));
                            para = new SqlParameter("@" + dBNameAttribute.Name, pro.GetValue(t));
                        }
                        else
                        {
                            para = new SqlParameter("@" + pro.Name, pro.GetValue(t));
                        }

                        comm.Parameters.Add(para);
                    }
                    return comm.ExecuteNonQuery();

                };

                this.ExexuteSql(sqlStr, func);

            };
            updateAction.Invoke(t);
        }




        #endregion

















    }
}
