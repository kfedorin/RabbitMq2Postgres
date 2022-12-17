using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitConsumer.Repositories.Base;

public class EntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}