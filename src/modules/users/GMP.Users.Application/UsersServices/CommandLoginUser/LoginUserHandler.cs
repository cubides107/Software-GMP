
using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Domain.Exceptions;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent;
using JKang.EventBus;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.CommandLoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserDTO>
    {
        private readonly IUsersRepositories usersRepositories;
        private readonly IUserSecurity userSecurity;
        private readonly IUsersFactories userFactory;
        private readonly IUtilities utilities;
        private readonly IAutoMapping autoMapping;

        private readonly IEventPublisher eventPublisher;

        public LoginUserHandler(IUsersRepositories usersRepositories, IUserSecurity userSecurity, IUsersFactories usersFactories, 
            IUtilities utilities, IEventPublisher eventPublisher, IAutoMapping autoMapping)
        {
            this.usersRepositories = usersRepositories;
            this.userSecurity = userSecurity;
            this.userFactory = usersFactories;
            this.utilities = utilities;
            this.eventPublisher = eventPublisher;
            this.autoMapping = autoMapping;
        }

        public async Task<LoginUserDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            User user;
            string newToken;
            Guid id;

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);
            else if (usersRepositories.Exists<User>(x => x.Mail == request.Mail) is false)
                throw new UserNotRegisterException(UserNotRegisterException.MESSAGE_USER_IS_NOT_REGISTER);

            user = await usersRepositories.Get<User>(x => x.Mail == request.Mail, cancellationToken);

            if (user is null)
                throw new EntityNotRecoveredException(EntityNotRecoveredException.MESSAGE_USER_NOT_RECOVERED);
            else if (ComparePassword(request.Password, user.Password) is false)
                throw new IncorrectPasswordException(IncorrectPasswordException.MESSAGE_INCORRECT_PASSWORD);


            id = utilities.CreateId(user.Id);

            newToken = userSecurity.CreateToken(id, user.Name, user.Mail, user.TipoUsuario.ToString());

            //Crea Usuario con nuevo token 
            user = (User)userFactory.BuildUser(user.Name, user.Lastname, user.Phone, user.Mail, user.Password, user.Address, user.TipoUsuario, newToken, id);

            //Modificar  Usuario
            user = await usersRepositories.Update(user, cancellationToken);

            if (user is null)
                throw new UpdateNullEntityException(UpdateNullEntityException.MESSAGE_ENTITY_IS_NULL);

            //Publicar el Evento
            await eventPublisher.PublishEventAsync(autoMapping.Map<User, LoginUserEvent>(user));

            //Crea y retorna objeto de transferencia de datos
            return new LoginUserDTO
            {
                Token = newToken
            };
        }

        /// <summary>
        /// compara la contraseña del request con la contraseña del usuario registrado
        /// </summary>
        /// <param name="passwordRequest"></param>
        /// <param name="encryptPassword"></param>
        /// <returns></returns>
        private bool ComparePassword(string passwordRequest, string encryptPassword)
        {
            var passwordRequestEncript = this.userSecurity.EncryptPassword(passwordRequest);
            if (passwordRequestEncript.Equals(encryptPassword))
                return true;
            else
                return false;
        }
    }
}
