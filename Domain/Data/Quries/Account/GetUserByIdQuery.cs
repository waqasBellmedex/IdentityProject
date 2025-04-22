using Domain.Model;
using Domain.Services.Account.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Quries.Account
{
    public class GetUserByIdQuery : IRequest<ApplicationUser>
    {
        public long Id { get; set; }
    }
}
