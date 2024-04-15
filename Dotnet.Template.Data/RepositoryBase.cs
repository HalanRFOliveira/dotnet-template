using Microsoft.EntityFrameworkCore.Storage;

namespace Dotnet.Template.Data
{
    public abstract class RepositoryBase
    {
        protected readonly MySqlContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the Entity class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        protected RepositoryBase(MySqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
    }
}
