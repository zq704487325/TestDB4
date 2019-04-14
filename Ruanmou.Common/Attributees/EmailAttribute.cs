using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ruanmou.Common.Attributees
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailAttribute:AbstractAttribute
    {

        public override bool Validate(object email)
        {
            string pattern = @"^([/ w -/.] +)@((/[[0 - 9]{ 1,3}/.[0 - 9] 
                           { 1,3}/.[0 - 9]{ 1,3}/.)| (([/ w -] +/.) +))
                           ([a - zA - Z]{ 2,4}|[0 - 9]{ 1,3})(/) ?]$";
            Regex emailRegex = new Regex(pattern);
            return emailRegex.IsMatch(email.ToString());

        }

        public override string GetErrorInfo(string proName)
        {
            return proName+"格式有误";
        }
    }
}
