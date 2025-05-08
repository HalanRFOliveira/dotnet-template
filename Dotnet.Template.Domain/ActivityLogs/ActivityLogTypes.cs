using System.ComponentModel.DataAnnotations;

namespace Dotnet.Template.Domain.ActivityLogs
{
	public enum ActivityLogType
	{
		[Display(Name = "Usuário - Adicionar")]
		AddUser = 1,
		[Display(Name = "Usuário - Deletar")]
        DeleteUser = 2,
		[Display(Name = "Usuário - Atualizar")]
        UpdateUser = 3,
		[Display(Name = "Autenticação - Atualizar Senha")]
        ResetPasswordEmail = 4,
    }
}
