using GMP.Application.Exceptions;
using GMP.Domain;
using GMP.Users.Domain.Entities;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using GMP.Users.IntegrationEvent.EventEditUser;
using JKang.EventBus;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.CommadEditUser
{
    public class EditUserHandler : IRequestHandler<EditUserCommand, EditUserDTO>
    {
        private readonly IUserSecurity userSecurity;
        private readonly IUsersRepositories usersRepositories;
        private readonly IUsersFactories usersFactories;
        private readonly IUtilities utilities;
        private readonly IAutoMapping mapping;
        private readonly IEventPublisher eventPublisher;

        public EditUserHandler(IEventPublisher eventPublisher, IAutoMapping mapping, IUsersFactories usersFactories, IUserSecurity userSecurity, IUsersRepositories usersRepositories, IUtilities utilities)
        {
            this.mapping = mapping;
            this.usersFactories = usersFactories;
            this.userSecurity = userSecurity;
            this.usersRepositories = usersRepositories;
            this.utilities = utilities;
            this.eventPublisher = eventPublisher;
        }
        public async Task<EditUserDTO> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            string mailUser;
            string mailUserRequest;
            TipoUsuarioEnum typeUserRequest;
            User userActual;
            User userToEdit;
            Guid id;
            User userUpdated;

            if (request is null)
                throw new CommandRequestNullException(CommandRequestNullException.MESSAGE_REQUEST_ISNULL);

            //verificar si el usuario actual(Quien realiza la peticion) esta registrado
            mailUser = this.userSecurity.GetClaim(request.UserClaims, IUserSecurity.MAIL);

            if (this.usersRepositories.Exists<User>(x => x.Mail == mailUser) == false)
                throw new UserNotRegisterException(UserNotRegisterException.MESSAGE_USER_IS_NOT_REGISTER);

            //Obtener el usuario actual
            userActual = await usersRepositories.Get<User>(x => x.Mail == mailUser, cancellationToken);

            if (userActual == null)
                throw new EntityNotRecoveredException(EntityNotRecoveredException.MESSAGE_USER_NOT_RECOVERED);

            // Verificar que el usuario realizador de la peticion sea interno o root y que el usuario a editar este registrado
            if (userActual.TipoUsuario == TipoUsuarioEnum.externo)
                throw new AccesNotAutorizedUser(AccesNotAutorizedUser.MESSAGE_ACCESS_DENIED);
            else if (this.usersRepositories.Exists<User>(x => x.Id == request.UserIdToEdit) is false)
                throw new UserNotRegisterException(UserNotRegisterException.MESSAGE_USER_TO_EDIT_IS_NOT_REGISTER);

            //Se obtine el usuario que se va a editar por medio del Id
            userToEdit = await usersRepositories.Get<User>(x => x.Id == request.UserIdToEdit, cancellationToken);

            if (userToEdit == null)
                throw new EntityNotRecoveredException(EntityNotRecoveredException.MESSAGE_USER_NOT_RECOVERED);

            mailUserRequest = request.DataUserToEdit.Mail;
            typeUserRequest = request.DataUserToEdit.TipoUsuario;

            //Verificar que el tipo de usuario del request no sea root
            //Verificar que el correo de la peticion sea diferente al del usuario a editar y si el correo de la peticion ya esta registrado con otro usuario
            if (typeUserRequest == TipoUsuarioEnum.root) 
                throw new TypeUserRootException(TypeUserRootException.MESSAGE_TYPE_USER_IS_ROOT);
            else if (!mailUserRequest.Equals(userToEdit.Mail) && usersRepositories.Exists<User>(x => x.Mail == mailUserRequest))
                throw new UserExistsException(UserExistsException.MESSAGE_EXISTS_USER_MAIL);

            //Convertir el id a tipo Guid 
            id = utilities.CreateId(userToEdit.Id);

            //Construir el usuario con los nuevos campos y el token null
            userUpdated = (User)usersFactories.BuildUser(request.DataUserToEdit.Name, request.DataUserToEdit.LastName, request.DataUserToEdit.Phone,
                mailUserRequest, userToEdit.Password, request.DataUserToEdit.Address, typeUserRequest, id: id);

            //Actualizar el usuario
            userUpdated = await usersRepositories.Update<User>(userUpdated, cancellationToken);

            if (userUpdated is null)
                throw new UpdateNullEntityException(UpdateNullEntityException.MESSAGE_ENTITY_IS_NULL);

            //Publicar el evento
            await eventPublisher.PublishEventAsync(mapping.Map<User, EventEditUser>(userUpdated));

            //Mapear y retornar objeto de transferencia de Datos
            return mapping.Map<User, EditUserDTO>(userUpdated);
        }
    }
}
