using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Moq;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sitecore.FakeDb.Sites;

namespace Neambc.Neamb.Foundation.Analytics.UnitTest.Gtm
{
    public class FooterProcessor
    {
        #region Fields
        protected IFooterProcessor _sut;
        protected Mock<IGtmService> _gtmService;
        delegate void AddOnClickEventCallback(ref HtmlNode anchorNode, object @object, bool overrideEvent);
        #endregion

        [OneTimeSetUp]
        public void SetUpOnce()
        {
            // set up default mock objects once 
            // tests can still create their own, but 
            // defaults are available, and kept if used
            _gtmService = new Mock<IGtmService>();
            _sut = new Analytics.Gtm.FooterProcessor(_gtmService.Object);
        }

        [SetUp]
        public void SetUp()
        {
        }

        [TestCase("")]
        [TestCase(null)]
        public void Process_Should_Return_SameInput_When_InputIsInvalidHtml(string input)
        {
            //Arrange
            //Act
            var result = _sut.Process(input);

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [Test]
        public void Process_Should_Return_SameInput_When_stringIsNotHtml()
        {
            //Arrange
            var input = Randomizer.CreateRandomizer().GetString(100).Replace("<", string.Empty);

            //Act
            var result = _sut.Process(input);

            //Assert
            Assert.AreEqual(input, result);
        }

        [Test]
        public void Process_Should_Return_FooterTagging()
        {
            //Arrange
            var innerText = Randomizer.CreateRandomizer().GetString(5);

            var input = $@"<div class=""{Configuration.FooterClass}""><a href=""/home"">{innerText}</a></div>";
            var expected = $"<div class=\"{Configuration.FooterClass}\">" +
                $"<a href=\"/home\" onclick=\"{Configuration.DataLayerFunction}({{'event':'navigation','navType':'footer','navText':'{innerText}'}});\">" +
                $"{innerText}" +
                "</a>" +
                "</div>";
            _gtmService.Setup(x => x.SerializeObject(It.IsAny<object>()))
                .Returns($@"{{'event':'navigation','navType':'footer','navText':'{innerText}'}}");

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var anchor = htmlDoc.DocumentNode.SelectSingleNode("//a");
            
            _gtmService.Setup(x => x.AddOnClickEvent(ref It.Ref<HtmlNode>.IsAny, It.IsAny<object>(), It.IsAny<bool>()))
                .Callback(new AddOnClickEventCallback((ref HtmlNode htmlNode, object @object, bool overrideEvent) => {
                    htmlNode.Attributes.Add("onclick", $"{Configuration.DataLayerFunction}({{'event':'navigation','navType':'footer','navText':'{innerText}'}});");
                }));
            //Act
            
                var result = _sut.Process(input);


                //Assert
                Assert.AreEqual(expected, result);
        }
    }
}
