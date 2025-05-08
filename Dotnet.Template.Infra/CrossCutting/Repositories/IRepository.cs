using Microsoft.EntityFrameworkCore.Storage;

namespace Dotnet.Template.Infra.CrossCutting.Repositories
{
	public interface IRepository<TEntity>
	{
		/// <summary>
		/// Finds the entity by the key.
		/// </summary>
		/// <returns>The found entity.</returns>
		/// <param name="key">Key.</param>
		TEntity FindBy(object key);

		/// <summary>
		/// Insert the specified entity.
		/// </summary>
		/// <param name="item">The entity.</param>
		Task<int> Insert(TEntity item);

		/// <summary>
		/// Update the specified entity.
		/// </summary>
		/// <param name="item">The entity.</param>
		Task<int> Update(TEntity item);

		/// <summary>
		/// Remove the specified entity.
		/// </summary>
		/// <param name="item">The entity.</param>
		void Remove(TEntity item);
	}

	public interface IRepository
	{
		IDbContextTransaction BeginTransaction();
	}
}
