using GMP.Users.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Security.Claims;

namespace GMP.Users.Application.UsersServices.QueryUser
{
    public  class QueryUser : IRequest<QueryUserDTO>
    {
        public List<Claim> UserClaims { get; set; }
    }
}
