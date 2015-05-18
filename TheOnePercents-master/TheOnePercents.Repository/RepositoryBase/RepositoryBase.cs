using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data;
using System.Data.Objects;
using TheOnePercents.Repository.Infrastructure;
using TheOnePercents.Repository;
using TheOnePercents.Model;
using System.Transactions;



namespace TheOnePercents.Repository
{
    public class RepositoryBase<C> : IDisposable, IRepositoryBase<theonepercentersEntities1>
           where C : DbContext, new()
    {
        private C _DataContext;
        private readonly PluralizationService _pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));

        public virtual C DataContext
        {
            get
            {
                if (_DataContext == null)
                {
                    _DataContext = new C();
                    this.AllowSerialization = true;
                    this._DataContext.Configuration.LazyLoadingEnabled = true;

                    // Test these?
                    this._DataContext.Configuration.AutoDetectChangesEnabled = false;
                    this._DataContext.Configuration.ValidateOnSaveEnabled = false;
                    //Disable ProxyCreationDisabled to prevent the "In order to serialize the parameter, add the type to the known types collection for the operation using ServiceKnownTypeAttribute" error
                }
                return _DataContext;
            }
        }


        //public DbContext GetDataContext() {
        //    return _DataContext;
        //}

        public virtual bool AllowSerialization
        {
            get
            {
                //return ((IObjectContextAdapter) _DataContext)
                //.ObjectContext.ContextOptions.ProxyCreationEnabled = false;
                return _DataContext.Configuration.ProxyCreationEnabled;
            }
            set
            {
                //_DataContext.Configuration.ProxyCreationEnabled = !value;
                _DataContext.Configuration.ProxyCreationEnabled = true;
            }
        }


        public virtual void DetachObject<TEntity>() where TEntity : class
        {

        }


        public IQueryable<TEntity> GetQuery<TEntity>() where TEntity : class
        {
            var entityName = GetEntityName<TEntity>();
            return ((IObjectContextAdapter)DataContext).ObjectContext.CreateQuery<TEntity>(entityName);
        }

        private string GetEntityName<TEntity>() where TEntity : class
        {
            return string.Format("{0}.{1}", ((IObjectContextAdapter)DataContext).ObjectContext.DefaultContainerName, _pluralizer.Pluralize(typeof(TEntity).Name));
        }

        public virtual T Get<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            if (predicate != null)
            {
                return DataContext.Set<T>().Where(predicate).SingleOrDefault();
            }
            else
            {
                throw new ApplicationException("Predicate value must be passed to Get<T>.");
            }
        }

        public virtual IQueryable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            try
            {
                return DataContext.Set<T>().Where(predicate);
            }
            catch (Exception)
            {
                //TODO [CB] Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList<T, TKey>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, TKey>> orderBy) where T : class
        {
            try
            {
                return GetList(predicate).OrderBy(orderBy);
            }
            catch (Exception)
            {
                //TODO [CB] Log error
            }
            return null;
        }

        public virtual IQueryable<T> GetList<T, TKey>(Expression<Func<T, TKey>> orderBy) where T : class
        {
            try
            {
                return GetList<T>().OrderBy(orderBy);
            }
            catch (Exception ex)
            {
                //TODO [CB] Log error
                var message = ex.Message.ToString();
            }
            return null;
        }

        public virtual IQueryable<T> GetList<T>() where T : class
        {
            try
            {
                return DataContext.Set<T>();
            }
            catch (Exception ex)
            {
                //TODO [CB] Log error
                var message = ex.Message.ToString();
            }
            return null;
        }

        public virtual OperationStatus Save<T>(T entity, SaveOption saveOption) where T : class
        {
            OperationStatus opStatus = new OperationStatus { Status = true };
            try
            {
                switch (saveOption)
                {
                    case SaveOption.New:
                        //logger.Debug("Save New");
                        DataContext.Entry(entity).State = EntityState.Added;
                        //logger.Debug("Save New Finish");
                        break;
                    case SaveOption.Update:
                        //logger.Debug("Save Update");
                        DataContext.Entry(entity).State = EntityState.Modified;
                        //logger.Debug("Save Update Finish");
                        break;
                    case SaveOption.Delete:
                        DataContext.Entry(entity).State = EntityState.Deleted;
                        break;
                }

                opStatus.Status = DataContext.SaveChanges() > 0;
                return opStatus;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        opStatus.Status = false;
                        opStatus.Message = dbEx.Message;
                        //logger.Debug(validationError.PropertyName + " " + validationError.ErrorMessage);
                        //opStatus.ExceptionInnerMessage = dbEx.InnerException.ToString();
                    }
                }
                return opStatus;
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.Message = "Error Saving " + typeof(T) + ".";

                if (opStatus.OriginatingException == string.Empty || opStatus.OriginatingException == null)
                {
                    opStatus = OperationStatus.CreateFromException("Error Saving " + typeof(T) + ".", "RepositoryBase.Save<T>", exp);
                }
                throw;
            }
        }

        /// <summary>
        /// Save a Batch of Entites at the same time. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public OperationStatus SaveBatch<T>(ICollection<T> entities) where T : class
        {

            throw new NotImplementedException();

            //OperationStatus opStatus = new OperationStatus { Status = true };
            //try
            //{

            //    using (var transactionScope = new TransactionScope())
            //    {
            //        DataContext.BulkInsert(entities);

            //        DataContext.SaveChanges();
            //        transactionScope.Complete();
            //    }
            //    return opStatus;
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //            opStatus.Status = false;
            //            opStatus.Message = dbEx.Message;
            //            //logger.Debug(validationError.PropertyName + " " + validationError.ErrorMessage);
            //            //opStatus.ExceptionInnerMessage = dbEx.InnerException.ToString();
            //        }
            //    }
            //    return opStatus;
            //}
            //catch (Exception exp)
            //{
            //    opStatus.Status = false;
            //    opStatus.Message = "Error Saving " + typeof(T) + ".";

            //    if (opStatus.OriginatingException == string.Empty || opStatus.OriginatingException == null)
            //    {
            //        opStatus = OperationStatus.CreateFromException("Error Saving " + typeof(T) + ".", "RepositoryBase.Save<T>", exp);
            //    }
            //    throw;
            //}

        }

        public virtual OperationStatus Save<T>(T entity) where T : class
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.Message = "Error Saving " + typeof(T) + ".";

                if (opStatus.OriginatingException == string.Empty || opStatus.OriginatingException == null)
                {
                    opStatus = OperationStatus.CreateFromException("Error Saving " + typeof(T) + ".", "RepositoryBase.Save<T>", exp);
                }
                throw;
            }

            return opStatus;
        }

        public virtual OperationStatus Update<T>(T entity, params string[] propsToUpdate) where T : class
        {
            OperationStatus opStatus = new OperationStatus { Status = true };

            try
            {
                DataContext.Set<T>().Attach(entity);
                DataContext.Entry(entity).State = EntityState.Modified;
                opStatus.Status = DataContext.SaveChanges() > 0;
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.Message = "Error updating " + typeof(T) + ".";

                if (opStatus.OriginatingException == string.Empty || opStatus.OriginatingException == null)
                {
                    opStatus = OperationStatus.CreateFromException("Error updating " + typeof(T) + ".", "RepositoryBase.Update<T>", exp);
                }
                throw;
            }

            return opStatus;
        }

        public OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters)
        {
            var opStatus = new OperationStatus { Status = true, Message = "Operation Complete" };

            try
            {
                opStatus.RecordsAffected = DataContext.Database.ExecuteSqlCommand(cmdText, parameters);
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.Message = "Operation Failed";

                if (opStatus.OriginatingException == string.Empty || opStatus.OriginatingException == null)
                {
                    opStatus = OperationStatus.CreateFromException("Error executing store command: ", "RepositoryBase.ExecuteStoreCommand", exp);
                }

                throw;
            }
            return opStatus;
        }


        public OperationStatus ExecuteStoreCommand(string cmdText)
        {
            var opStatus = new OperationStatus { Status = true, Message = "Operation Complete" };

            try
            {
                opStatus.RecordsAffected = DataContext.Database.ExecuteSqlCommand(cmdText);
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.Message = "Operation Failed";

                if (opStatus.OriginatingException == string.Empty || opStatus.OriginatingException == null)
                {
                    opStatus = OperationStatus.CreateFromException("Error executing store command: ", "RepositoryBase.ExecuteStoreCommand", exp);
                }

                throw;
            }
            return opStatus;
        }

        public IQueryable<TEntity> Find<TEntity>(ISpecification<TEntity> criteria) where TEntity : class
        {
            return criteria.SatisfyingEntitiesFrom(GetQuery<TEntity>());
        }

        public virtual IQueryable<TEntity> AllIncluding<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
        {
            IQueryable<TEntity> query = DataContext.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T Find<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DataContext.Set<T>().Where(predicate).SingleOrDefault();
        }


        public IQueryable<T> GetAllNonTracking<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DataContext.Set<T>().Where(predicate).AsNoTracking();
        }

        public virtual IQueryable<TEntity> AllIncludingNonTracking<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
        {
            IQueryable<TEntity> query = DataContext.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsNoTracking();
        }

        public T FindNonTracking<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DataContext.Set<T>().Where(predicate).AsNoTracking().SingleOrDefault();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            ObjectContext oc = ((IObjectContextAdapter)DataContext).ObjectContext;
            oc.DeleteObject(entity);

            oc.SaveChanges();
        }


        public void Dispose()
        {
            if (DataContext != null) DataContext.Dispose();
        }


    }

    public interface IRepositoryBase<C> where C : DbContext, new()
    {
        T Get<T>(Expression<Func<T, bool>> predicate) where T : class;

        IQueryable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class;

        IQueryable<T> GetList<T, TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderBy) where T : class;

        IQueryable<T> GetList<T, TKey>(Expression<Func<T, TKey>> orderBy) where T : class;

        IQueryable<T> GetList<T>() where T : class;

        OperationStatus Update<T>(T entity, params string[] propsToUpdate) where T : class;

        OperationStatus Save<T>(T entity) where T : class;

        OperationStatus Save<T>(T entity, SaveOption saveOption) where T : class;

        IQueryable<TEntity> Find<TEntity>(ISpecification<TEntity> criteria) where TEntity : class;

        IQueryable<TEntity> AllIncluding<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;

        IQueryable<TEntity> AllIncludingNonTracking<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;

        T Find<T>(Expression<Func<T, bool>> predicate) where T : class;

        T FindNonTracking<T>(Expression<Func<T, bool>> predicate) where T : class;

        IQueryable<T> GetAllNonTracking<T>(Expression<Func<T, bool>> predicate) where T : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        OperationStatus ExecuteStoreCommand(string cmdText, params object[] parameters);

        OperationStatus ExecuteStoreCommand(string cmdText);

        OperationStatus SaveBatch<T>(ICollection<T> entities) where T : class;

    }
}
