using App.Core.Utils;
using App.Domain.Entities;
using App.Infrastructure.Dtos;
using App.Infrastructure.Repositories;
using App.Infrastructure.Transformers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace App.Core.User.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
{

}

public class GetAllUsersQueryHandler : BaseCommandHandler, IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IRepository<UserEntity> _userRepository;
    public GetAllUsersQueryHandler(IRepository<UserEntity> userRepository, ILogger<GetAllUsersQueryHandler> logger) : base(logger)
    {
        ArgumentNullException.ThrowIfNull(nameof(userRepository));
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return (await _userRepository.GetAllAsync(cancellationToken)).Select(u => u.ToDto());
    }
}

