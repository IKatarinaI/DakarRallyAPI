using AutoMapper;
using DakarRally.API.Validators;
using DakarRally.Core.DTO.ReadObjects;
using DakarRally.DTO.CreateObjects;
using DakarRally.DTO.ReadObjects;
using DakarRally.DTO.UpdateObjects;
using DakarRally.Models;
using System;

namespace DakarRally.API.AutoMapper
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, ReadVehicleObject>()
                .ForMember(dest => dest.Type,
                opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Class,
                opt => opt.MapFrom(src => src.Class.ToString()))
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreateVehicleObject, Vehicle>();
            CreateMap<UpdateVehicleObject, Vehicle>();
            CreateMap<Vehicle, UpdateVehicleObject>();
            CreateMap<Vehicle, VehicleValidator>()
                .ForMember(dest => dest.RaceStatus,
                opt => opt.MapFrom(src => src.Race.Status))
                .ForMember(dest => dest.RaceYear,
                opt => opt.MapFrom(src => src.Race.Year));
            CreateMap<UpdateVehicleObject, VehicleValidator>();
            CreateMap<Vehicle, ReadVehicleStatisticsObject>()
                .ForMember(dest => dest.MalfunctionStatistics,
                opt => opt.MapFrom(src => src.MalfunctionStatistics.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)))
                .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
