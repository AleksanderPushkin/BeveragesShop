using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BeveragesShop.Models;
using System.Threading.Tasks;

namespace BeveragesShop.Repository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        T GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected BeveragesShopContext RepositoryContext { get; set; }

        public RepositoryBase(BeveragesShopContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public T GetById(int id)
        {
            return this.RepositoryContext.Set<T>().Find(id);
        }

        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }
    }

    public interface ICoinsRepository : IRepositoryBase<Coins>
    {
    }

    public interface IProducersRepository : IRepositoryBase<Producers>
    {
    }

    public class CoinsRepository : RepositoryBase<Coins>, ICoinsRepository
    {
        public CoinsRepository(BeveragesShopContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
    public class ProducersRepository : RepositoryBase<Producers>, IProducersRepository
    {
        public ProducersRepository(BeveragesShopContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }

    public interface IRepositoryWrapper
    {
        ICoinsRepository Coins { get; }
        IProducersRepository Producers { get; }
        void Save();
    }


    public class RepositoryWrapper : IRepositoryWrapper
    {
        private BeveragesShopContext _repoContext;
        private ICoinsRepository _coins;
        private IProducersRepository _producers;

        public ICoinsRepository Coins
        {
            get
            {
                if (_coins == null)
                {
                    _coins = new CoinsRepository(_repoContext);
                }

                return _coins;
            }
        }

        public IProducersRepository Producers
        {
            get
            {
                if (_producers == null)
                {
                    _producers = new ProducersRepository(_repoContext);
                }

                return _producers;
            }
        }

        public RepositoryWrapper(BeveragesShopContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
