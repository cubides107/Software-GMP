using GMP.Domain;
using GMP.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMP.Infrastructure
{
    public abstract class Repository : IRepository
    {
        protected readonly DbContext context;

        protected Repository(DbContext context)
        {
            this.context = context;
        }

        public virtual async Task Delete<T>(T obj, CancellationToken cancellationToken) where T : Entity
        {
            try
            {
                context.Set<T>().Remove(obj);

                //confirma que se elimino el objeto
                if (await context.SaveChangesAsync(cancellationToken) < 0)
                    throw new SqlDbException($"{SqlDbException.MESSAGE_NOT_SAVE} {obj.GetType()}");

            }
            catch (SqlDbException e)
            {
                throw new SqlDbException($"{SqlDbException.MESSAGE_NOT_EXIST} {e.Message}");
            }
        }

        public virtual bool Exists<T>(Expression<Func<T, bool>> expression) where T : Entity
        {
            try
            {
                return context.Set<T>().AsQueryable().Any(expression);
            }
            catch (SqlDbException e)
            {

                throw new SqlDbException($"{SqlDbException.MESSAGE_NOT_EXIST} {e.Message}");
            }
        }

        public virtual async Task<T> Get<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken) where T : Entity
        {
            try
            {
                return await context.Set<T>().FirstOrDefaultAsync(expression, cancellationToken);
            }
            catch (SqlDbException e)
            {
                throw new SqlDbException($"{SqlDbException.MESSAGE_NOT_GET} {e.Message}");
            }
        }

        public virtual async Task<List<T>> GetAll<T>(Expression<Func<T, string>> sort, int page, int pageSize, 
            Expression<Func<T, bool>> expressionConditional, CancellationToken cancellationToken) where T : Entity
        {
            try
            {
                int skipRows = page * pageSize;
                return await context.Set<T>()
                    .Where(expressionConditional)
                    .OrderBy(sort)
                    .Skip(skipRows)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            }
            catch (SqlDbException e)
            {
                throw new SqlDbException($"{SqlDbException.MESSAGE_NOT_GET_QUERY_LIST} {e.Message}");
            }
        }

        public virtual async Task<T> Save<T>(T obj, CancellationToken cancellationToken) where T : Entity
        {
            try
            {
                var entity = await context.Set<T>().AddAsync(obj, cancellationToken);

                //confirma que se añadio el objeto
                if (await context.SaveChangesAsync(cancellationToken) < 0)
                    throw new SqlDbException($"{SqlDbException.MESSAGE_NOT_SAVE} {obj.GetType()}");

                return entity.Entity;
            }
            catch (SqlDbException e)
            {
                throw new SqlDbException($"{e.Message}");
            }
        }

        public virtual async Task<T> Update<T>(T obj, CancellationToken cancellationToken) where T : Entity
        {
            try
            {
                context.Entry(await context.Set<T>().FirstOrDefaultAsync(x => x.Id == obj.Id)).CurrentValues.SetValues(obj);

                if (await context.SaveChangesAsync(cancellationToken) < 0)
                    throw new SqlDbException($"{SqlDbException.MESSAGE_NOT_UPDATE}: {obj.GetType()}");

                return await context.Set<T>().FirstOrDefaultAsync(x => x.Id == obj.Id);
            }
            catch (SqlDbException e)
            {
                throw new SqlDbException($"{SqlDbException.MESSAGE_NOT_UPDATE} {e.Message}");
            }
        }

        public async Task<int> Count<T>(Expression<Func<T, bool>> expressionConditional, CancellationToken cancellationToken) where T : Entity
        {
            try
            {
                return await context.Set<T>().CountAsync(expressionConditional, cancellationToken);
            }
            catch (SqlDbException e)
            {
                throw new SqlDbException($"{SqlDbException.MESSAGE_NOT_GET} {e.Message}");
            }
        }
    }
}
