using GMP.Domain.Validations;
using GMP.Researchs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Validations
{
    public class RegisterMapHomeValidation
    {
        internal static readonly Func<MapHome, string>[] validations =
        {
            (x) =>
            {
                return (GlobalValidations<MapHome>.IsNotNullAndEmpty(x.Id)) ?
                String.Empty : String.Format("Id");
            },
            (x) =>
            {
                return (GlobalValidations<MapHome>.IsNotNullAndEmpty(x.ContainerName)) ?
                String.Empty : String.Format("containerName");
            },
            (x) =>
            {
                return (GlobalValidations<MapHome>.IsNotNullAndEmpty(x.Label)) ?
                String.Empty : String.Format("Label");
            },
        };
    }
}
