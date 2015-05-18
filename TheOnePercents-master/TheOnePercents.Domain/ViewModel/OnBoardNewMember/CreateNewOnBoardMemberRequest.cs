using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOnePercents.Domain.ViewModel.OnBoardNewMember
{
    public class CreateNewOnBoardMemberRequest
    {
        public Guid CompanyID { get; set; }
        public Guid CompetitionID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }
}
