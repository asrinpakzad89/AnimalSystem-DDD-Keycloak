using AnimalIdentifier.Application.Commands;
using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using AnimalIdentifier.Domain.Seedwork;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace AnimalIdentifier.Application.UnitTests.DeleteAnimalCommandTests;

public class DeleteAnimalCommandHandlerTests
{
    private readonly Mock<IAnimalRepository> _repositoryMock = new();
    private readonly Mock<IValidator<DeleteAnimalCommand>> _validatorMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly DeleteAnimalCommandHandler _handler;
    public DeleteAnimalCommandHandlerTests()
    {
        _handler = new DeleteAnimalCommandHandler(_repositoryMock.Object, _validatorMock.Object);
    }
    [Fact]
    public async Task Handle_Should_Delete_Animal_If_Valid()
    {
        var command = new DeleteAnimalCommand { Id = 99 };
        var animal = new Animal("Tiger");

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetAsync(command.Id, It.IsAny<CancellationToken>())).ReturnsAsync(animal);
        _repositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _validatorMock.Verify(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.GetAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.Delete(animal), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(Unit.Value, result);
    }
}
