using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent;
using JKang.EventBus;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersRootServices.CommandRootRegisterUserExternal
{
    public class RegisterUserExternalHandler : IRequestHandler<RegisterUserExternalCommand, RegisterUserExternalDTO>
    {
        private readonly IUsersRepositories usersRepositories;

        private readonly IUserSecurity userSecurity;

        private readonly IUsersFactories userFactory;

        private readonly IUtilities utilities;

        private readonly IAutoMapping autoMapping;

        private readonly IEventPublisher eventPublisher;


        public RegisterUserExternalHandler(IUsersRepositories usersRepositories, IUserSecurity userSecurity,
            IUsersFactories userFactory, IUtilities utilities, IAutoMapping autoMapping, IEventPublisher eventPublisher)
        {
            this.usersRepositories = usersRepositories;
            this.userSecurity = userSecurity;
            this.userFactory = userFactory;
            this.utilities = utilities;
            this.autoMapping = autoMapping;
            this.eventPublisher = eventPublisher;
        }

        public async Task<RegisterUserExternalDTO> Handle(RegisterUserExternalCommand request, CancellationToken cancellationToken)
        {
            string mailUserRoot;
            string mailUserRootRequest;
            string passwordUserRoot;
            Guid id;

            User newUserExternal;

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            //verificar si el mail pertenece al usuario root
            mailUserRoot = utilities.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT);
            mailUserRootRequest = userSecurity.GetClaim(request.RootClaims, IUserSecurity.MAIL);

            if (mailUserRoot != mailUserRootRequest)
                throw new NotRootException(NotRootException.MESSAGE_MAIL);

            //Verificar si existe el usuario root en la variables de entorno con la contraseña suministrada
            passwordUserRoot = userSecurity.EncryptPassword(utilities.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT));

            if (usersRepositories.Exists<User>(x => x.Mail == mailUserRootRequest && x.Password == passwordUserRoot) is false)
                throw new NotRootException(NotRootException.MESSAGE_PASSWORD);

            //entonces, creamos el nuevo usuario interno con el nuevo token si no existe el usuario con el mail suministrado
            if (this.usersRepositories.Exists<User>(x => x.Mail == request.NewUserExternal.Mail) == true)
                throw new UserExistsException(UserExistsException.MESSAGE);

            id = utilities.CreateId();

            //Encriptar contraseña
            string encryptedPassword = userSecurity.EncryptPassword(request.NewUserExternal.Password);

            //Creación de Usuario Externo
            newUserExternal = (User)userFactory.BuildUser(
                name: request.NewUserExternal.Name,
                lastname: request.NewUserExternal.Lastname,
                phone: request.NewUserExternal.Phone,
                mail: request.NewUserExternal.Mail,
                password: encryptedPassword,
                address: request.NewUserExternal.Address,
                tipoUsuarioEnumGuid: TipoUsuarioEnum.externo,
                id: id);

            newUserExternal = await usersRepositories.Save<User>(newUserExternal, cancellationToken);
            if (newUserExternal is null)
                throw new SaveNullException(SaveNullException.MESSAGE);

            //publicar el evento
            await eventPublisher.PublishEventAsync(autoMapping.Map<User,RootRegisterUserExternalEvent>(newUserExternal));

            //Mapeo y retorno del nuevo usuario externo
            return autoMapping.Map<User, RegisterUserExternalDTO>(newUserExternal);
        }
    }
}
