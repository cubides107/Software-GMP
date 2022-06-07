﻿using GMP.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Users.Application.UsersRootServices.CommandRootRegistersUserInternal
{
    public class RegistersUserInternalDTO : DTO<RegistersUserInternalDTO>
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Phone { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        public string Address { get; set; }
    }
}
