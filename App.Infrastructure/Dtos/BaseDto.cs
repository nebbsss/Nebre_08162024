namespace App.Infrastructure.Dtos;

public class BaseDto
{
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool IsActive { get; set; }
}
