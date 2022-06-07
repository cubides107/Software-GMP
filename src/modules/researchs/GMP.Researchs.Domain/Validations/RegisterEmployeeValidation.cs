using GMP.Domain.Validations;
using GMP.Researchs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Validations
{
    public class RegisterEmployeeValidation
    {
        internal static readonly Func<Employee, string>[] validations =
        {
            (x) =>
            {
                return (GlobalValidations<Employee>.IsNotNullAndEmpty(x.Id)) ?
                String.Empty : String.Format("Id");
            },
            (x) =>
            {
                return (GlobalValidations<Employee>.IsNotNullAndEmpty(x.Name)) ?
                String.Empty : String.Format("Name");
            },
            (x) =>
            {
                return (GlobalValidations<Employee>.IsNotNullAndEmpty(x.LastName)) ?
                String.Empty : String.Format("LastName");
            },
            (x) =>
            {
                return (GlobalValidations<Employee>.IsNotNullAndEmpty(x.DocumentNumber)) ?
                String.Empty : String.Format("DocumentNumber");
            },
            (x) =>
            {
                return (GlobalValidations<Employee>.IsNotNullAndEmpty(x.Phone)) ?
                String.Empty : String.Format("Phone");
            },
            (x) =>
            {
                return (GlobalValidations<bool>.IsNotNull(x.IsCritical)) ?
                String.Empty : String.Format("IsCritical");
            }
        };
    }
}
