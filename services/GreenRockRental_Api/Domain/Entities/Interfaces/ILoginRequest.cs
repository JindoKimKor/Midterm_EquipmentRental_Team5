namespace GreenRockRental_Api.Domain.Interfaces
{
    public interface ILoginRequest
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}