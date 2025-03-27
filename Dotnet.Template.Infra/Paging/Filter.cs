using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Template.Infra.Paging
{
    public class Filter<T> where T : class, new()
    {
        /// <summary>
		/// Inicia uma nova instância da classe <see cref="Filter{T}"/>.
		/// </summary>
		public Filter()
        {
            Data = new T();
        }

        /// <summary>
        /// Obtém ou define os dados utilizados como filtro.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Obtém ou define a página (Padrão: 1).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Obtém ou define a quantidade de registros (Padrão: 10).
        /// </summary>
        [FromQuery]
        public int RowsPerPage { get; set; } = 10;

        /// <summary>
        /// Obtém ou define a propriedade de ordenação (Padrão: Id).
        /// </summary>
        public string SortBy { get; set; } = "id";

        /// <summary>
        /// Obtém ou define a propriedade de ordenação (Padrão: Id).
        /// </summary>
        public bool? Descending { get; set; } = null;

        public bool HasSorter
        {
            get
            {
                return Descending.HasValue && !string.IsNullOrWhiteSpace(SortBy);
            }
        }

        public int SkipCount
        {
            get
            {
                return (Page - 1) * RowsPerPage;
            }
        }
    }
}
