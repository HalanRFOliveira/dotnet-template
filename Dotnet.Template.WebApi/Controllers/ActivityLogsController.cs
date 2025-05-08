using Dotnet.Template.Domain.ActivityLogs;
using Dotnet.Template.Infra.Mediator;
using Dotnet.Template.Infra.Paging;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Template.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ActivityLogs : ApiController
    {
        private readonly IMediatorHandler _mediator;

        public ActivityLogs(IMediatorHandler mediator)
        {
            _mediator = mediator;
        }

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
