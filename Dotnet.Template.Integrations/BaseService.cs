using Dotnet.Template.Infra.CrossCutting.Domain;
using Dotnet.Template.Infra.Integration;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Dotnet.Template.Integrations
{
    public abstract class BaseService<T> : IIntegrationService<T> where T : IEntity
	{
		protected readonly IConfiguration Configuration;
		public ILogger Logger { get; private set; }

		public int ItemsPerPage { get; }

		protected int ItemsUpdated { get; set; }

		protected int ItemsAdded { get; set; }

		protected int ItemsFailed { get; set; }

		public int UpdateTimeMaxAgeInDays { get; protected set; }

		public DateTime UpdateStartAt
		{
			get
			{
				return UpdateTimeMaxAgeInDays > 0 ? DateTime.Now.AddDays(-UpdateTimeMaxAgeInDays) : DateTime.MinValue;
			}
		}

		protected BaseService(IConfiguration configuration)
		{
			Configuration = configuration;
			Logger = Log.Logger;
			ItemsPerPage = Configuration.GetValue("MaxItemsPerRequest", 100);
			UpdateTimeMaxAgeInDays = 3;
		}

		/// <summary>
		/// Responsável por realizar a atualização do item com o identificador fornecido
		/// e controlar o nivel de tentativas para recursão quando tratar entidades dependentes
		/// </summary>
		/// <param name="id">Identificador do item</param>
		/// <param name="attempts">Usado para controlar a recursividade nas tentativas de atualizaçãoes de entidades dependentes</param>
		public abstract Task<bool> UpdateAsync(object id, int attempts = 0); 

		/// <summary>
		/// Responsável por buscar todos os dados do tipo alvo e realizar a verificação para atualizar
		/// </summary>
		public abstract Task UpdateAsync();

		/// <summary>
		/// Método responsável pela atualização do item na base de dados do Siedi.
		/// Quando detecta erro Relacional de FK ao inserir uma nova entidade, ou atualizar, este método
		/// deve chamar o método CheckDependencyEntitiesAsync e logo após realizar uma chamada recursiva
		/// com controle atrável do <paramref name="attempts"/>, não permitindo mais que uma unica nova tentativa por item.
		/// </summary>
		/// <param name="item">Item alvo da atualização</param>
		/// <param name="attempts">Número de tentativas, usado quando necessário tentar novamente após resolver as dependências de outras entidades</param>
		protected abstract Task<bool> UpdateAsync(T item, int attempts = 0);

		/// <summary>
		/// Implementar lógica de dependencia neste método.
		/// Chamar este método quando detectado erro para gravar a entidade alvo
		/// por falta de dados relacionados, de outras entidades.
		/// </summary>
		/// <param name="item"></param>
		protected virtual Task CheckDependencyEntitiesAsync(T item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Registra o total de itens adicionados
		/// </summary>
		protected void LogRegisterAdded()
		{
			Logger.Information($"{GetType().Name} | {ItemsAdded} registros tratados como novos.");
		}

		/// <summary>
		/// Registra o total de itens que necessitaram de atualização
		/// </summary>
		protected void LogRegisterUpdated()
		{
			Logger.Information($"{GetType().Name} | {ItemsUpdated} registros tratados como atualização.");
		}

		/// <summary>
		/// Registra o total de itens que geraram erro no processamento
		/// </summary>
		protected void LogRegisterProccessError()
		{
			Logger.Information($"{GetType().Name} | {ItemsFailed} erros ao processar.");
		}

		/// <summary>
		/// Registra uma mensagem como log de erro
		/// </summary>
		protected void LogError(string message)
		{
			Logger.Error($"{GetType().Name} | {message}");
		}

		/// <summary>
		/// Registra uma mensagem como log de erro
		/// </summary>
		protected void LogInfo(string message)
		{
			Logger.Information($"{GetType().Name} | {message}");
		}
	}
}
