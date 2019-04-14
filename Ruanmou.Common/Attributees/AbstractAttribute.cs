using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Common.Attributees
{
    public abstract class AbstractAttribute:Attribute
    {
       public abstract bool Validate(object value);
        public abstract string GetErrorInfo(string proName);

    }
}
