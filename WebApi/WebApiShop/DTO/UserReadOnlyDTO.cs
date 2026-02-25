namespace DTO
{
    public record UserReadOnlyDTO
    (
        int UserId,
        string UserName,
        string FirstName,
        string LastName,
        bool IsAdmin
    );
}
