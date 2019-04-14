using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Common.Attributees
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute:AbstractAttribute
    {
        public override bool Validate(object value)
        {
            return (value != null && !string.IsNullOrWhiteSpace(value.ToString()));
        }
        public override string GetErrorInfo(string proName)
        {
            return proName+ "不允许为空";
        }

    }
}
