using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Ports;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.QueryUsers
{
    public class UsersByNameHandler : IRequestHandler<UsersByNameQuery, UsersByNameDTO>
    {
        private readonly IUsersRepositories usersRepositories;
        private readonly IAutoMapping mapping;
        private readonly IUserSecurity userSecurity;


        public UsersByNameHandler(IUsersRepositories usersRepositories, IAutoMapping mapping, IUserSecurity userSecurity)
        {
            this.usersRepositories = usersRepositories;
            this.mapping = mapping;
            this.userSecurity = userSecurity;
        }

        public async Task<UsersByNameDTO> Handle(UsersByNameQuery request, CancellationToken cancellationToken)
        {
            string mailUser;
            User user;
            UsersByNameDTO usersByNameDTO = new();
            List<User> listUsers;

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            //Verificar si esta registrado y si es usuario interno o root
            mailUser = userSecurity.GetClaim(request.ClaimsUser, IUserSecurity.MAIL);

            if (usersRepositories.Exists<User>(x => x.Mail == mailUser) == false)
                throw new UserNotRegisterException(UserNotRegisterException.MESSAGE_USER_IS_NOT_REGISTER);

            user = await usersRepositories.Get<User>(x => x.Mail == mailUser, cancellationToken);

            if (user is null)
                throw new EntityNotRecoveredException(EntityNotRecoveredException.MESSAGE_USER_NOT_RECOVERED);

            if (user.TipoUsuario == TipoUsuarioEnum.externo)
                throw new PermissionQueryUserException(PermissionQueryUserException.PERMISSION_REFUSE_MESSAGE);

            //1. Realizar Query para consultar la lista de usuarios
            listUsers = await usersRepositories.GetAll<User>(x => x.Name, request.Page, request.PageSize, x => x.Name == request.FilterName, cancellationToken);
            if (listUsers is null)
                throw new ListUsersException(ListUsersException.MESSAGE_LIST_NULL);

            //Se mapea la lista y se retorna el objeto de trasferencia de datos
            usersByNameDTO.ListUsers =  mapping.Map<List<User>, List<UsersByNameDTO.UsersDTO>>(listUsers);

            //2. realizar Query para consultar el numero que representa la cantidad de registros
            usersByNameDTO.CountUsers = await this.usersRepositories.Count<User>(x => x.Name == request.FilterName, cancellationToken);

            return usersByNameDTO;
        }
    }
}
