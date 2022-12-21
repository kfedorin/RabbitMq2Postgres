using RabbitConsumer.Repositories.Base;

namespace RabbitConsumer.Repositories.Models
{
    public class User : EntityBase
    {
        public int IdOrganization { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
