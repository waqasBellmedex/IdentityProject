using Domain.Data.Quries.Account;
using Domain.Interface;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, ApplicationUser>
    {
        private readonly IAccountService _accountService;

        public GetUserByIdHandler(IAccountService accountService) 
        {
            _accountService = accountService;
        }

        public async Task<ApplicationUser> Handle(GetUserByIdQuery request,CancellationToken cancellationToken)
        {
            return await _accountService.Get(request.Id);
        }   
    }
}
