using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Ports;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.QueryUser
{
    public class QueryUserHandler : IRequestHandler<QueryUser, QueryUserDTO>
    {
        private readonly IUserSecurity userSecurity;
        private readonly IUsersRepositories usersRepositories;
        private readonly IAutoMapping mapping;

        public QueryUserHandler(IUserSecurity userSecurity, IUsersRepositories usersRepositories, IAutoMapping mapping)
        {
            this.userSecurity = userSecurity;
            this.usersRepositories = usersRepositories;
            this.mapping = mapping;
        }

        public async Task<QueryUserDTO> Handle(QueryUser request, CancellationToken cancellationToken)
        {
            string userId;
            User user;
            QueryUserDTO queryUser = new();

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            //Verificar si el usuario esta registrado
            userId = userSecurity.GetClaim(request.UserClaims, IUserSecurity.USERID);

            if (usersRepositories.Exists<User>(x => x.Id == userId) == false)
                throw new UserNotRegisterException(UserNotRegisterException.MESSAGE_USER_IS_NOT_REGISTER);

            //Obtener usuario por el ID
            user = await usersRepositories.Get<User>(x => x.Id == userId, cancellationToken);

            //Verificar si el usuario y si el token del mismo estan nulos
            if (user is null)
                throw new EntityNotRecoveredException(EntityNotRecoveredException.MESSAGE_USER_NOT_RECOVERED);
            else if(user.Token is null)
                throw new TokenNullException(TokenNullException.NOT_START_SESSION_EXCEPTION);

            //Mapear el objeto de retorno de datos y retornar objeto
            queryUser = mapping.Map<User,QueryUserDTO>(user);

            return queryUser;
        }
    }
}
