using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RabbitConsumer.Commands.OrganizationCommand;
using RabbitConsumer.Controllers;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Test.ControllersTests
{
    public class OrganizationControllerTests
    {
        private readonly IMediator _mediator;
        private readonly IValidator<UpdateOrganizationCommand> _validator;

        public OrganizationControllerTests()
        {
            _mediator = A.Fake<IMediator>();
            _validator = A.Fake<IValidator<UpdateOrganizationCommand>>();
        }

        [Fact]
        public void OrganizationController_GetOrganizations_ReturnOK()
        {
            //Arrange
            var controller = new OrganizationController(_mediator, _validator);

            //Act
            var result = controller.GetAll();

            //Assert
            result.Should().NotBeNull();
            //result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}
