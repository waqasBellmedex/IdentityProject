using AutoMapper;
using Domain.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modules
{
    public static class ApplicationModule
    {
        public static Profile UserMappingProfile()
        {
            return new UserMappingProfile();
        }
    }
}
