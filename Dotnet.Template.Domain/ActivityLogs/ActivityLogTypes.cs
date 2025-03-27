using System.ComponentModel.DataAnnotations;

namespace Dotnet.Template.Domain.ActivityLogs
{
	public enum ActivityLogType
	{
		[Display(Name = "Usuário - Adicionar")]
		AddUser = 1,
    }
}
