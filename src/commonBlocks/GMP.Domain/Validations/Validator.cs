using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Domain.Validations
{
    public class Validator
    {
        public static List<string> Validate<T>(T obj, params Func<T, string>[] validatons)
        {
            return validatons
                .Select(x => { return x(obj); })
                .Where(x => x != string.Empty)
                .ToList();
        }
    }
}
