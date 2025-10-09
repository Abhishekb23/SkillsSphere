using AutoMapper;
using skillsphere.core.Dtos;
using skillsphere.core.Entities;

namespace skillsphere.infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO
            CreateMap<User, UserDto>();

            // DTO -> Entity
            CreateMap<UserDto, User>();

            // CreateUserDto -> User
            CreateMap<CreateUserDto, User>()
                // Map Password -> PasswordHash (you can hash later if needed)
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                // Set default CreatedAt/UpdatedAt automatically
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
