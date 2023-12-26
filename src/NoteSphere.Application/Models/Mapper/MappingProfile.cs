using Application.Models.DTOs.User;
using Application.Models.Identity;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, UserAuth>();
            CreateMap<UserRegistrationDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
