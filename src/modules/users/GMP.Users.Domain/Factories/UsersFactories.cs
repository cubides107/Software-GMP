using GMP.Domain;
using GMP.Users.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Domain.Factories
{
    public class UsersFactories : IUsersFactories
    {
        public Entity BuildRegisterUserInternal(string name, string lastname, string phone, 
            string mail, string password, string address, string token, Guid? id = null)
        {
            return new User(name, lastname, phone, mail, password, address, token, id);
        }
    }
}
