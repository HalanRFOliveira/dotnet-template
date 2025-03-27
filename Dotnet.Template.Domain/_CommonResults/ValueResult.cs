namespace Dotnet.Template.Domain
{
	/// <summary>
	/// Representa um único valor, normalmente numerico
	/// </summary>
	/// <typeparam name="T">Tipo do valor, normalmente int, long, enum</typeparam>
	public class ValueResult<T>(T value)
    {
        public T Value { get; } = value;
    }
}
