using Dotnet.Template.Infra.Messaging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dotnet.Template.Infra.Paging
{
    public class CommandWithFilter<TFilter, TResultSet>
      : Command<PagedResult<TResultSet>>, IRequest<PagedResult<TResultSet>>
      where TFilter : class, new()
      where TResultSet : class
    {
        public CommandWithFilter()
        {
            Filter = new Filter<TFilter>();
        }

        [FromQuery]
        public Filter<TFilter> Filter { get; set; }

        private string queryStringFilter;

        [FromQuery(Name = "filter")]
        public string QueryStringFilter
        {
            get
            {
                return queryStringFilter;
            }
            set
            {
                queryStringFilter = value;
                Filter = JsonConvert.DeserializeObject<Filter<TFilter>>(queryStringFilter);
            }
        }
    }
}
