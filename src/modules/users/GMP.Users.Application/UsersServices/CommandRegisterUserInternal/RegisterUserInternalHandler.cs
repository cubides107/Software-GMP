using GMP.Domain;
using GMP.Users.Domain.Factories;
using GMP.Users.Domain.Ports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersServices.CommandRegisterUserInternal
{
    public class RegisterUserInternalHandler : IRequestHandler<RegisterUserInternalCommand, RegisterUserInternalDTO>
    {
        private readonly IUsersRepositories usersRepositories;

        private readonly IAutoMapping autoMapping;

        private readonly IUsersFactories factory;

        public RegisterUserInternalHandler(IUsersRepositories usersRepositories, IAutoMapping autoMapping, IUsersFactories factory)
        {
            this.autoMapping = autoMapping;
            this.usersRepositories = usersRepositories;
            this.factory = factory;
        }

        public async Task<RegisterUserInternalDTO> Handle(RegisterUserInternalCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
