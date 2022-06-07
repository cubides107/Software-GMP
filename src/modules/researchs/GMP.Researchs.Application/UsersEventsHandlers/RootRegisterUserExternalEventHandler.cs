using GMP.Researchs.Application.CommonServices;
using GMP.Researchs.Domain.Factories;
using GMP.Researchs.Domain.Ports;
using GMP.Users.IntegrationEvent;
using JKang.EventBus;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Researchs.Application.UsersEventsHandlers
{
    public class RootRegisterUserExternalEventHandler : IEventHandler<RootRegisterUserExternalEvent>
    {
        private readonly IResearchsFactory researchsFactory;

        private readonly IResearchsRepository researchsRepository;

        /// <summary>
        /// asocoasion por composicion
        /// </summary>
        private readonly CommonEventHandler commonEventHandler;

        public RootRegisterUserExternalEventHandler(IResearchsFactory researchsFactory, IResearchsRepository researchsRepository)
        {
            this.researchsFactory = researchsFactory;
            this.researchsRepository = researchsRepository;
            this.commonEventHandler = new CommonEventHandler();
        }

        public async Task HandleEventAsync(RootRegisterUserExternalEvent @event)
        {
            await this.commonEventHandler.BuildAndSaveUser<RootRegisterUserExternalEvent>(@event, this.researchsFactory, this.researchsRepository);
        }
    }
}
