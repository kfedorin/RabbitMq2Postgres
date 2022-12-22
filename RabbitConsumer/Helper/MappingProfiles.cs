using AutoMapper;
using RabbitConsumer.Commands.OrganizationCommand;
using RabbitConsumer.Commands.UserCommand;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateOrganizationCommand, Organization>();
            CreateMap<UpdateOrganizationCommand, Organization>();
            CreateMap<DeleteOrganizationCommand, Organization>();

            CreateMap<CreateUserCommand, User>();
            CreateMap<UpdateUserCommand, User>();
            CreateMap<DeleteUserCommand, User>();
        }
    }
}
