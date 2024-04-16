using Dotnet.Template.Infra.Mediator;
using Dotnet.Template.Infra.Paging;
using Dotnet.Templates.Domain.ActivityLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dotnet.Template.WebApi.Controllers
{
    /// <summary>
    /// Classe controladora de ActivityLogs
    /// </summary>
    //[Authorize(Roles = "Template.AdminAccess")]
    [Route("api/[controller]")]
    public class ActivityLogs : ApiController
    {
        private readonly IMediatorHandler _mediator;

        /// <summary>
        /// Injeta mediator na classe
        /// </summary>
        /// <param name="mediator"></param>
        public ActivityLogs(IMediatorHandler mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obté dados de logs de atividades
        /// </summary>
        /// <returns>Os negócios realizados</returns>
        /// <response code="200">Se algum log foi encontrado.</response>
        /// <response code="204">Se nenhum log foi encontrado.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="403">Usuário não autorizado.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<GetActivityLogsCommandResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActivityLogs([FromQuery] GetActivityLogsCommand command)
        {
            var response = await _mediator.SendCommandAsync(command);

            return CustomResponse(response);
        }
    }
}
