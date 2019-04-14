using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Common.Attributees
{
     [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property,AllowMultiple =false,Inherited =false)]
    public class DBNameAttribute:Attribute
    {
        public string Name { get; private set; }
        public DBNameAttribute(string name)
        {
           this. Name = name;
        }


    }
}
