using RabbitConsumer.Repositories.Base;

namespace RabbitConsumer.Repositories.Models
{
    public class Organization : EntityBase
    {
        public Organization()
        {
            Users = new HashSet<User>();
        }

        public string Name { get; set; }



        public virtual ICollection<User>? Users { get; set; }
    }
}
