using Microsoft.EntityFrameworkCore.Storage;

namespace Dotnet.Template.Data
{
    public abstract class RepositoryBase
    {
        protected readonly MySqlContext _dbContext;

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
