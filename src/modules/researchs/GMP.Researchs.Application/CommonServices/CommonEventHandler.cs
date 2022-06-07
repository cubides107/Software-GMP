using GMP.Application.Exceptions;
using GMP.Researchs.Domain.Entities;
using GMP.Researchs.Domain.Factories;
using GMP.Researchs.Domain.Ports;
using GMP.Users.IntegrationEvent.EventAccessRoot;
using GMP.Users.IntegrationEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Researchs.Application.CommonServices
{
    public class CommonEventHandler
    {
        internal CommonEventHandler()
        {

        }

        public async Task BuildAndSaveUser<T>(T @event, IResearchsFactory researchsFactory, 
            IResearchsRepository researchsRepository) where T : UserEvent
        {
            User user;
            CancellationToken cancellationToken = new(false);

            //construimos el usuario
            user = (User) researchsFactory.BuildRegisterUser(@event.Mail, Guid.Parse(@event.Id));

            //guardamos el usuario
            user = await researchsRepository.Save<User>(user, cancellationToken);
            if (user is null)
                throw new SaveNullException(SaveNullException.MESSAGE);
        }
    }
}
