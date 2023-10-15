﻿namespace Huamn_Capital_Managment.Common
{
    using AutoMapper;
    using Human_Capital_Managment.Data.Models2;
    using Human_Capital_Managment.ViewModels.AuthenticationViewModels;

    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {

            CreateMap<RegisterViewModel, Employee>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        }

    }
}
