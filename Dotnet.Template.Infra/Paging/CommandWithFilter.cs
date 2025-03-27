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

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="CommandWithFilter{TFilter, TResultSet}"/>.
        /// </summary>
        public CommandWithFilter()
        {
            Filter = new Filter<TFilter>();
        }

        /// <summary>
        /// Obtém ou define o filtro.
        /// </summary>
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
