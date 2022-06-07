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

namespace GMP.Users.Application.UsersRootServices.CommandRootRegistersUserInternal
{
    public class RegistersUserInternalHandler : IRequestHandler<RegistersUserInternalCommand, RegistersUserInternalDTO>
    {
        private readonly IUsersRepositories usersRepositories;

        private readonly IUserSecurity userSecurity;

        private readonly IUsersFactories userFactory;

        private readonly IUtilities utilities;

        private readonly IAutoMapping autoMapping;

        private readonly IEventPublisher eventPublisher;

        public RegistersUserInternalHandler(IUsersRepositories usersRepositories, IUserSecurity userSecurity,
            IUsersFactories usersFactories, IUtilities utilities, IAutoMapping autoMapping, IEventPublisher eventPublisher)
        {
            this.usersRepositories = usersRepositories;
            this.userSecurity = userSecurity;
            this.userFactory = usersFactories;
            this.utilities = utilities;
            this.autoMapping = autoMapping;
            this.eventPublisher = eventPublisher;
        }

        public async Task<RegistersUserInternalDTO> Handle(RegistersUserInternalCommand request, CancellationToken cancellationToken)
        {
            string mailRoot;
            string mailRootRequest;
            string passwordRoot;

            User newUserInternal;
            Guid id;

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            //verificar si el mail pertenece al root
            mailRoot = this.utilities.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT);
            mailRootRequest = this.userSecurity.GetClaim(request.RootClaims, IUserSecurity.MAIL);

            if (mailRoot != mailRootRequest)
                throw new NotRootException(NotRootException.MESSAGE_MAIL);

            //verificar si existe el root en las variables de entorno con la contraseña suministrada
            passwordRoot = userSecurity.EncryptPassword(this.utilities.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT));

            if (this.usersRepositories.Exists<User>(x => x.Mail == mailRootRequest && x.Password == passwordRoot) == false)
                throw new NotRootException(NotRootException.MESSAGE_DB_PASSWORD_NULL);

            //entonces, creamos el nuevo usuario interno con el nuevo token si no existe el usuario
            if (this.usersRepositories.Exists<User>(x => x.Mail == request.NewUserInternal.Mail) == true)
                throw new UserExistsException(UserExistsException.MESSAGE);

            id = this.utilities.CreateId();

            //Encriptar contraseña
            string encryptedPassword = userSecurity.EncryptPassword(request.NewUserInternal.Password);

            newUserInternal = (User)this.userFactory.BuildUser(
                name: request.NewUserInternal.Name,
                lastname: request.NewUserInternal.Lastname,
                phone: request.NewUserInternal.Phone,
                mail: request.NewUserInternal.Mail,
                password: encryptedPassword,
                address: request.NewUserInternal.Address,
                tipoUsuarioEnumGuid: TipoUsuarioEnum.interno,
                id: id);

            newUserInternal = await this.usersRepositories.Save<User>(newUserInternal, cancellationToken);

            if (newUserInternal is null)
                throw new SaveNullException(SaveNullException.MESSAGE);

            //Publicar el Evento
            await eventPublisher.PublishEventAsync(autoMapping.Map<User, RootRegisterUserInternalEvent>(newUserInternal));

            //mapeamos y retornamos el nuevo usuario interno
            return this.autoMapping.Map<User, RegistersUserInternalDTO>(newUserInternal);
        }
    }
}
