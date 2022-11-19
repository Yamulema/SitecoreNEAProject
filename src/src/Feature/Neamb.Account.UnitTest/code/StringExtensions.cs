using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Bson;
using NUnit.Framework;

namespace Neambc.Neamb.Feature.Account.UnitTest
{
    [TestFixture]
    public class StringExtensions
    {
        [Test]
        public void AsFormattedPhoneNumber_Should_Return_EmptyString_When_StringIsNull() {
            //Arrange
            string nullString = null;

            //Act
            var result = nullString.AsFormattedPhoneNumber();

            //Assert
            Assert.IsEmpty(result);
        }
    }
}
