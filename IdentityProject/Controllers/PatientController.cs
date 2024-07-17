using Domain.Model;
using Domain.Services.Account.Dto;
using Domain.Services.Patient.Dto;
using Microsoft.AspNetCore.Mvc;

namespace IdentityProject.Controllers
{
    public class PatientController : BaseController<PatientController>
    {
        public PatientController()
        {

        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<Patient>> Get(GetPatientDto request)
        {
            //var result = await _accountService.Get(request);
            return new Patient { };
        }
    }
}
