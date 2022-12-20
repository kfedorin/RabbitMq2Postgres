using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Commands.OrganizationCommand
{
    public class DeleteOrganizationCommand : IRequest<bool>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdOrganization { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MiddleNane { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
    }

    public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand, bool>
    {
        private readonly IDbContext _dbContext;
        public DeleteOrganizationCommandHandler(IDbContext dbContext) : base()
        {
            _dbContext = dbContext;
        }


        public async Task<bool> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DeleteOrganizationCommand, Organization>();
            }).CreateMapper();

            var entity = mapper.Map<Organization>(request);

            _dbContext.Set<Organization>().Remove(entity);

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                return false;
            }
            
            return true;
        }
    }
}
