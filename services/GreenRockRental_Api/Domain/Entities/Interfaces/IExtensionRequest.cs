namespace GreenRockRental_Api.Domain.Interfaces
{
    public interface IExtensionRequest
    {
        DateTime NewDueDate { get; set; }
        string Reason { get; set; }
    }
}