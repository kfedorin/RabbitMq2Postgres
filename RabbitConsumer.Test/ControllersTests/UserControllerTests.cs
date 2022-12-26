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
using RabbitConsumer.Commands.UserCommand;
using RabbitConsumer.Controllers;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Test.ControllersTests
{
    public class UserControllerTests
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateUserCommand> _validatorCreate;
        private readonly IValidator<UpdateUserCommand> _validatorUpdate;

        public UserControllerTests()
        {
            _mediator = A.Fake<IMediator>();
            _validatorUpdate = A.Fake<IValidator<UpdateUserCommand>>();
            _validatorCreate = A.Fake<IValidator<CreateUserCommand>>();
        }

        [Fact]
        public void UserController_GetUsers_ReturnOK()
        {
            //Arrange
            var controller = new UserController(_mediator, _validatorCreate, _validatorUpdate);

            //Act
            var result = controller.GetAll();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void UserController_CreateUser_ReturnOK()
        {
            //Arrange
            var userCreate = A.Fake<CreateUserCommand>();
            var controller = new UserController(_mediator, _validatorCreate, _validatorUpdate);

            //Act
            var result = controller.Create(userCreate);

            //Assert
            result.Should().NotBeNull();
        }
    }


}
