﻿using System.Data.Entity;
using System.IO;

namespace SQLite.CodeFirst
{
    /// <summary>
    /// An implementation of <see cref="IDatabaseInitializer{TContext}"/> that will always recreate and optionally re-seed the 
    /// database the first time that a context is used in the app domain. To seed the database, create a derived class and override the Seed method.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    public class SqliteDropCreateDatabaseAlways<TContext> : SqliteInitializerBase<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteDropCreateDatabaseAlways{TContext}"/> class.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public SqliteDropCreateDatabaseAlways(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        { }

        /// <summary>
        /// Initialize the database for the given context.
        /// Generates the SQLite-DDL from the model and executs it against the database.
        /// After that the <see cref="Seed" /> method is executed.
        /// All actions are be executed in transactions.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void InitializeDatabase(TContext context)
        {
            string databseFilePath = GetDatabasePathFromContext(context);

            if (!IsMemoryDb(databseFilePath))
            {
                bool exists = File.Exists(databseFilePath);
                if (exists)
                {
                    File.Delete(databseFilePath);
                }
            }

            base.InitializeDatabase(context);
        }
    }
}
