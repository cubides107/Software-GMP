using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent.EventRestorePasswordUser;
using JKang.EventBus;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersRootServices.CommandRestorePasswordUser
{
    public class RestorePasswordUserHandler : IRequestHandler<RestorePasswordUserCommand, RestorePasswordUserDTO>
    {
        private readonly IUsersRepositories usersRepositories;
        private readonly IUserSecurity userSecurity;
        private readonly IUsersFactories userFactory;
        private readonly IUtilities utilities;
        private readonly IAutoMapping mapping;
        private readonly IEventPublisher eventPublisher;

        public RestorePasswordUserHandler(IUsersRepositories usersRepositories, IUserSecurity userSecurity,
            IUsersFactories usersFactories, IUtilities utilities, IAutoMapping mapping, IEventPublisher eventPublisher)
        {
            this.usersRepositories = usersRepositories;
            this.userSecurity = userSecurity;
            this.userFactory = usersFactories;
            this.utilities = utilities;
            this.mapping = mapping;
            this.eventPublisher = eventPublisher;
        }

        public async Task<RestorePasswordUserDTO> Handle(RestorePasswordUserCommand request, CancellationToken cancellationToken)
        {
            string mailUserActual;
            string encriptedPassword;
            User userActual;
            User userToRestorePassword;
            User userUpdated;
            Guid idUser;

            if (request == null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            //verificar si se encuentra registrado y si es usuario root
            mailUserActual = this.userSecurity.GetClaim(request.RootClaims, IUserSecurity.MAIL);

            if (this.usersRepositories.Exists<User>(x => x.Mail == mailUserActual) == false)
                throw new UserNotRegisterException(UserNotRegisterException.MESSAGE_USER_IS_NOT_REGISTER);

            userActual = await usersRepositories.Get<User>(x => x.Mail == mailUserActual, cancellationToken);

            if (userActual is null)
                throw new EntityNotRecoveredException(EntityNotRecoveredException.MESSAGE_USER_NOT_RECOVERED);
            else if (userActual.TipoUsuario != TipoUsuarioEnum.root)
                throw new NotRootException(NotRootException.MESSAGE_MAIL);

            //Verificar que el usuario a restablecer la contraseña se encuentre registrado
            if (usersRepositories.Exists<User>(x => x.Mail == request.MailUserRestorePassword) == false)
                throw new UserNotRegisterException(UserNotRegisterException.MESSAGE_USER_IS_NOT_REGISTER);

            //Obtener el usuario al cual se le va a restablecer la contraseña y verificar que no sea nulo
            userToRestorePassword = await usersRepositories.Get<User>(x => x.Mail == request.MailUserRestorePassword, cancellationToken);

            //Verificar si el usuario no es nulo y si el usuario no es usuario root
            if (userToRestorePassword == null)
                throw new EntityNotRecoveredException(EntityNotRecoveredException.MESSAGE_USER_NOT_RECOVERED);
            else if (userToRestorePassword.TipoUsuario == TipoUsuarioEnum.root)
                throw new TypeUserRootException(TypeUserRootException.MESSAGE_TYPE_USER_ROOT_RESTORE_PASSWORD);

            //Encriptar contraseña y convertir id a tipo Guid
            encriptedPassword = userSecurity.EncryptPassword(request.NewPassword);
            idUser = utilities.CreateId(userToRestorePassword.Id);

            //Crear usuario con la contraseña encriptada y el token nulo 
            userUpdated = (User)userFactory.BuildUser(userToRestorePassword.Name, userToRestorePassword.Lastname, userToRestorePassword.Phone, userToRestorePassword.Mail, 
                encriptedPassword, userToRestorePassword.Address, userToRestorePassword.TipoUsuario, id:idUser);

            //Actualizar usuario 
            userUpdated = await usersRepositories.Update<User>(userUpdated, cancellationToken);

            if(userUpdated is null)
                throw new SaveNullException(SaveNullException.MESSAGE);

            //Publicar el evento
            await eventPublisher.PublishEventAsync(mapping.Map<User, RestorePasswordUserEvent>(userUpdated));

            return new RestorePasswordUserDTO
            {};
        }

       
    }
}
