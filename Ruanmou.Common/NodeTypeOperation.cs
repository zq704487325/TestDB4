using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Ruanmou.Common
{
    public static class NodeTypeOperation
    {
        /// <summary>
        /// 解析关系运算符
        /// </summary>
        /// <param name="nodeType">节点关系</param>
        /// <returns></returns>
        public static string GetNodeTypeOpeStr(ExpressionType nodeType)
        {
            if (nodeType == ExpressionType.Add)
                return " + ";
            else if (nodeType == ExpressionType.AddAssign)
                return "+=";
            else if (nodeType == ExpressionType.GreaterThan)
                return ">";
            else if (nodeType == ExpressionType.GreaterThanOrEqual)
                return ">=";
            else if (nodeType == ExpressionType.LessThan)
                return "<";
            else if (nodeType == ExpressionType.LessThanOrEqual)
                return "<=";
            else if (nodeType == ExpressionType.Equal)
                return "=";
            else if (nodeType == ExpressionType.NotEqual)
                return "!=";
            else
                throw new Exception("尚未解析相应的关系运算符！");
           

        }


    }
}
