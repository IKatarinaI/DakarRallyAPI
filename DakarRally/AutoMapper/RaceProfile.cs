using AutoMapper;
using DakarRally.API.Validators;
using DakarRally.DTO.CreateObjects;
using DakarRally.DTO.ReadObjects;
using DakarRally.DTO.UpdateObjects;
using DakarRally.Models;

namespace DakarRally.API.AutoMapper
{
    public class RaceProfile : Profile
    {
        public RaceProfile()
        {
            CreateMap<Race, ReadRaceObject>()
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString())); ;
            CreateMap<CreateRaceObject, Race>();
            CreateMap<UpdateRaceObject, Race>();
            CreateMap<Race, UpdateRaceObject>();
            CreateMap<UpdateRaceObject, RaceValidator>();
            CreateMap<Race, LeaderboardValidator>();
        }
    }
}
