using AutoMapper;
using GMP.Users.Application.UsersRootServices.CommandRootRegistersUserInternal;
using GMP.Users.Application.UsersRootServices.CommandRootRegisterUserExternal;
using GMP.Users.Application.UsersServices.CommadEditUser;
using GMP.Users.Application.UsersServices.CommandInternalRegisterExternal;
using GMP.Users.Application.UsersServices.QueryUser;
using GMP.Users.Application.UsersServices.QueryUsers;
using GMP.Users.Domain.Entities;
using GMP.Users.IntegrationEvent.EventAccessRoot;
using GMP.Users.IntegrationEvent.EventEditUser;
using GMP.Users.IntegrationEvent.EventInternalRegisterExternal;
using GMP.Users.IntegrationEvent.EventLoginUser;
using GMP.Users.IntegrationEvent.EventLogOut;
using GMP.Users.IntegrationEvent.EventRestorePasswordUser;
using GMP.Users.IntegrationEvent.EventRootRegisterUserExternal;
using GMP.Users.IntegrationEvent.EventRootRegisterUserInternal;
using System.Collections.Generic;

namespace GMP.Users.Infrastructure.Startup
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            this.CreateMap<User, LoginUserEvent>();
            this.CreateMap<User, InternalRegisterExternalEvent>();
            this.CreateMap<User, InternalRegisterExternalDTO>();
            this.CreateMap<User, RootRegisterUserExternalEvent>();
            this.CreateMap<User, RegisterUserExternalDTO>();
            this.CreateMap<User, RootRegisterUserInternalEvent>();
            this.CreateMap<User, RegistersUserInternalDTO>();
            this.CreateMap<User, AccessRootEvent>();
            this.CreateMap<User, UsersByNameDTO.UsersDTO>();
            this.CreateMap<User, LogOutEvent>();
            this.CreateMap<User, QueryUserDTO>();
            this.CreateMap<User, EditUserDTO>();
            this.CreateMap<User, EventEditUser>();
            this.CreateMap<User, RestorePasswordUserEvent>();
        }
    }
}
