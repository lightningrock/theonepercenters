using TheOnePercents.Infrastructure;
using TheOnePercents.Model;
using TheOnePercents.Repository;
using Xunit;

namespace TheOnePercents.Test._Login_
{
    public class LoginTest
    {
        private readonly IRepositoryBase<theonepercentersEntities1> _repositoryBase;

        public LoginTest() {
            _repositoryBase = RepositoryDI.Resolve<IRepositoryBase<theonepercentersEntities1>>();
        }

        [Fact]
        public void Should_Connect_To_Database() {
            var test = _repositoryBase.GetList<Company>();
        }
    }
}
