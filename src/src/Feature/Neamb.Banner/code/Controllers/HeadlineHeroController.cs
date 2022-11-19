using System;
using System.Web.Mvc;
using Neambc.Neamb.Feature.Banner.Models;
using Neambc.Neamb.Feature.Banner.Repositories.Enums;
using Neambc.Neamb.Foundation.Configuration.Utility;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;

namespace Neambc.Neamb.Feature.Banner.Controllers {
	public class HeadlineHeroController : BaseController {
		#region ActionResult Methods
		public ActionResult HeadlineHero() {
			return View("/Views/Neamb.Banner/HeadlineHero.cshtml", CreateModel());
		}
		public ActionResult HeadlineHeroLight()
		{
			var model = CreateModel();
			model.Type = HeadlineType.NoImageLight;
			return View("/Views/Neamb.Banner/HeadlineHero.cshtml", model );
		}
		public ActionResult OverlappingHeadlineHero() {
			return View("/Views/Neamb.Banner/OverlappingHeadlineHero.cshtml", CreateOverlappingModel());
		}

		private HeadlineHeroDto CreateOverlappingModel() {
			var model = new HeadlineHeroDto();
			model.Initialize(RenderingContext.Current.Rendering);
			model.PageTitle = CreateIdItemTuple(Templates.PageInfo.Fields.PageTitle, model.PageItem);
			model.Subheadline = CreateIdItemTuple(Templates.Article.Fields.Subheadline, model.PageItem);
			model.HeroImage = CreateIdItemTuple(Templates.Article.Fields.HeroImage, model.PageItem);
			model.HasImage = !string.IsNullOrEmpty(model.PageItem.ImageUrl(Templates.Article.Fields.HeroImage));
			return model;
		}

		private static Tuple<ID, Item> CreateIdItemTuple(ID id, Item item) {
			return new Tuple<ID, Item>(id, item);
		}
		#endregion

		#region Static Methods
		private static HeadlineHeroDto CreateModel() {
			var model = new HeadlineHeroDto();
			model.Initialize(RenderingContext.Current.Rendering);

			// Checks if the render has datasource, if not it uses the PageItem instead.
			if (model.HasDatasource) {
				model.PageTitle = new Tuple<ID, Item>(Templates.HeadlineHero.Fields.PageTitle, model.Item);
				model.Subheadline = new Tuple<ID, Item>(Templates.HeadlineHero.Fields.Subheadline, model.Item);
				model.QuoteWidget = new Tuple<ID, Item>(Templates.HeadlineHero.Fields.QuoteWidget, model.Item);
                model.Type = GetHeadlineHeroDtoType(model.Item, model.HasDatasource);
                model.CenteredText = ((CheckboxField)model.Item.Fields[Templates.HeadlineHero.Fields.CenteredText]).Checked;

                switch (model.Type) {
					case HeadlineType.NoImage:
						model.HeroImageSrc = string.Empty;
						break;
					case HeadlineType.HeroImage:
						model.HeroImageSrc = model.Item.ImageUrl(Templates.HeadlineHero.Fields.HeroImage);
						break;
					case HeadlineType.LargeHeroImage:
						model.HeroImageSrc = model.Item.ImageUrl(Templates.HeadlineHero.Fields.LargeHeroImage);
						break;
					default:
						model.HeroImageSrc = string.Empty;
						break;
				}
			} else if (model.PageItem != null) {
				model.PageTitle = new Tuple<ID, Item>(Templates.PageInfo.Fields.PageTitle, model.PageItem);
				model.Subheadline = new Tuple<ID, Item>(Templates.PageHeader.Fields.Subheadline, model.PageItem);
                model.QuoteWidget = new Tuple<ID, Item>(Templates.PageHeader.Fields.QuoteWidget, model.Item);
                model.Type = GetHeadlineHeroDtoType(model.Item, model.HasDatasource);
                model.CenteredText = ((CheckboxField)model.Item.Fields[Templates.PageHeader.Fields.CenteredText]).Checked;
                
                switch (model.Type) {
					case HeadlineType.NoImage:
						model.HeroImageSrc = string.Empty;
						break;
					case HeadlineType.HeroImage:
						model.HeroImageSrc = model.Item.ImageUrl(Templates.PageHeader.Fields.HeroImage);
						break;
					case HeadlineType.LargeHeroImage:
						model.HeroImageSrc = model.Item.ImageUrl(Templates.PageHeader.Fields.LargeHeroImage);
						break;
					default:
						model.HeroImageSrc = string.Empty;
						break;
				}
			}

			return model;
		}

		private static HeadlineType GetHeadlineHeroDtoType(Item item, bool hasDatasource) {
			HeadlineType result;
			if (hasDatasource) {
				result =
					!string.IsNullOrEmpty(item.Fields[Templates.HeadlineHero.Fields.HeroImage]?.Value)
						? HeadlineType.HeroImage
						: !string.IsNullOrEmpty(item.Fields[Templates.HeadlineHero.Fields.LargeHeroImage]?.Value)
							? HeadlineType.LargeHeroImage
							: HeadlineType.NoImage;
			} else {
				result =
					!string.IsNullOrEmpty(item.Fields[Templates.PageHeader.Fields.HeroImage]?.Value)
						? HeadlineType.HeroImage
						: !string.IsNullOrEmpty(item.Fields[Templates.PageHeader.Fields.LargeHeroImage]?.Value)
							? HeadlineType.LargeHeroImage
							: HeadlineType.NoImage;
			}
			return result;
		}
		#endregion
	}
}