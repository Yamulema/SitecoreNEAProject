using HtmlAgilityPack;
using Moq;
using Neambc.Neamb.Foundation.Analytics.Gtm;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Neambc.Neamb.Foundation.Analytics.UnitTest.Gtm
{
    public class ContentCarouselProcessor
	{
        #region Fields
        protected IContentCarouselProcessor _sut;
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
            _sut = new Analytics.Gtm.ContentCarouselProcessor(_gtmService.Object);
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
		public void Process_Should_Return_ContentCarouselTagging_WithClassBoxes() {
			//Arrange
			var title = "Preventing Hearing Loss From Classroom Noise";

			var input = $"<section class=\"section-normal content-carousel\">" +
				$"<div class=\"container\">" +
				$"<div class=\"row\">" +
				$"<div class=\"col-md-12\">" +
				$"<div class=\"row boxes-container\">" +
				$"<div class=\"col-md-3\">" +
				$"<div class=\"box\"><span style=\"text-align: center;\">" +
				$"<a href=\"/family-and-wellness/how-to-prevent-hearing-loss-from-classroom-noise\">" +
				$"<img alt=\"Older male teacher instructing young music students\" width=\"100%\" src=\"\">" +
				$"</a></span><h3>" +
				$"<a href=\"/family-and-wellness/how-to-prevent-hearing-loss-from-classroom-noise\">" +
				$"<span>Preventing Hearing Loss From Classroom Noise</span></a></h3><p class=\"text-alpha\">" +
				$"<a href=\"/family-and-wellness/how-to-prevent-hearing-loss-from-classroom-noise\">" +
				$"<span>Use these tips to protect your hearing while modeling healthy listening behaviors to your students.</span>" +
				$"</a>" +
				$"</p>" +
				$"</div>" +
				$"</div><!--stop col-md-3-->" +
				$"</div><!--end row-->    " +
				$"</div><!--end container-->" +
				$"</div></div>" +
				$"</section>";
			var expected = $"<section class=\"section-normal content-carousel\">" +
				$"<div class=\"container\">" +
				$"<div class=\"row\">" +
				$"<div class=\"col-md-12\">" +
				$"<div class=\"row boxes-container\">" +
				$"<div class=\"col-md-3\">" +
				$"<div class=\"box\"><span style=\"text-align: center;\">" +
				$"<a href=\"/family-and-wellness/how-to-prevent-hearing-loss-from-classroom-noise\" onclick=\"{Configuration.DataLayerFunction}({{'event':'content','contentTitle':'{title}','contentLocation':'carousel'}});\">" +
				$"<img alt=\"Older male teacher instructing young music students\" width=\"100%\" src=\"\">" +
				$"</a></span><h3>" +
				$"<a href=\"/family-and-wellness/how-to-prevent-hearing-loss-from-classroom-noise\" onclick=\"{Configuration.DataLayerFunction}({{'event':'content','contentTitle':'{title}','contentLocation':'carousel'}});\">" +
				$"<span>Preventing Hearing Loss From Classroom Noise</span></a></h3><p class=\"text-alpha\">" +
				$"<a href=\"/family-and-wellness/how-to-prevent-hearing-loss-from-classroom-noise\" onclick=\"{Configuration.DataLayerFunction}({{'event':'content','contentTitle':'{title}','contentLocation':'carousel'}});\">" +
				$"<span>Use these tips to protect your hearing while modeling healthy listening behaviors to your students.</span>" +
				$"</a>" +
				$"</p>" +
				$"</div>" +
				$"</div><!--stop col-md-3-->" +
				$"</div><!--end row-->    " +
				$"</div><!--end container-->" +
				$"</div></div>" +
				$"</section>";

			var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(input);

			_gtmService.Setup(x => x.AddOnClickEvent(ref It.Ref<HtmlNode>.IsAny, It.IsAny<object>(), It.IsAny<bool>()))
				.Callback(new AddOnClickEventCallback((ref HtmlNode htmlNode, object @object, bool overrideEvent) => {
					htmlNode.Attributes.Add("onclick", $"{Configuration.DataLayerFunction}({{'event':'content','contentTitle':'{title}','contentLocation':'carousel'}});");
				}));
			//Act

			var result = _sut.Process(input);


			//Assert
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Process_Should_Return_ContentCarouselTagging_WithClassCard()
		{
			//Arrange
			var title = "Did you know you have no-cost life insurance?";

			var input = $"<section class=\"section-normal slider-section c-014 content-carousel\"><div class=\"container\"><h2 class=\"header text-center\">Don’t miss these exclusive member benefits</h2><div class=\"row\"><div class=\"col-md-12\"><div class=\"row\"><div class=\"regular slider content-carousel slick-initialized slick-slider slick-dotted\"><button class=\"slick-prev slick-arrow slick-disabled\" aria-label=\"Previous\" type=\"button\" aria-disabled=\"true\" style=\"display: inline-block;\">Previous</button><div class=\"slick-list draggable\"><div class=\"slick-track\" style=\"opacity: 1; width: 2170px; transform: translate3d(0px, 0px, 0px);\"><div class=\"slick-slide slick-current slick-active\" data-slick-index=\"0\" aria-hidden=\"false\" style=\"width: 290px;\" role=\"tabpanel\" id=\"slick-slide00\"><div><div style=\"width: 100%; display: inline-block;\"><div class=\"card\"><a href=\"/products/nea-complimentary-life-insurance\" tabindex=\"0\"><img class=\"thumb img-responsive\" alt=\"NEA Complimentary Life Insurance\" src=\"-/media/6d97bb0412a14321a98cb40fb709cfb2.ashx?w=100%&amp;hash=DE21F59D00710C7AA36245BD389F67CA3AC28C8B\"></a><h3><a href=\"/products/nea-complimentary-life-insurance\" tabindex=\"0\">Did you know you have no-cost life insurance?</a></h3><p><a href=\"/products/nea-complimentary-life-insurance\" tabindex=\"0\">Eligible NEA members receive trusted insurance protection. Register your beneficiary.</a></p></div></div></div></div></div></div></div><!--slick--></div><!--row---></div><!-- col-md-12--></div></div></section>";
			var expected = $"<section class=\"section-normal slider-section c-014 content-carousel\"><div class=\"container\"><h2 class=\"header text-center\">Don’t miss these exclusive member benefits</h2><div class=\"row\"><div class=\"col-md-12\"><div class=\"row\"><div class=\"regular slider content-carousel slick-initialized slick-slider slick-dotted\"><button class=\"slick-prev slick-arrow slick-disabled\" aria-label=\"Previous\" type=\"button\" aria-disabled=\"true\" style=\"display: inline-block;\">Previous</button><div class=\"slick-list draggable\"><div class=\"slick-track\" style=\"opacity: 1; width: 2170px; transform: translate3d(0px, 0px, 0px);\"><div class=\"slick-slide slick-current slick-active\" data-slick-index=\"0\" aria-hidden=\"false\" style=\"width: 290px;\" role=\"tabpanel\" id=\"slick-slide00\"><div><div style=\"width: 100%; display: inline-block;\"><div class=\"card\">" +
				$"<a href=\"/products/nea-complimentary-life-insurance\" tabindex=\"0\" onclick=\"{Configuration.DataLayerFunction}({{'event':'content','contentTitle':'{title}','contentLocation':'carousel'}});\">" +
				$"<img class=\"thumb img-responsive\" alt=\"NEA Complimentary Life Insurance\" src=\"-/media/6d97bb0412a14321a98cb40fb709cfb2.ashx?w=100%&amp;hash=DE21F59D00710C7AA36245BD389F67CA3AC28C8B\">" +
				$"</a><h3>" +
				$"<a href=\"/products/nea-complimentary-life-insurance\" tabindex=\"0\" onclick=\"{Configuration.DataLayerFunction}({{'event':'content','contentTitle':'{title}','contentLocation':'carousel'}});\">" +
				$"Did you know you have no-cost life insurance?" +
				$"</a></h3><p>" +
				$"<a href=\"/products/nea-complimentary-life-insurance\" tabindex=\"0\" onclick=\"{Configuration.DataLayerFunction}({{'event':'content','contentTitle':'{title}','contentLocation':'carousel'}});\">" +
				$"Eligible NEA members receive trusted insurance protection. Register your beneficiary.</a></p></div></div></div></div></div></div></div><!--slick--></div><!--row---></div><!-- col-md-12--></div></div></section>";

			var htmlDoc = new HtmlDocument();
			htmlDoc.LoadHtml(input);

			_gtmService.Setup(x => x.AddOnClickEvent(ref It.Ref<HtmlNode>.IsAny, It.IsAny<object>(), It.IsAny<bool>()))
				.Callback(new AddOnClickEventCallback((ref HtmlNode htmlNode, object @object, bool overrideEvent) => {
					htmlNode.Attributes.Add("onclick", $"{Configuration.DataLayerFunction}({{'event':'content','contentTitle':'{title}','contentLocation':'carousel'}});");
				}));
			//Act

			var result = _sut.Process(input);


			//Assert
			Assert.AreEqual(expected, result);
		}
	}
}
