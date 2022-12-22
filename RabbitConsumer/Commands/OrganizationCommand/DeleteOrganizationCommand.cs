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

    }

    public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand, bool>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        public DeleteOrganizationCommandHandler(IDbContext dbContext, IMapper mapper) : base()
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<bool> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {

            var entity = _mapper.Map<Organization>(request);

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
