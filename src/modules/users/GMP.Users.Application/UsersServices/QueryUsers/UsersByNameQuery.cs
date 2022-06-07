using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.QueryUsers
{
    public class UsersByNameQuery : IRequest<UsersByNameDTO>
    {
        public List<Claim> ClaimsUser { get; set; }
        public string FilterName { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
