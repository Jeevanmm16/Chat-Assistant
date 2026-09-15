# DTO Mapping Rules (AutoMapper)

The application uses **AutoMapper** to handle all mapping between Entities and DTOs.

## General Rules
1. **Never return Entities from the API**: All data returned from API Controllers must be mapped to a DTO.
2. **Never accept Entities in API Requests**: All `[FromBody]` payloads must be DTOs, which are then mapped to Entities inside the Service layer.

## AutoMapper Profile Location
All mapping configurations must be defined in AutoMapper `Profile` classes.
- Profiles belong in the `Service` layer.
- Name profiles based on the feature/entity (e.g., `UserProfile.cs`).

## Example Profile
```csharp
using AutoMapper;
using <ProjectName>Core.Entities.Models;
using <ProjectName>Core.DTO.Models;

namespace <ProjectName>Core.Service.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Entity to DTO
        CreateMap<User, UserDto>();
        
        // DTO to Entity (for creates/updates)
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
```

## Using IMapper in Services
Inject `IMapper` into your Service classes to perform the mapping.

```csharp
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user); // Map Entity -> DTO before returning
    }
}
```
