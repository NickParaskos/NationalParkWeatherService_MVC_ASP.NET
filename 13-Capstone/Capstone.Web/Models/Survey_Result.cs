using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class Survey_Result
    {
        public int Id { get; set; }

        [Required]
        public string ParkCode { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [StringLength(2)]
        [MinLength(2)]
        [Required]
        public string State { get; set; }

        [Required]
        public string ActivityLevel { get; set; }
    }
}
