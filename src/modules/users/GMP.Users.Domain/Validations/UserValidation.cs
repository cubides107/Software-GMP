using GMP.Domain.Validations;
using GMP.Users.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Domain.Validations
{
    public class UserValidation
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
                return (GlobalValidations<User>.IsNotNullAndEmpty(x.Lastname))  && x.Lastname.Length <= 30?
                String.Empty : String.Format("Lastname");
            },
            (x) =>
            {
                return (GlobalValidations<User>.IsNotNullAndEmpty(x.Mail)) && x.Mail.Contains("@")?
                String.Empty : String.Format("Mail");
            },
            (x) =>
            {
                return (GlobalValidations<User>.IsNotNullAndEmpty(x.Name)) && x.Name.Length <= 30?
                String.Empty : String.Format("Name");
            },
            (x) =>
            {
                return (GlobalValidations<User>.IsNotNullAndEmpty(x.Password))?
                String.Empty : String.Format("Password");
            },
            (x) =>
            {
                return (GlobalValidations<User>.IsNotNullAndEmpty(x.Phone))?
                String.Empty : String.Format("Phone");
            },
            (x) =>
            {
                return (GlobalValidations<User>.IsNotEmpty(x.Token))?
                String.Empty : String.Format("Token");
            }
        };
    }
}
