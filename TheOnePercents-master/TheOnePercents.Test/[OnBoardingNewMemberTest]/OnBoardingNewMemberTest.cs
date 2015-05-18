using System;
using TheOnePercents.Domain;
using TheOnePercents.Domain.ViewModel.OnBoardNewMember;
using TheOnePercents.Infrastructure;
using TheOnePercents.Model;
using TheOnePercents.Repository;
using TheOnePercents.Repository.Infrastructure;
using Xunit;
using System.Linq;

namespace TheOnePercents.Test._OnBoardingNewMemberTest_
{
    public class OnBoardingNewMemberTest
    {
        private readonly IRepositoryBase<theonepercentersEntities1> _repositoryBase;
        private readonly IOnboardingNewMember _onboardingNewMember;

        public OnBoardingNewMemberTest()
        {
            _repositoryBase = RepositoryDI.Resolve<IRepositoryBase<theonepercentersEntities1>>();
            _onboardingNewMember = RepositoryDI.Resolve<IOnboardingNewMember>();
        }

        [Fact]
        public void Should_Set_Up_A_New_Member_In_The_Correct_Competition() {
            OperationStatus opStatus = new OperationStatus();

            Company company = _repositoryBase.AllIncludingNonTracking<Company>(x => x.Competitions).Where(x => x.CompanyName == "Chad MacKay").FirstOrDefault();

            CreateNewOnBoardMemberRequest request = new CreateNewOnBoardMemberRequest();
            request.CompanyID = company.CompanyID;
            request.CompetitionID = company.Competitions.FirstOrDefault().CompetitionID;
            request.FirstName = "Colin";
            request.LastName = "Byrne";
            request.EmailAddress = "colinbyrne@tboae.com";

            opStatus = _onboardingNewMember.SaveNewMerchantDetails(request);
            
        }
    }
}
