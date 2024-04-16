using Dotnet.Templates.Infra.CrossCutting.Domain;
using System.ComponentModel.DataAnnotations;

namespace Dotnet.Template.Infra.CrossCutting.Domain
{
    public class EntityBase<TId> : IEntity
    {
        [Key]
        public TId Id { get; set; }
    }
}
