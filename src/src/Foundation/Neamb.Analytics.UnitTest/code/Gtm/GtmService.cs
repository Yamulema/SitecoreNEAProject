using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Moq;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using Neambc.Neamb.Foundation.Analytics.Interfaces;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sitecore.FakeDb.Sites;

namespace Neambc.Neamb.Foundation.Analytics.UnitTest.Gtm
{
    [TestFixture]
    public class GtmService
    {
        #region Fields
        protected IGtmService _sut;
        private FakeSiteContext _context;
        private Mock<IHtmlProcessor> _htmlProcessorMock;
        #endregion

        [OneTimeSetUp]
        public void SetUpOnce()
        {
            // set up default mock objects once 
            // tests can still create their own, but 
            // defaults are available, and kept if used
            _htmlProcessorMock = new Mock<IHtmlProcessor>();
            _sut = new Analytics.Gtm.GtmService(_htmlProcessorMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            _context = new FakeSiteContext(new Sitecore.Collections.StringDictionary()
            {
                {"enableWebEdit", "true"},
                {"masterDatabase", "master"}
            });
            _htmlProcessorMock
                .Setup(x => x.GetTextHtml(It.IsAny<string>()))
                .Returns<string>(x => x);
            _htmlProcessorMock
                .Setup(x => x.GetSuppresedText(It.IsAny<string>()))
                .Returns<string>(x => x);
        }

        [Test]
        public void SerializeObject_Should_Return_Null_When_NullObjectIsPassed()
        {
            //Arrange

            //Act
            var result = _sut.SerializeObject(null);

            //Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void SerializeObject_Should_Return_Json_When_AnyObjectIsPassed()
        {
            //Arrange
            var @object = new Navigation()
            {
                Event = Randomizer.CreateRandomizer().GetString(4),
                NavText = Randomizer.CreateRandomizer().GetString(4),
                NavType = Randomizer.CreateRandomizer().GetString(4)
            };
            var expected = $"{{'event':'{@object.Event}','navType':'{@object.NavType}','navText':'{@object.NavText}'}}";

            //Act
            var result = _sut.SerializeObject(@object);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetOnClickEvent_Should_Return_emptyString_When_NullObjectIsPassed()
        {
            //Arrange

            //Act
            var result = _sut.GetOnClickEvent(null);

            //Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void GetOnClickEvent_Should_Return_String_When_AnyObjectIsPassed()
        {
            //Arrange
            var @object = new Navigation()
            {
                Event = Randomizer.CreateRandomizer().GetString(4),
                NavText = Randomizer.CreateRandomizer().GetString(4),
                NavType = Randomizer.CreateRandomizer().GetString(4)
            };
            var expected = $"onclick = \"{Configuration.DataLayerFunction}({{'event':'{@object.Event}','navType':'{@object.NavType}','navText':'{@object.NavText}'}},this);\"";

            //Act
            var result = _sut.GetOnClickEvent(@object);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void AddOnClickEvent_Should_AddTagging_When_InputHasNoEvent()
        {
            //Arrange
            var innerText = Randomizer.CreateRandomizer().GetString(5);
            var input = $@"<section class=""""><a href=""/home"">{innerText}</a></section>";
            var @object = new Navigation()
            {
                Event = Randomizer.CreateRandomizer().GetString(4),
                NavText = innerText,
                NavType = Randomizer.CreateRandomizer().GetString(4)
            };
            var expected = $@"<section class="""">" +
                $"<a href=\"/home\" onclick=\"{Configuration.DataLayerFunction}({{'event':'{@object.Event}','navType':'{@object.NavType}','navText':'{@object.NavText}'}},this);\">" +
                $"{innerText}" +
                "</a>" +
                "</section>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var anchor = htmlDoc.DocumentNode.SelectSingleNode("//a");
            //Act
            _sut.AddOnClickEvent(ref anchor, @object);

            //Assert
            Assert.AreEqual(expected, htmlDoc.DocumentNode.OuterHtml);
        }

        [Test]
        public void AddOnClickEvent_Should_AddTagging_When_InputHasEmptyEvent()
        {
            //Arrange
            var innerText = Randomizer.CreateRandomizer().GetString(5);
            var input = $@"<section class=""""><a href=""/home"" onclick="""">{innerText}</a></section>";
            var @object = new Navigation()
            {
                Event = Randomizer.CreateRandomizer().GetString(4),
                NavText = innerText,
                NavType = Randomizer.CreateRandomizer().GetString(4)
            };
            var expected = $@"<section class="""">" +
                $"<a href=\"/home\" onclick=\"{Configuration.DataLayerFunction}({{'event':'{@object.Event}','navType':'{@object.NavType}','navText':'{@object.NavText}'}},this);\">" +
                $"{innerText}" +
                "</a>" +
                "</section>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var anchor = htmlDoc.DocumentNode.SelectSingleNode("//a");

            //Act
            _sut.AddOnClickEvent(ref anchor, @object);

            //Assert
            Assert.AreEqual(expected, htmlDoc.DocumentNode.OuterHtml);
        }

        [Test]
        public void AddOnClickEvent_Should_AddTagging_When_InputHasNonClosedEvent()
        {
            //Arrange
            var innerText = Randomizer.CreateRandomizer().GetString(5);
            var input = $@"<section class=""""><a href=""/home"" onclick=""alert()"">{innerText}</a></section>";
            var @object = new Navigation()
            {
                Event = Randomizer.CreateRandomizer().GetString(4),
                NavText = innerText,
                NavType = Randomizer.CreateRandomizer().GetString(4)
            };
            var expected = $@"<section class="""">" +
                $"<a href=\"/home\" onclick=\"alert();{Configuration.DataLayerFunction}({{'event':'{@object.Event}','navType':'{@object.NavType}','navText':'{@object.NavText}'}},this);\">" +
                $"{innerText}" +
                "</a>" +
                "</section>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var anchor = htmlDoc.DocumentNode.SelectSingleNode("//a");
            //Act
            _sut.AddOnClickEvent(ref anchor, @object);

            //Assert
            Assert.AreEqual(expected, htmlDoc.DocumentNode.OuterHtml);
        }

        [Test]
        public void AddOnClickEvent_Should_AddTagging_When_InputHasClosedEvent()
        {
            //Arrange
            var innerText = Randomizer.CreateRandomizer().GetString(5);
            var input = $@"<section class=""""><a href=""/home"" onclick=""alert();"">{innerText}</a></section>";
            var @object = new Navigation()
            {
                Event = Randomizer.CreateRandomizer().GetString(4),
                NavText = innerText,
                NavType = Randomizer.CreateRandomizer().GetString(4)
            };
            var expected = $@"<section class="""">" +
                $"<a href=\"/home\" onclick=\"alert();{Configuration.DataLayerFunction}({{'event':'{@object.Event}','navType':'{@object.NavType}','navText':'{@object.NavText}'}},this);\">" +
                $"{innerText}" +
                "</a>" +
                "</section>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var anchor = htmlDoc.DocumentNode.SelectSingleNode("//a");

            //Act
            _sut.AddOnClickEvent(ref anchor, @object);

            //Assert
            Assert.AreEqual(expected, htmlDoc.DocumentNode.OuterHtml);
        }
        [Test]
        public void AddOnClickEvent_Should_ReplaceTagging_When_InputHasAlreadyATag()
        {
            //Arrange
            var innerText = Randomizer.CreateRandomizer().GetString(5);
            var input = $@"<section class=""""><a href=""/home"" onclick=""alert();{Configuration.DataLayerFunction}({{'event':'something','random':'lorem','ipsum':'rng'}},this);alert();"">{innerText}</a></section>";
            var @object = new Navigation()
            {
                Event = Randomizer.CreateRandomizer().GetString(4),
                NavText = innerText,
                NavType = Randomizer.CreateRandomizer().GetString(4)
            };
            var expected = $@"<section class="""">" +
                $"<a href=\"/home\" onclick=\"alert();{Configuration.DataLayerFunction}({{'event':'{@object.Event}','navType':'{@object.NavType}','navText':'{@object.NavText}'}},this);alert();\">" +
                $"{innerText}" +
                "</a>" +
                "</section>";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);
            var anchor = htmlDoc.DocumentNode.SelectSingleNode("//a");
            //Act
            _sut.AddOnClickEvent(ref anchor, @object, true);

            //Assert
            Assert.AreEqual(expected, htmlDoc.DocumentNode.OuterHtml);
        }
    }
}
