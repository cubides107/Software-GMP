using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public Task<T> Update<T>(T obj, CancellationToken cancellationToken) where T : Entity;

        /// <summary>
        /// retorna el usuario por id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> Get<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken) where T : Entity;

        /// <summary>
        /// verificar si existe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Exists<T>(Expression<Func<T, bool>> expression) where T : Entity;

        /// <summary>
        /// retornamos una lista teniendo en cuenta el filtro, pagina, numero de pagina y condicion.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="expressionNest"></param>
        /// <param name="expressionConditional"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<T>> GetAll<T>(Expression<Func<T, string>> sort, int page, int pageSize, 
            Expression<Func<T, bool>> expressionConditional, CancellationToken cancellationToken) where T : Entity;

        /// <summary>
        /// eliminar objetos
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Delete<T>(T obj, CancellationToken cancellationToken) where T : Entity;

        /// <summary>
        /// calcula el total de registros segun la conticion
        /// retorna un numero que representa el total de registros que tiene la busqueda segun la exprecion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressionConditional"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> Count<T>(Expression<Func<T, bool>> expressionConditional, CancellationToken cancellationToken) where T : Entity;
    }
}
