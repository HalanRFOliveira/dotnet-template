namespace Dotnet.Template.Infra.Paging
{
    /// <summary>
    /// Inicia uma nova instância da classe <see cref="PagedResult{T}"/>.
    /// </summary>
    /// <param name="totalSize">O total de registros.</param>
    /// <param name="pageSize">O total de registros por página.</param>
    /// <param name="data">O resultado da consulta.</param>
    public class PagedResult<T>(long totalSize, IEnumerable<T> data)
      where T : class
    {

        /// <summary>
        /// Obtém ou define o total de registros.
        /// </summary>
        public long TotalSize { get; private set; } = totalSize;

        /// <summary>
        /// Obtém ou define o resultado da consulta.
        /// </summary>
        public IEnumerable<T> Data { get; private set; } = data;

    }
}
