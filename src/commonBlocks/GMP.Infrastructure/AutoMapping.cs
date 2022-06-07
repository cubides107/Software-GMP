using AutoMapper;
using GMP.Domain;
using GMP.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMP.Infrastructure
{
    public class AutoMapping : IAutoMapping
    {
        private readonly IMapper mapper;

        public AutoMapping(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            try
            {
                return mapper.Map<TSource, TDestination>(source);
            }
            catch (Exception e)
            {

                throw new AutoMappingException($"{AutoMappingException.MESSAGE} {e.Message}");
            }
        }
    }
}
