using App.Domain.Entities;
using App.Infrastructure.Dtos;

namespace App.Infrastructure.Transformers;

public static class UserTransformer
{
    public static UserDto ToDto(this UserEntity entity)
    {
        return new UserDto()
        {
            DateCreated = entity.DateCreated,
            Age = entity.Age,
            BirthDate = entity.BirthDate,
            Bmi = entity.Bmi,
            DateUpdated = entity.DateUpdated,
            Height = entity.Height,
            IsActive = entity.IsActive,
            Name = entity.Name,
            UserId = entity.UserId,
            Weight = entity.Weight
        };
    }
}
