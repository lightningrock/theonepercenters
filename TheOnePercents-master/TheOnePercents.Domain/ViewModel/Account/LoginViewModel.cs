using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOnePercents.Domain.ViewModel
{
    public class LoginViewModel
    {
        public Guid CompanyID { get; set; }
        public string CompanyName { get; set; }
        public Guid CompetitionID { get; set; }
        public string CompetitionName { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
