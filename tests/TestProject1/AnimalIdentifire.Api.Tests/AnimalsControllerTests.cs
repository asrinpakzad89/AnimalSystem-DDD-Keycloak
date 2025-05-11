using AnimalIdentifier.API.Controllers;
using AnimalIdentifier.Application.Animals.ViewModels;
using AnimalIdentifier.Application.Commands;
using AnimalIdentifier.Application.Queries;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AnimalIdentifire.Api.Tests;

public class AnimalsControllerTests
{
    private readonly AnimalsController _controller;
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    public AnimalsControllerTests()
    {
        _controller = new AnimalsController(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Create_Should_Return_Ok_When_Command_Succeeds()
    {
        var command = new CreateAnimalCommand { Name = "Elephant" };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var result = await _controller.Create(command, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_Should_Return_Ok_When_Command_Succeeds()
    {
        var command = new UpdateAnimalCommand { Id = 1, Name = "Tiger" };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var result = await _controller.Update(command, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_Should_Return_Ok_When_Command_Succeeds()
    {
        var command = new DeleteAnimalCommand { Id = 1 };
        _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Unit.Value);

        var result = await _controller.Delete(command, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_Should_Return_Animal_When_Found()
    {
        var query = new GetAnimalByIdQuery { Id = 1 };
        var expectedAnimal = new AnimalDto { Id = 1, Name = "Lion" };
        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAnimal);

        var result = await _controller.GetById(query, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedAnimal);
    }

    [Fact]
    public async Task GetById_Should_Return_NotFound_When_Result_Is_Null()
    {
        var query = new GetAnimalByIdQuery { Id = 999 };
        _mediatorMock.Setup(m => m.Send(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AnimalDto?)null);

        var result = await _controller.GetById(query, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAll_Should_Return_List_Of_Animals()
    {
        var expectedList = new List<AnimalDto>
            {
                new AnimalDto { Id = 1, Name = "Lion" },
                new AnimalDto { Id = 2, Name = "Zebra" }
            };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllAnimalsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedList);

        var result = await _controller.GetAll(CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedList);
    }
}