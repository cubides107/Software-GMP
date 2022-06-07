using GMP.Domain.Validations;
using GMP.Researchs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Validations
{
    public class RegisterAddressValidation
    {
        internal static readonly Func<Address, string>[] validations =
        {
            (x)=>
            {
                return (GlobalValidations<Address>.IsNotNullAndEmpty(x.Id)) ?
                String.Empty : String.Format("Id");
            },
            (x)=>
            {
                return (GlobalValidations<Address>.IsNotNullAndEmpty(x.Neighborhood)) ?
                String.Empty : String.Format("Neighborhood");
            },
            (x)=>
            {
                return (GlobalValidations<Address>.IsNotNullAndEmpty(x.City)) ?
                String.Empty : String.Format("City");
            },
            (x)=>
            {
                return (GlobalValidations<Address>.IsNotNullAndEmpty(x.Country)) ?
                String.Empty : String.Format("Country");
            },
            (x)=>
            {
                return (GlobalValidations<Address>.IsNotNullAndEmpty(x.AddressText)) ?
                String.Empty : String.Format("AddressText");
            }
        };
    }
}
