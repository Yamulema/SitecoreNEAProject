using Moq;
using Neambc.Neamb.Foundation.Cache.Managers;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Cache.UnitTest.BlankPlaceholder
{
    [TestFixture]
    public class GetBlankPlaceholderShould
    {
        [Test]
        public void Return_BlankPlaceholder_When_Called()
        {
            /*
             * THIS IS AN EMPTY TEST JUST TO ALLOW SITECORE TO COUNT TEST PROPERLY
             * DELETE THIS UPON CREATING FIRST REAL TEST IN THIS PROJECT
             */
            //Arrange
            //Act
            //Assert

            var cache = new Mock<ICacheManager>();

            Assert.IsTrue(true);
        }
    }
}
