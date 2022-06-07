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

namespace GMP.Users.Application.UsersServices.CommandLogOut
{
    public class LogOutUserHandler : IRequestHandler<LogOutUserCommand, LogOutUserDTO>
    {
        private readonly IUsersRepositories usersRepositories;
        private readonly IUtilities utilities;
        private readonly IUsersFactories usersFactory;
        private readonly IAutoMapping autoMapping;
        private readonly IUserSecurity userSecurity;
        private readonly IEventPublisher eventPublisher;
       

        public LogOutUserHandler(IUsersRepositories usersRepositories, IUtilities utilities, IUsersFactories usersFactory, IAutoMapping autoMapping,IUserSecurity userSecurity, IEventPublisher eventPublisher)
        {
            this.usersRepositories = usersRepositories;
            this.utilities = utilities;
            this.usersFactory = usersFactory;
            this.autoMapping = autoMapping;
            this.userSecurity = userSecurity;
            this.eventPublisher = eventPublisher;

        }
        public async Task<LogOutUserDTO> Handle(LogOutUserCommand request, CancellationToken cancellationToken)
        {
            User user;
            Guid id;
            string mailUser;

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            mailUser = userSecurity.GetClaim(request.UserClaims,IUserSecurity.MAIL);
          
            //Obtener usuario y verificar si ha cerrado sesion previamente
            user = await usersRepositories.Get<User>(x => x.Mail == mailUser, cancellationToken);

            if (user is null)
                throw new EntityNotRecoveredException(EntityNotRecoveredException.MESSAGE_USER_NOT_RECOVERED);

            if (user.Token is null)
                throw new TokenNullException(TokenNullException.MESSAGE);

            id = utilities.CreateId(user.Id);

            //Crear Usuario con el token nulo y actulizar el mismo en la db
            user = (User)usersFactory.BuildUser(user.Name, user.Lastname, user.Phone, user.Mail, user.Password, user.Address, user.TipoUsuario, id:id);

            user = await usersRepositories.Update(user, cancellationToken);

            if (user is null)
                throw new UpdateNullEntityException(UpdateNullEntityException.MESSAGE_ENTITY_IS_NULL);

            //Publicar el Evento
            await eventPublisher.PublishEventAsync(autoMapping.Map<User, LogOutEvent>(user));

            return new LogOutUserDTO
            {};
        }
    }
}
