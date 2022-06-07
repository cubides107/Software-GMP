using GMP.Apis.Web.Exceptions;
using GMP.Apis.Web.Filters;
using GMP.Researchs.Application.StudyResearchServices.CommandRegisterStudyResearch;
using GMP.Researchs.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GMP.Apis.Web.ResearchsControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
    public class StudyResearchController : ControllerBase
    {
        /// <summary>
        /// atributo que representa el mediador entre la api y el cu
        /// </summary>
        private readonly IMediator mediator;

        public StudyResearchController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPut]
        [Route("RegisterStudyResearch")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [TypeFilter(typeof(ExceptionManagerConfigurationFilter))]
        public async Task<IActionResult> PutRegisterStudyResearch(IFormCollection formFiles)
        {
            RegisterStudyResearchCommand command = new();
            string key;

            //verifico el estado de la data
            if (!ModelState.IsValid)
                throw new ModelStateIsInvalid(ModelStateIsInvalid.MESSAGE_MODEL_IS_INVALID);

            //asigno cada formdata a cada parametros deserializado
            foreach (var formFile in formFiles)
            {
                key = formFile.Key;

                switch (key)
                {
                    case "solicitation":
                        var solicitud = JsonSerializer.Deserialize<SolicitationCommand>(formFile.Value);
                        command.Solicitation = solicitud;
                        break;

                    case "employee":
                        var empleado = JsonSerializer.Deserialize<EmployeeCommand>(formFile.Value);
                        command.Employee = empleado;
                        break;

                    case "address":
                        var direccion = JsonSerializer.Deserialize<AddressCommand>(formFile.Value);
                        command.Address = direccion;
                        break;

                    default:
                        break;
                }
                
            }

            //verificamos si tiene la file y se lo pasamos a la peticion
            if(formFiles.Files.Count > 0)
                command.File = formFiles.Files[0];

            //asignamos los claims
            command.Claims = User.Claims.ToList();

            //enviamos al mediador y retornamos
            var dto = await mediator.Send(command);
            return Ok(dto);
        }
    }
}
