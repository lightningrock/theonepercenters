using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TheOnePercents.Domain.ViewModel.OnBoardNewMember;
using TheOnePercents.Model;
using TheOnePercents.Repository;
using TheOnePercents.Repository.Infrastructure;

namespace TheOnePercents.Domain
{
    /// <summary>
    /// Onboard a new member tp the correct competition.
    /// Add to Memberships. Users and Contacts
    /// </summary>
    public class OnboardingNewMember : IOnboardingNewMember
    {
        private readonly IRepositoryBase<theonepercentersEntities1> _repositoryBase;

        public OnboardingNewMember(IRepositoryBase<theonepercentersEntities1> repositoryBase) { 
            _repositoryBase = repositoryBase;
        }

        public OperationStatus SaveNewMerchantDetails(CreateNewOnBoardMemberRequest createNewOnBoardMemberRequest)
        {
            OperationStatus opStatus = new OperationStatus() { Status = true, Message = "New Member Onboarding Complete" };

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    opStatus = CreateNewOnBoardMember(createNewOnBoardMemberRequest);
                    if (opStatus.Status)
                    {
                        scope.Complete(); // All Good -Complete the transaction.
                    }
                    else
                    {
                        scope.Dispose(); // Rollback
                    }
                }
                return opStatus;
            }
            catch (Exception ex)
            {
                opStatus.Status = false;
                if (opStatus.OriginatingException == string.Empty || opStatus.OriginatingException == null || opStatus.OriginatingException == null || opStatus.OriginatingException == null)
                {
                    opStatus = OperationStatus.CreateFromException("Save New Merchant Details Failed", "MerchantOnBoardingOrchestration.SaveNewMerchantDetails", ex);
                }
                throw;
            }
        }



        public OperationStatus CreateNewOnBoardMember(CreateNewOnBoardMemberRequest createNewOnBoardMemberRequest)
        {
            OperationStatus opStatus = new OperationStatus(){Status = true, Message = "New Member Onboarding Complete"};
            
            // Contact
            Contact contact = new Contact();
            contact.ContactID = Guid.NewGuid();
            contact.CompanyID = createNewOnBoardMemberRequest.CompanyID;
            contact.FirstName = createNewOnBoardMemberRequest.FirstName;
            contact.LastName = createNewOnBoardMemberRequest.LastName;
            contact.CreateDate = DateTime.Now;
            contact.CreateUser = "tops";
            contact.UpdateDate = DateTime.Now;
            contact.UpdateUser = "tops";

            opStatus = _repositoryBase.Save<Contact>(contact, SaveOption.New);
            
            // Check that all was ok.
            if (!opStatus.Status) return opStatus;

            // Principal User
            PrincipalUser principalUser = new PrincipalUser();
            principalUser.PrincipalUserID = Guid.NewGuid();
            principalUser.CompanyID = createNewOnBoardMemberRequest.CompanyID;
            principalUser.ContactID = contact.ContactID;
            principalUser.Active = true;
            principalUser.UserName = createNewOnBoardMemberRequest.FirstName + createNewOnBoardMemberRequest.LastName;
            principalUser.LoweredUserName = principalUser.UserName.ToLower();
            principalUser.CreateDate = DateTime.Now;
            principalUser.CreateUser = "tops";
            principalUser.UpdateDate = DateTime.Now;
            principalUser.UpdateUser = "tops";

            opStatus = _repositoryBase.Save<PrincipalUser>(principalUser, SaveOption.New);

            if (!opStatus.Status) return opStatus;

            // Membership
            Membership membership = new Membership();
            membership.MembershipID = Guid.NewGuid();
            membership.PrincipalUserID = principalUser.PrincipalUserID;
            membership.CompanyID = createNewOnBoardMemberRequest.CompanyID;
            membership.Password = "";
            membership.PasswordFormat = 1; // Not sure what this is?
            membership.PasswordSalt = "";
            membership.PasswordSetIdentifier = Guid.NewGuid(); // This is so that we can identify the email that is sent. 
            membership.MobilePin = null;
            membership.Email = createNewOnBoardMemberRequest.EmailAddress;
            membership.LoweredEmail = createNewOnBoardMemberRequest.EmailAddress.ToLower();
            membership.PasswordQuestion = "";
            membership.PasswordAnswer = "";
            membership.IsApproved = false;
            membership.IsLockedOut = false;
            membership.IsOnHold =false;
            membership.LastLoginDate = null;
            membership.LastLockoutDate = null;
            membership.LastPasswordChangedDate = null;
            membership.FailedPasswordAnswerAttemptCount = 0;
            membership.FailedPasswordAnswerAttemptWindowStart = null;
            membership.FailedPasswordAttemptCount = 0;
            membership.FailedPasswordAttemptWindowStart = null;
            membership.Comment = "";
            membership.CreateDate = DateTime.Now;
            membership.CreateUser = "tops";
            membership.UpdateDate = DateTime.Now;
            membership.UpdateUser = "tops";

            opStatus = _repositoryBase.Save<Membership>(membership, SaveOption.New);

            if (!opStatus.Status) return opStatus;

            // User Competiton
            UserCompetition userCompetition = new UserCompetition();
            userCompetition.UserCompetitionID = Guid.NewGuid();
            userCompetition.PrincipalUserID = principalUser.PrincipalUserID;
            userCompetition.CompanyID = createNewOnBoardMemberRequest.CompanyID;
            userCompetition.CompetitionID = createNewOnBoardMemberRequest.CompetitionID;
            userCompetition.CreateDate = DateTime.Now;
            userCompetition.CreateUser = "tops";
            userCompetition.UpdateDate = DateTime.Now;
            userCompetition.UpdateUser = "tops";

            opStatus = _repositoryBase.Save<UserCompetition>(userCompetition, SaveOption.New);

            if (!opStatus.Status) return opStatus;

            // Send Email.

            return opStatus;
        }
    }

    public interface IOnboardingNewMember {
        OperationStatus SaveNewMerchantDetails(CreateNewOnBoardMemberRequest createNewOnBoardMemberRequest);
    }
}
