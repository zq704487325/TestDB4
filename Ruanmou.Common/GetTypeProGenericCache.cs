using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Ruanmou.Common
{

    /// <summary>
    /// 获取类型的属性信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GetTypeProGenericCache<T>
    {
        private GetTypeProGenericCache()
        {

        }
        private static PropertyInfo[] proInfos;

        static GetTypeProGenericCache()
        {
            Type type = typeof(T);
            proInfos = type.GetProperties();
        }


        public static PropertyInfo[] GetTypeProInfos()
        {
            return proInfos;
        }
    }
}
