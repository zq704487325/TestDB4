using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ruanmou.Common.Attributees
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MobileAttribute : AbstractAttribute
    {
        public override bool Validate(object mobile)
        {
            string pattern = "^1[0-9]{10}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(mobile.ToString());

        }
        public override string GetErrorInfo(string proName)
        {
            return proName+ "格式有误！";
        }

    }
}
