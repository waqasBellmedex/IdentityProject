using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(450)]
        public string? ModifiedBy { get; set; }
    }
}
