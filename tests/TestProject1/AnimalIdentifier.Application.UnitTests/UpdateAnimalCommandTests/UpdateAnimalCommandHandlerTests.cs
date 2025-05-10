using AnimalIdentifier.Application.Commands;
using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace AnimalIdentifier.Application.UnitTests.UpdateAnimalCommandTests;

public class UpdateAnimalCommandHandlerTests
{
    private readonly Mock<IAnimalRepository> _repositoryMock = new();
    private readonly Mock<IValidator<UpdateAnimalCommand>> _validatorMock = new();

    private readonly UpdateAnimalCommandHandler _handler;

    public UpdateAnimalCommandHandlerTests()
    {
        _handler = new UpdateAnimalCommandHandler(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_Animal_When_Valid()
    {
        // Arrange
        var command = new UpdateAnimalCommand { Id = 1, Name = "Updated" };
        var existingAnimal = new Animal("OldName");

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetAsync(command.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingAnimal);

        _repositoryMock.Setup(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .Returns((Task<int>)Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Updated", existingAnimal.Name);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Validation_Fails()
    {
        var command = new UpdateAnimalCommand { Id = 1, Name = "" };
        var validationResult = new ValidationResult(new[] {
            new ValidationFailure("Name", "Name is required")
        });

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(validationResult);

        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Animal_Not_Found()
    {
        var command = new UpdateAnimalCommand { Id = 99, Name = "New" };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.GetAsync(command.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Animal)null);

        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None)
        );
    }
}
