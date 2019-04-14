using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Ruanmou.Common
{
    public class DefinedExpressionVisitor:ExpressionVisitor
    {


        private Stack<string> stackSqlSplit = new Stack<string>();
        public string GetSelectSqlStr<T>(Expression<Func<T, bool>> exp)
        {
            base.Visit(exp);
            stackSqlSplit.Push("select * from [" + typeof(T).Name + "] where");
            return string.Join(" ", stackSqlSplit);
        }


        public string GetDelSqlStr<T>(Expression<Func<T, bool>> exp)
        {
            base.Visit(exp);
            stackSqlSplit.Push("delete from [" + typeof(T).Name + "] where");
            return string.Join(" ", stackSqlSplit);
        }


        public string GetSqlStrRequire<T>(Expression<Func<T, bool>> exp)
        {
            base.Visit(exp);
            return string.Join(" ", stackSqlSplit);
        }



        //public override Expression Visit(Expression node)
        //{
        //    return base.Visit(node);
        //}

        protected override Expression VisitBinary(BinaryExpression node)
        {

            this.Visit(node.Right);
            stackSqlSplit.Push(NodeTypeOperation.GetNodeTypeOpeStr(node.NodeType));
            this.Visit(node.Left);
            return node;

        }


        protected override Expression VisitConstant(ConstantExpression node)
        {
            stackSqlSplit.Push("'"+node.Value+"'");
            return node;
        }


        protected override Expression VisitParameter(ParameterExpression node)
        {
         
            return node;
        }



        protected override Expression VisitMember(MemberExpression node)
        {
            stackSqlSplit.Push("[" + node.Member.Name + "]");
            base.Visit(node.Expression);


            if (node.Member.Name == "Length")
            {
                string memName = this.stackSqlSplit.Pop();
                this.stackSqlSplit.Pop();
                this.stackSqlSplit.Push("LEG(" + memName + ")");
            }

             return node;
        }

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            return node;
        }


        protected override Expression VisitMethodCall(MethodCallExpression node)
        {

            string format = "";
            switch (node.Method.Name)
            {
                case "Equals":
                    format = "{0} = {1}";
                    break;
                case "Substring":
                        format = "substring({0},{1})";
                    break;
                default:
                    throw new Exception("存在不能识别的方法");
            }
            this.Visit(node.Object);
            this.Visit(node.Arguments);
          
            string argsStr = "";
            if (node.Method.Name == "Equals")
            {
                argsStr= stackSqlSplit.Pop();
            }

            if (node.Method.Name == "Substring")
            {
                int num = node.Arguments.Count();
                List<string> args = new List<string>();
                for (int i = 0; i < num; i++)
                {
                    string arg = stackSqlSplit.Pop();
                    args.Add(arg);
                }
                
                args.Reverse();
                if (num == 1)
                {
                    args[0]=$"convert(int,{args[0]})+1";
                    args.Add($"8000-convert(int,{args[0]})-1");
                }
                if (num == 2)
                {
                    args[0] = $"convert(int,{args[0]})+1";
                    args[1] = $"convert(int,{args[1]})";
                }
                argsStr = string.Join(",", args);
            }

            string obj = stackSqlSplit.Pop();
            string result = string.Format(format, obj, argsStr);
            stackSqlSplit.Push(result);

            return node;
        }








    }
}
