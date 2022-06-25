using System;
namespace Dapper.Repository.Base
{
    public interface IDataRepository<TEntity>
    {
        public IEnumerable<TEntity> GetAll();

        TEntity Get(decimal id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}

