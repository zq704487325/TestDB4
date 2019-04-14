using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruanmou.DB.Interface;
using System.Configuration;
using System.Reflection;

namespace Ruanmou.DB.SimpleFactory
{
    public class DBSimpleFactory
    {

        #region 作业7
         


        /// <summary>
        /// 利用反射工厂创建对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private IDBHelper ReflectCreateInstance(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            Assembly ass = Assembly.Load(value.Split(',')[0]);
            Type type = ass.GetType(value.Split(',')[1]);
            return (IDBHelper)Activator.CreateInstance(type);
        }

        /// <summary>
        /// 简单工厂创建对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDBHelper CreateInstanceFactory(string key)
        {
            switch (key.Trim().ToLower())
            {
                case "sqlserver":
                    return ReflectCreateInstance("sqlserver");
                default:
                    throw new Exception(key+"未配置");
            }
        }

        #endregion


    }
}
