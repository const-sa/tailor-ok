using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SewingSystem.LinqModel;

namespace SewingSystem.Data
{
    /// <summary>
    /// Thin data-access layer over the LINQ-to-SQL <see cref="DataClasses1DataContext"/>.
    ///
    /// Centralizes two things that were copy-pasted across the forms:
    ///   * creating the context with <c>Program.ConnectionString</c>, and
    ///   * the "delete-then-insert" save pattern keyed by a match predicate.
    ///
    /// Behaviour is intentionally identical to the previous inline code so forms
    /// can be migrated one at a time without functional change.
    /// </summary>
    public class Repository<T> where T : class
    {
        private static DataClasses1DataContext CreateContext()
        {
            return new DataClasses1DataContext(Program.ConnectionString);
        }

        /// <summary>All rows for the entity type (detached list, safe for binding).</summary>
        public List<T> GetAll()
        {
            using (var db = CreateContext())
                return db.GetTable<T>().ToList();
        }

        /// <summary>
        /// Insert when <paramref name="isNew"/>, otherwise delete the rows matching
        /// <paramref name="matchExisting"/> and re-insert — mirroring the original
        /// per-form save logic.
        /// </summary>
        public void Save(T entity, Expression<Func<T, bool>> matchExisting, bool isNew)
        {
            using (var db = CreateContext())
            {
                var table = db.GetTable<T>();
                if (!isNew)
                    table.DeleteAllOnSubmit(table.Where(matchExisting));
                table.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
        }

        /// <summary>Delete all rows matching the predicate.</summary>
        public void Delete(Expression<Func<T, bool>> match)
        {
            using (var db = CreateContext())
            {
                var table = db.GetTable<T>();
                table.DeleteAllOnSubmit(table.Where(match));
                db.SubmitChanges();
            }
        }
    }
}
