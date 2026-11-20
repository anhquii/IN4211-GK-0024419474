using EduHub.Domain.Entities;

namespace EduHub.Tests.Domain;

public class RegistrationTests
{
    [Fact]
    public void Create_WithValidIds_ShouldInitializePendingStatus()
    {
        var memberId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        var r = Registration.Create(memberId, eventId);

        Assert.NotEqual(Guid.Empty, r.Id);
        Assert.Equal(memberId, r.MemberId);
        Assert.Equal(eventId, r.EventId);
        Assert.Equal(RegistrationStatus.Pending, r.Status);
    }

    [Fact]
    public void Create_WithEmptyGuid_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Registration.Create(Guid.Empty, Guid.NewGuid()));
        Assert.Throws<ArgumentException>(() => Registration.Create(Guid.NewGuid(), Guid.Empty));
    }

    [Fact]
    public void Confirm_WhenPending_ShouldChangeStatusToConfirmed()
    {
        var r = Registration.Create(Guid.NewGuid(), Guid.NewGuid());

        r.Confirm();

        Assert.Equal(RegistrationStatus.Confirmed, r.Status);
    }

    [Fact]
    public void Confirm_WhenCancelled_ShouldThrowInvalidOperationExceptionAndKeepCancelledStatus()
    {
        var memberId = Guid.NewGuid();
        var eventId = Guid.NewGuid();
        var r = Registration.Create(memberId, eventId);
        r.Cancel();

        Assert.Throws<InvalidOperationException>(() => r.Confirm());
        Assert.Equal(RegistrationStatus.Cancelled, r.Status);
    }
}