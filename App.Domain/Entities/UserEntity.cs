namespace App.Domain.Entities;

public class UserEntity : BaseEntity, IEntity
{
    public object? Id { get => UserId; set => UserId = value != null ? (Guid)value : Guid.Empty; }
    public Guid UserId { get; set; }
    public string? Name { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Age { get; set; }
    public double? Bmi { get; set; }
}
