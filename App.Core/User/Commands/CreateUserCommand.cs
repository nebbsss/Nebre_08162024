using App.Core.Utils;
using App.Domain.Entities;
using App.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Core.User.Commands;

public class CreateUserCommand : IRequest<BaseResponse>
{
    public string? Name { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Age
    {
        get
        {
            if (!BirthDate.HasValue) return null;

            var today = DateTime.Today;
            var age = today.Year - BirthDate.Value.Year;

            return age > 0 ? age : 0;
        }
    }
    public double? Bmi
    {
        get
        {
            if (!Weight.HasValue || !Height.HasValue) return null;
            return Weight.Value / Math.Pow(Height.Value / 100.0, 2);
        }
    }
}

public class CreateUserCommandHandler : BaseCommandHandler, IRequestHandler<CreateUserCommand, BaseResponse>
{
    private readonly IRepository<UserEntity> _userRepository;
    public CreateUserCommandHandler(
        IRepository<UserEntity> userRepository,
        ILogger<CreateUserCommandHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(userRepository));
        _userRepository = userRepository;
    }

    public async Task<BaseResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse();
        UserEntity? user = null;

        try
        {
            user = new UserEntity()
            {
                Age = request.Age,
                BirthDate = request.BirthDate,
                Bmi = request.Bmi,
                Height = request.Height,
                Name = request.Name,
                Weight = request.Weight,
                DateCreated = DateTime.UtcNow
            };

            user = await _userRepository.Create(user, cancellationToken).ConfigureAwait(false);
            LogInformation("Create user success");

            response.Success = true;
            response.Message = "User successfully created!";
        }
        catch (Exception ex)
        {
            LogError(ex, "Error create user");

            response.Success = false;
            response.Message = "Error! Try again.";
        }

        return response;
    }
}
