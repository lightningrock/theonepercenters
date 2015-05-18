using System.Linq.Expressions;
using System;
using System.Linq;
using System.Data.Entity;

namespace TheOnePercents.Repository
{
    public class Specification<TEntity> : ISpecification<TEntity>
    {
        public Specification(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }

        public AndSpecification<TEntity> And(Specification<TEntity> specification)
        {
            return new AndSpecification<TEntity>(this, specification);
        }

        public OrSpecification<TEntity> Or(Specification<TEntity> specification)
        {
            return new OrSpecification<TEntity>(this, specification);
        }

        public TEntity SatisfyingEntityFrom(IQueryable<TEntity> query)
        {
            return query.Where(Predicate).SingleOrDefault();
        }

        public IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(Predicate);
        }

        public Expression<Func<TEntity, bool>> Predicate;
    }
}