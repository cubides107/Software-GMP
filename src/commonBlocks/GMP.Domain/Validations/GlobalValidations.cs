using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Domain.Validations
{
    public class GlobalValidations<T>
    {
        public static readonly Predicate<T> IsNotNull =
            (x) => x != null;

        public static readonly Predicate<string> IsNotEmpty =
            (x) => x != "";

        public static readonly Predicate<string> IsNotNullAndEmpty =
            (x) => x != "" && x != null;
    }
}
