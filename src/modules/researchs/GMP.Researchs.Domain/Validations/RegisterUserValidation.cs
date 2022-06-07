using GMP.Domain.Validations;
using GMP.Researchs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Validations
{
    public class RegisterUserValidation
    {
        internal static readonly Func<User, string>[] validations =
        {
            (x) =>
            {
                return (GlobalValidations<User>.IsNotNullAndEmpty(x.Id)) ?
                String.Empty : String.Format("Id");
            },
            (x) =>
            {
                return (GlobalValidations<User>.IsNotNullAndEmpty(x.Mail)) && x.Mail.Contains("@") ?
                String.Empty : String.Format("Mail");
            }
        };
    }
}
