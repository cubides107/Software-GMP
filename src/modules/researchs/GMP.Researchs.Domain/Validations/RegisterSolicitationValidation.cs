using GMP.Domain.Validations;
using GMP.Researchs.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Domain.Validations
{
    public class RegisterSolicitationValidation
    {
        internal static readonly Func<Solicitation, string>[] validations =
        {
            (x) =>
            {
                return (GlobalValidations<Solicitation>.IsNotNullAndEmpty(x.Id)) ?
                String.Empty : String.Format("Id");
            },
            (x) =>
            {
                return (GlobalValidations<DateTime>.IsNotNull(x.SolicitationDate)) ?
                String.Empty : String.Format("SolicitationDate");
            },
            (x) =>
            {
                return (GlobalValidations<DateTime>.IsNotNull(x.StartDate)) ?
                String.Empty : String.Format("StartDate");
            },
            (x) =>
            {
                return (GlobalValidations<DateTime>.IsNotNull(x.EndDate)) ?
                String.Empty : String.Format("EndDate");
            },
            (x) =>
            {
                //en el caso de que el id sea vacio o nulo, al menos, el objeto deje existir
                return (GlobalValidations<Solicitation>.IsNotNullAndEmpty(x.UserCreatesSolicitationId)) || (GlobalValidations<User>.IsNotNull(x.UserCreatesSolicitation)) ?
                String.Empty : String.Format("UserId");
            },
            (x) =>
            {
                //en el caso de que el id sea vacio o nulo, al menos, el objeto deje existir
                return (GlobalValidations<Solicitation>.IsNotNullAndEmpty(x.EmployeeId)) || (GlobalValidations<Employee>.IsNotNull(x.Employee)) ?
                String.Empty : String.Format("UserId");
            },
            (x) =>
            {
                //validamos que existe el usuario quien creo la soliitud
                return (GlobalValidations<User>.IsNotNull(x.UserCreatesSolicitation)) ?
                String.Empty : String.Format("UserId");
            }
        };

        internal static readonly Func<Solicitation, string>[] validationsWithUserManages =
        {
            (x) =>
            {
                return (GlobalValidations<Solicitation>.IsNotNullAndEmpty(x.Id)) ?
                String.Empty : String.Format("Id");
            },
            (x) =>
            {
                return (GlobalValidations<DateTime>.IsNotNull(x.SolicitationDate)) ?
                String.Empty : String.Format("SolicitationDate");
            },
            (x) =>
            {
                return (GlobalValidations<DateTime>.IsNotNull(x.StartDate)) ?
                String.Empty : String.Format("StartDate");
            },
            (x) =>
            {
                return (GlobalValidations<DateTime>.IsNotNull(x.EndDate)) ?
                String.Empty : String.Format("EndDate");
            },
            (x) =>
            {
                //en el caso de que el id sea vacio o nulo, al menos, el objeto deje existir
                return (GlobalValidations<Solicitation>.IsNotNullAndEmpty(x.UserCreatesSolicitationId)) || (GlobalValidations<User>.IsNotNull(x.UserCreatesSolicitation)) ?
                String.Empty : String.Format("UserId");
            },
            (x) =>
            {
                //en el caso de que el id sea vacio o nulo, al menos, el objeto deje existir
                return (GlobalValidations<Solicitation>.IsNotNullAndEmpty(x.EmployeeId)) || (GlobalValidations<Employee>.IsNotNull(x.Employee)) ?
                String.Empty : String.Format("UserId");
            },
            (x) =>
            {
                //validamos que existe el usuario quien modificara la solicitud (al que se elasigna)
                return (GlobalValidations<User>.IsNotNull(x.UserManagesSolicitation)) ?
                String.Empty : String.Format("UserId");
            },
            (x) =>
            {
                //validamos que existe el usuario quien creo la soliitud
                return (GlobalValidations<User>.IsNotNull(x.UserCreatesSolicitation)) ?
                String.Empty : String.Format("UserId");
            }
            ,
            (x) =>
            {
                //validamos que existe el usuario quien creo la soliitud
                return x.TipoSolicitudEnums is not null && x.TipoSolicitudEnums.Count > 0 ?
                String.Empty : String.Format("UserId");
            }
        };
    }
}
