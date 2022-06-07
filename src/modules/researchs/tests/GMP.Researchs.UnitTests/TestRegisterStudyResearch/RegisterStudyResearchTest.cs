using GMP.Domain;
using GMP.Researchs.Application.StudyResearchServices.CommandRegisterStudyResearch;
using GMP.Researchs.Domain.Entities;
using GMP.Researchs.Domain.Factories;
using GMP.Researchs.Domain.Ports;
using JKang.EventBus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Researchs.UnitTests.TestCommandRegisterStudyResearch
{
    [TestClass]
    public class RegisterStudyResearchTest
    {
        private readonly Mock<IResearchsFactory> researchsFactory;
        private readonly Mock<IResearchsRepository> repository;
        private readonly Mock<IUserSecurity> security;
        private readonly Mock<IUtilities> utilities;
        private readonly Mock<IResearchsRepositoryBlob> researchsRepositoryBlob;

        private readonly IResearchsFactory factory;

        public RegisterStudyResearchTest()
        {
            researchsFactory = new Mock<IResearchsFactory>();
            repository = new Mock<IResearchsRepository>();
            security = new Mock<IUserSecurity>();
            utilities = new Mock<IUtilities>();

            this.factory = new ResearchsFactory();
        }

        [TestMethod]
        public void TestRegisterStudyResearch()
        {
            
        }

    }
}
