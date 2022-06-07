using GMP.Application.Exceptions;
using GMP.Researchs.Application.CommonServices;
using GMP.Researchs.Domain.Entities;
using GMP.Researchs.Domain.Factories;
using GMP.Researchs.Domain.Ports;
using GMP.Users.IntegrationEvent.EventAccessRoot;
using JKang.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Researchs.Application.UsersEventsHandlers
{
    public class AccessRootEventHandler : IEventHandler<AccessRootEvent>
    {
        private readonly IResearchsFactory researchsFactory;

        private readonly IResearchsRepository researchsRepository;

        /// <summary>
        /// asosiacion por composicion
        /// </summary>
        private readonly CommonEventHandler commonEventHandler;

        public AccessRootEventHandler(IResearchsFactory researchsFactory, IResearchsRepository researchsRepository)
        {
            this.researchsFactory = researchsFactory;
            this.researchsRepository = researchsRepository;
            commonEventHandler = new CommonEventHandler();
        }

        public async Task HandleEventAsync(AccessRootEvent @event)
        {
            await this.commonEventHandler.BuildAndSaveUser<AccessRootEvent>(@event, this.researchsFactory, this.researchsRepository);
        }
    }
}
