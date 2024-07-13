using AutoMapper;
using Clinic.Application.DTOS;
using PasySkyTask.Application.DTOS;
using PaySkyTask.Domain.Entities;
using System.Net;
using System.Runtime.CompilerServices;

namespace PaySkyTask.API.Mapping;

public class MappingProfiles : Profile
{

    public MappingProfiles()
    {

        CreateMap<Vacancy, VacancyRequestDto>().ReverseMap();
        CreateMap<Vacancy, VacancyResponseDto>().ReverseMap();
        CreateMap<Applications, ApplicationRequestDto>().ReverseMap();
     
        CreateMap<ApplicationRequestDto, ApplicationResponseDto>().ReverseMap();
        CreateMap<User, RegisterRequestDto>().ReverseMap();
        CreateMap<LoginResponseDto, LoginRequestDto>().ReverseMap();
        CreateMap<User, LoginResponseDto>();
        CreateMap<RegisterRequestDto, User>();

        CreateMap<Applications,ApplicationResponseDto>()           
            .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => src.ApplicationDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

    }
}


