using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace Ruanmou.Common
{
    public class ReaderToObjectGenericCache<T>
    {
        private ReaderToObjectGenericCache()
        {

        }

        private static Func<SqlDataReader, T> _func;

        /// <summary>
        /// reader到实体的转换
        /// </summary>
        static ReaderToObjectGenericCache()
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
            _func = func.Compile();
        }

        public static Func<SqlDataReader,T> GetReaderToObjectDelegate()
        {
            return _func;

        }

    }
}
