namespace App.Domain.Entities;

public class CsvLogEntity : BaseEntity, IEntity
{
    public object? Id { get => CsvLogId; set => CsvLogId = value != null ? (Guid)value : Guid.Empty; }
    public Guid CsvLogId { get; set; }
    public string? FileName { get; set; }
    public int? RecordsProcessed { get; set; }
    public int? TotalRecords { get; set; }
    public double? Duration { get; set; }
    public double FileSize { get; set; }
}
