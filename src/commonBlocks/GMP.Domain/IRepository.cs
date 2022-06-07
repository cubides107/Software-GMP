using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Domain
{
    public interface IRepository
    {
        /// <summary>
        /// guarda una entidad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<T> Save<T>(T obj, CancellationToken cancellationToken) where T : Entity;

        /// <summary>
        /// actualizar la entidad en db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Update<T>(T obj, CancellationToken cancellationToken) where T : Entity;

        /// <summary>
        /// retorna el usuario por id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> Get<T>(string id) where T : Entity;

        /// <summary>
        /// verificar si existe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Exists<T>(Func<T, bool> expression) where T : Entity;
    }
}
