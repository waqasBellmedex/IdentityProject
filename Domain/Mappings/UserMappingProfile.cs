using AutoMapper;
using Domain.Model;
using Domain.Services.Account.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, RegistrationRequest>().ReverseMap();
        }
    }
}
