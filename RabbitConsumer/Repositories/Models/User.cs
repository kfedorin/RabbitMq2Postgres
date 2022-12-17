﻿using RabbitConsumer.Repositories.Base;

namespace RabbitConsumer.Repositories.Models
{
    public class User : EntityBase
    {
        public int IdOrganization { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleNane { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public virtual Organization? Organization { get; set; }
    }
}
