using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent.EventInternalRegisterExternal;
using JKang.EventBus;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using GMP.Users.IntegrationEvent;

namespace GMP.Users.Application.UsersServices.CommandInternalRegisterExternal
{
    public class InternalRegisterExternalHandler : IRequestHandler<InternalRegisterExternalCommand, InternalRegisterExternalDTO>
    {
        private readonly IUsersRepositories usersRepositories;
        private readonly IUserSecurity userSecurity;
        private readonly IUsersFactories userFactory;
        private readonly IUtilities utilities;
        private readonly IAutoMapping autoMapping;
        private readonly IEventPublisher eventPublisher;

        public InternalRegisterExternalHandler(IUsersRepositories usersRepositories, IUserSecurity userSecurity, 
            IUsersFactories usersFactories, IUtilities utilities, IAutoMapping autoMapping, IEventPublisher eventPublisher)
        {
            this.usersRepositories = usersRepositories;
            this.userSecurity = userSecurity;
            this.userFactory = usersFactories;
            this.utilities = utilities;
            this.autoMapping = autoMapping;
            this.eventPublisher = eventPublisher;
        }

        public async Task<InternalRegisterExternalDTO> Handle(InternalRegisterExternalCommand request, CancellationToken cancellationToken)
        {
            string mailInternal;
            User userInternal;
            User newUserExternal;
            Guid id;

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            //verificar si esta registrado y si es un usuario interno
            mailInternal = this.userSecurity.GetClaim(request.UserInternalClaims, IUserSecurity.MAIL);

            if(this.usersRepositories.Exists<User>(x => x.Mail == mailInternal) == false)
                throw new UserNotRegisterException(UserNotRegisterException.MESSAGE_USER_IS_NOT_REGISTER);

            userInternal = await this.usersRepositories.Get<User>(x => x.Mail == mailInternal, cancellationToken);

            if (userInternal is null)
                throw new EntityNotRecoveredException(EntityNotRecoveredException.MESSAGE_USER_NOT_RECOVERED);

            if (userInternal.TipoUsuario != TipoUsuarioEnum.interno)
                throw new InternalUserException(InternalUserException.MESSAGE);

            //Entonces, verificar que el nuevo usuario no exista y crearlo
            if (this.usersRepositories.Exists<User>(x => x.Mail == request.NewUserExternal.Mail) == true)
                throw new UserExistsException(UserExistsException.MESSAGE);

            id = this.utilities.CreateId();

            //Encriptar Contraseña
            string encriptedPassword = userSecurity.EncryptPassword(request.NewUserExternal.Password);

            newUserExternal = (User)this.userFactory.BuildUser(
                name: request.NewUserExternal.Name,
                lastname: request.NewUserExternal.Lastname,
                phone: request.NewUserExternal.Phone,
                mail: request.NewUserExternal.Mail,
                password: encriptedPassword,
                address: request.NewUserExternal.Address,
                tipoUsuarioEnumGuid: TipoUsuarioEnum.externo,
                id: id);

            newUserExternal = await this.usersRepositories.Save<User>(newUserExternal, cancellationToken);
            if (newUserExternal is null)
                throw new SaveNullException(SaveNullException.MESSAGE);

            //publicar el evento
            await this.eventPublisher.PublishEventAsync(this.autoMapping.Map<User, InternalRegisterExternalEvent>(newUserExternal));

            //mapeamos y retornamos el nuevo usuario interno
            return this.autoMapping.Map<User, InternalRegisterExternalDTO>(newUserExternal);
        }
    }
}
