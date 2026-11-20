using EduHub.Application.Interfaces;
using EduHub.Application.Services;
using EduHub.Domain.Entities;
using EduHub.Domain.Interfaces;
using Moq;

namespace EduHub.Tests.Application;

public class EventServiceTests
{
    [Fact]
    public void CreateEvent_WithValidInput_ShouldCallAddAndSendOnce()
    {
        var clubId = Guid.NewGuid();
        var eventDate = new DateTime(2026, 12, 1, 9, 0, 0, DateTimeKind.Utc);
        var repositoryMock = new Mock<IEventRepository>();
        var notificationMock = new Mock<INotificationService>();
        var factoryMock = new Mock<INotificationServiceFactory>();
        factoryMock.Setup(f => f.Create("email")).Returns(notificationMock.Object);

        var service = new EventService(repositoryMock.Object, factoryMock.Object);

        var createdEvent = service.CreateEvent(clubId, "OOP Workshop", eventDate, 50, "email");

        Assert.NotEqual(Guid.Empty, createdEvent.Id);
        Assert.Equal(clubId, createdEvent.ClubId);
        Assert.Equal("OOP Workshop", createdEvent.Title);
        Assert.Equal(50, createdEvent.Capacity);

        repositoryMock.Verify(
            r => r.Add(It.Is<Event>(e => e.ClubId == clubId && e.Title == "OOP Workshop" && e.Capacity == 50)),
            Times.Once);
        notificationMock.Verify(
            n => n.Send(It.Is<string>(msg => msg.Contains("OOP Workshop"))),
            Times.Once);
    }

    [Fact]
    public void CreateEvent_WithInvalidInput_ShouldThrowAndNotCallAddOrSend()
    {
        var clubId = Guid.NewGuid();
        var eventDate = DateTime.UtcNow.AddDays(5);
        var repositoryMock = new Mock<IEventRepository>();
        var notificationMock = new Mock<INotificationService>();
        var factoryMock = new Mock<INotificationServiceFactory>();
        factoryMock.Setup(f => f.Create(It.IsAny<string>())).Returns(notificationMock.Object);

        var service = new EventService(repositoryMock.Object, factoryMock.Object);

        Assert.Throws<ArgumentException>(() => service.CreateEvent(clubId, "   ", eventDate, 20, "email"));
        Assert.Throws<ArgumentException>(() => service.CreateEvent(clubId, "Seminar", eventDate, 0, "email"));

        repositoryMock.Verify(r => r.Add(It.IsAny<Event>()), Times.Never);
        notificationMock.Verify(n => n.Send(It.IsAny<string>()), Times.Never);
    }
}