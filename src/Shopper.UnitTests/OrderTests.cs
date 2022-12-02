using FluentAssertions;
using Shopper.Domain;

namespace Shopper.UnitTests;

public class OrderTests
{
    [Fact]
    public void Order_WhenCreated_ShouldBePending()
    {
        // Arrange
        Order order = new Order();

        // Act
        var result = order.Status;

        // Assert
        result.Should().Be(OrderStatus.Pending);

    }

    [Theory]
    [InlineData(OrderStatus.Pending, OrderStatus.Completation)]
    [InlineData(OrderStatus.Completation, OrderStatus.Sent)]
    [InlineData(OrderStatus.Sent, OrderStatus.Delivered)]
    [InlineData(OrderStatus.Delivered, OrderStatus.Completed)]    
    public void Confirm_WhenSourceStatus_ShouldBeStatus(OrderStatus sourceStatus, OrderStatus expected)
    {
        // Arrange        
        Order order = new Order(sourceStatus);

        // Act
        order.Confirm();

        // Assert
        order.Status.Should().Be(expected);
    }

    [Theory]
    [InlineData(OrderStatus.Pending, OrderStatus.Cancelled)]
    [InlineData(OrderStatus.Delivered, OrderStatus.Cancelled)]
    public void Cancel_WhenSourceStatus_ShouldBeStatus(OrderStatus sourceStatus, OrderStatus expected)
    {
        // Arrange        
        Order order = new Order(sourceStatus);

        // Act
        order.Cancel();

        // Assert
        order.Status.Should().Be(expected);
    }

    [Fact]
    public void Cancel_WhenSentStatus_ShouldThrowInvalidOperationException()
    {
        // Arrange        
        Order order = new Order(OrderStatus.Sent);

        // Act
        var act = () => order.Cancel();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Cancel_WhenCompletedStatus_ShouldThrowInvalidOperationException()
    {
        // Arrange        
        Order order = new Order(OrderStatus.Completed);

        // Act
        var act = () => order.Cancel();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }



    [Theory]
    [InlineData(OrderStatus.Pending, true)]
    [InlineData(OrderStatus.Completation, true)]
    [InlineData(OrderStatus.Sent, true)]
    [InlineData(OrderStatus.Delivered, true)]
    [InlineData(OrderStatus.Completed, false)]
    public void CanConfirm_WhenStatus_ShouldBe(OrderStatus sourceStatus, bool expected)
    {
        // Arrange        
        Order order = new Order(sourceStatus);

        // Act
        var result = order.CanConfirm;

        // Assert
        result.Should().Be(expected);
    }
}