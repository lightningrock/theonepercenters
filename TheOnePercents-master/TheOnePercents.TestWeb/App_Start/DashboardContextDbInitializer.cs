using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TheOnePercents.TestWeb.Models;
using System.Transactions;

namespace TheOnePercents.TestWeb
{
    public class DashboardContextDbInitializer : DropCreateDatabaseAlways<DashboardContext>
    {
        protected override void Seed(DashboardContext context)
        {
            context.Task.Add(new Task() { Name = "Training", Active = true, Points = 5 });
            context.Task.Add(new Task() { Name = "Stretching", Active = true, Points = 1 });
            context.Task.Add(new Task() { Name = "Diet", Active = true, Points = 1 });
            context.Task.Add(new Task() { Name = "Hydration", Active = true, Points = 1 });
            context.Task.Add(new Task() { Name = "Sleep", Active = true, Points = 1 });
        }
    }
}

public class ReallyDoDropCreateDatabaseIfModelChanges<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
{
    protected const string Sql =
        "if (select DB_ID('{0}')) is not null\n"
        + "begin\n"
        + "alter database [{0}] set offline with rollback immediate;\n"
        + "alter database [{0}] set online;\n"
        + "drop database [{0}];\n"
        + "end\n";

    public virtual void InitializeDatabase(TContext context)
    {
        if (DbExists(context))
        {
            //if (context.Database.CompatibleWithModel(false))
            //    return;

            DropDatabase(context);
            //context.Database.Delete();
            context.Database.Create();
            Seed(context);
        }
        
        context.SaveChanges();
    }

    protected virtual bool DbExists(TContext context)
    {
        using (new TransactionScope(TransactionScopeOption.Suppress))
        {
            return context.Database.Exists();
        }
    }

    protected virtual void DropDatabase(TContext context)
    {
        var cmdText = string.Format(Sql, context.Database.Connection.Database);
        context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, cmdText);
    }

    protected virtual void Seed(TContext context) { }
}