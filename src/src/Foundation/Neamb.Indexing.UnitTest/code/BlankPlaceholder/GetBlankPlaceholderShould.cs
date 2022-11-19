using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Neambc.Neamb.Foundation.Indexing.UnitTest.BlankPlaceholder
{
    [TestFixture]
    public class GetBlankPlaceholderShould
    {
        [Test]
        public void Return_BlankPlaceholder_When_Called()
        {
            /*
             * THIS IS AN EMPTY TEST JUST TO ALLOW NUNIT TO COUNT TEST PROPERLY
             * DELETE THIS UPON CREATING FIRST REAL TEST IN THIS PROJECT
             */
            //Arrange
            //Act
            //Assert

            var template = Templates.PageTypeTemplates.Article.Guid;

            Assert.IsTrue(true);
        }
    }
}
