using Dotnet.Template.Infra.CrossCutting.Domain;

namespace Dotnet.Template.Infra.Integration
{
	public interface IIntegrationService<T> where T : IEntity
	{
		Task<bool> UpdateAsync(object id, int attempts = 0);

		Task UpdateAsync();
	}
}
