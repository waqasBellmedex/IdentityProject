using Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ApplicationUser : IdentityUser<long>, ICreationAudited, IModificationAudited
    {
        public ApplicationUser()
        {
        }
        public string? FullName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [StringLength(450)]
        public string? ModifiedBy { get; set; }
    }
}
