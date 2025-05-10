using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using AnimalIdentifier.Domain.Tests.Animals.Builders;
using FluentAssertions;
using Xunit;

namespace AnimalIdentifier.Domain.Tests.Animals;

public class AnimalTests
{
    [Fact]
    public void Constructor_Should_Set_Name_Correctly()
    {
        //var animal = new AnimalBuilder().WithName("Tiger").Build();

        //animal.Name.Should().Be("Tiger");
        string name = "Dog";
        Animal animal = new Animal(name);
        Assert.Equal(name, animal.Name);
    }
    [Fact]
    public void Constructor_Should_Throw_Exception_When_Name_Is_Empty()
    {
        string name = "";
        Animal animal = new Animal(name);
        Assert.Throws<ArgumentException>(() => animal.Name);
    }
    [Fact]
    public void UpdateName_Should_Change_Name()
    {
        Animal animal = new Animal("Cat");
        animal.UpdateName("Cat 1");
        Assert.Equal("Cat 1", animal.Name);
    }
    [Fact]
    public void UpdateName_Should_Throw_Exception_When_Name_Is_Null()
    {
        Animal animal = new Animal("Cat");
        Assert.Throws<ArgumentNullException>(() => animal.UpdateName(null));
    }

    //[Theory]
    //[InlineData(null)]
    //[InlineData("")]
    //public void Create_InvalidName_ThrowsException(string invalidName)
    //{
    //    var act = () => new AnimalBuilder().WithName(invalidName).Build();

    //    act.Should().Throw<ArgumentException>()
    //       .WithMessage("Animal name cannot be empty.*");
    //}

    //[Fact]
    //public void UpdateName_WithValidName_UpdatesCorrectly()
    //{
    //    var animal = new AnimalBuilder().WithName("Old").Build();

    //    animal.UpdateName("New");

    //    animal.Name.Should().Be("New");
    //}

    //[Theory]
    //[InlineData(null)]
    //[InlineData("")]
    //public void UpdateName_WithInvalidName_ThrowsException(string name)
    //{
    //    var animal = new AnimalBuilder().WithName("Old").Build();

    //    var act = () => animal.UpdateName(name);

    //    act.Should().Throw<ArgumentException>()
    //       .WithMessage("Animal name cannot be empty.*");
    //}
}
