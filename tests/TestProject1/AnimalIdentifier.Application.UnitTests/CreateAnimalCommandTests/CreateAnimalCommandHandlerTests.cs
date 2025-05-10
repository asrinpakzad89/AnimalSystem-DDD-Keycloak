using AnimalIdentifier.Application.Commands;
using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using AnimalIdentifier.Domain.Events;
using AnimalIdentifier.Domain.Seedwork;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace AnimalIdentifier.Application.UnitTests.CreateAnimalCommandTests;

public class CreateAnimalCommandHandlerTests
{
    private readonly Mock<IAnimalRepository> _repositoryMock = new();
    private readonly Mock<IValidator<CreateAnimalCommand>> _validatorMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly CreateAnimalCommandHandler _handler;
    public CreateAnimalCommandHandlerTests()
    {
        _handler = new CreateAnimalCommandHandler(_repositoryMock.Object, _validatorMock.Object, _mediatorMock.Object);
    }
    [Fact]
    public async Task Handle_Should_Create_Animal_And_Publish_Event_If_Valid()
    {
        // Arrange
        var command = new CreateAnimalCommand { Name = "  Lion " };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(r => r.UnitOfWork)
                      .Returns(_unitOfWorkMock.Object);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _validatorMock.Verify(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);

        _repositoryMock.Verify(r => r.Add(It.Is<Animal>(a => a.Name == "Lion")), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);

        _mediatorMock.Verify(m => m.Publish(It.Is<AnimalCreatedDomainEvent>(e =>
            e.AnimalName == "Lion"), It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_Should_Throw_ValidationException_If_Invalid()
    {
        // Arrange
        var command = new CreateAnimalCommand { Name = "" };

        var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            };

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new ValidationResult(failures));

        var handler = new CreateAnimalCommandHandler(_repositoryMock.Object, _validatorMock.Object, _mediatorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

        // اطمینان از عدم ادامه روند در صورت نامعتبر بودن داده‌ها
        _repositoryMock.Verify(r => r.Add(It.IsAny<Animal>()), Times.Never);
        _mediatorMock.Verify(m => m.Publish(It.IsAny<AnimalCreatedDomainEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}