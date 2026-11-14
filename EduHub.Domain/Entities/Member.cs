namespace EduHub.Domain.Entities;

public class Member
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;

    private Member() { }

    public static Member Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        if (!email.Contains('@'))
            throw new ArgumentException("Email must contain '@'.", nameof(email));

        return new Member
        {
            Id = Guid.NewGuid(),
            Email = email.Trim()
        };
    }
}