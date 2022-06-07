using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Domain.Exceptions;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent.EventAccessRoot;
using JKang.EventBus;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersRootServices.CommandAccessRoot
{
    public class AccessRootHandler : IRequestHandler<AccessRootCommand, AccessRootDTO>
    {
        private readonly IUsersRepositories usersRepositories;

        private readonly IUserSecurity userSecurity;

        private readonly IUsersFactories userFactory;

        private readonly IUtilities utilities;

        private readonly IEventPublisher eventPublisher;

        private readonly IAutoMapping autoMapping;

        public AccessRootHandler(IUsersRepositories usersRepositories, IUserSecurity userSecurity, IUsersFactories usersFactories,
            IUtilities utilities, IEventPublisher eventPublisher, IAutoMapping autoMapping)
        {
            this.usersRepositories = usersRepositories;
            this.userSecurity = userSecurity;
            this.userFactory = usersFactories;
            this.utilities = utilities;
            this.eventPublisher = eventPublisher;
            this.autoMapping = autoMapping;
        }

        public async Task<AccessRootDTO> Handle(AccessRootCommand request, CancellationToken cancellationToken)
        {
            string mailRoot;
            string encriptedPasswordRoot;
            User userRoot;
            string token;
            Guid id;

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            //verificar si es un usuario root
            mailRoot = this.utilities.GetEnvironmentVariable(IUtilities.MAIL_USER_ROOT);
            encriptedPasswordRoot = userSecurity.EncryptPassword(this.utilities.GetEnvironmentVariable(IUtilities.PASSWORD_USER_ROOT));

            if (mailRoot != request.Mail)
                throw new NotRootException(NotRootException.MESSAGE_MAIL);

            else if (encriptedPasswordRoot != this.userSecurity.EncryptPassword(request.Password))
                throw new NotRootException(NotRootException.MESSAGE_PASSWORD);

            //si existe en la db lo eliminamos
            if (usersRepositories.Exists<User>(x => x.Mail == request.Mail) == true)
            {
                userRoot = await this.usersRepositories.Get<User>(x => x.Mail == request.Mail, cancellationToken);
                await this.usersRepositories.Delete<User>(userRoot, cancellationToken);
            }

            //luego, creamos el usuario root
            id = this.utilities.CreateId();
            token = this.userSecurity.CreateToken(id, mailRoot, mailRoot, TipoUsuarioEnum.root.ToString());

            userRoot = (User)this.userFactory.BuilRegisterUserRoot(request.Mail, encriptedPasswordRoot, token, id);
            userRoot = await this.usersRepositories.Save<User>(userRoot, cancellationToken);
            if (userRoot is null)
                throw new SaveNullException(SaveNullException.MESSAGE);

            //mapeamos y publicamos
            await this.eventPublisher.PublishEventAsync<AccessRootEvent>(
                this.autoMapping.Map<User, AccessRootEvent>(userRoot));

            //retornamos
            return new AccessRootDTO
            {
                Token = userRoot.Token
            };
        }
    }
}
