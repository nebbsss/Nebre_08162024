namespace App.Infrastructure.Dtos;

public class CsvLogDto : BaseDto
{
    public string? FileName { get; set; }
    public int? RecordsProcessed { get; set; }
    public double? Duration { get; set; }
    public double FileSize { get; set; }
}
