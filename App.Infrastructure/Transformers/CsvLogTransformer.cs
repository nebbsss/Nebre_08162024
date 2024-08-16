using App.Domain.Entities;
using App.Infrastructure.Dtos;

namespace App.Infrastructure.Transformers;

public static class CsvLogTransformer
{
    public static CsvLogDto ToDto(this CsvLogEntity entity)
    {
        return new CsvLogDto()
        {
            DateCreated = entity.DateCreated,
            FileName = entity.FileName,
            RecordsProcessed = entity.RecordsProcessed,
            TotalRecords = entity.TotalRecords,
            Duration = entity.Duration,
            DateUpdated = entity.DateUpdated,
            FileSize = entity.FileSize,
            IsActive = entity.IsActive
        };
    }
}
