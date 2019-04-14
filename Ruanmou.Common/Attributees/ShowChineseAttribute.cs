using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Common.Attributees
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false,Inherited =false)]
    public class ShowChineseAttribute : Attribute
    {

        public string ProName { get;private set; }
        public ShowChineseAttribute(string name)
        {
            this.ProName = name;
        }
    }
}
