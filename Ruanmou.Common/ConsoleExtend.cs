using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Ruanmou.Common.Attributees;


namespace Ruanmou.Common
{
    public  static class ConsoleExtend
    {

        #region 作业4
        /// <summary>
        /// 输出任意实体的属性名和属性值
        /// </summary>
        public static void ShowProNameAndVal<T>(this T t)
        {
            Type type = t.GetType();
            foreach (var pro in type.GetProperties())//第二次作业 作业5
            {
                Console.WriteLine(pro.Name + "=" + pro.GetValue(t));
            }
        }

        #endregion



        #region 第二次作业  作业1
        /// <summary>
        /// 输出任意实体的属性名（如果有则输出中文名）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void ShowChiProNameAndVal<T>(this T t)
        {
            Type type = t.GetType();
            foreach (var pro in type.GetProperties())//第二次作业 作业5
            {
                if (pro.IsDefined(typeof(Attributees.ShowChineseAttribute), true))
                {
                    object o = pro.GetCustomAttribute(typeof(Attributees.ShowChineseAttribute), false);
                    Attributees.ShowChineseAttribute sca = (Attributees.ShowChineseAttribute)o;
                    Console.WriteLine(sca.ProName + "=" + pro.GetValue(t));
                }
                else
                {
                    Console.WriteLine(pro.Name + "=" + pro.GetValue(t));
                }
            }

        }

        #endregion


        #region 第二次作业 作业3
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static bool Validate<T>(this T t,out List<string> infos)
        {
            Type type = t.GetType();
            bool ifBy = true;
            infos = new List<string>();
            foreach (var pro in type.GetProperties())//第二次作业 作业5
            {
                if (pro.IsDefined(typeof(AbstractAttribute), true))
                {
                    object[] objects = pro.GetCustomAttributes(typeof(AbstractAttribute), true);
                    foreach (var o in objects)
                    {
                        AbstractAttribute abAtt = (AbstractAttribute)o;
                        if (!abAtt.Validate(pro.GetValue(t)))
                        {
                            ifBy = false;
                            infos.Add(abAtt.GetErrorInfo(pro.Name));

                        }

                    }

                }
               
            }
            return ifBy;


        }

        #endregion



    }
}
