using System;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Extensions;
using Sitecore.Publishing.Pipelines.PublishItem;
using Sitecore.SecurityModel;

namespace Neambc.Seiumb.Feature.Campaign.Events {
	/// <summary>
	/// Event class that adds rendering to parametered item after publishing
	/// </summary>
	public class ModifyRenderingsAfterPublish {
		/// <summary>
		/// Sitecores Home page
		/// </summary>
		public static readonly ID HOME_PAGE = new ID("{72051F7F-87F7-4279-B455-5A7EACA902F5}");

		/// <summary>
		/// Sitecore site parameter from Feature.Campaign.config
		/// </summary>
		public ArrayList Sites { get; } = new ArrayList();

		/// <summary>
		/// Templates to add rendering in process, parameter from Feature.Campaign.config
		/// </summary>
		public ArrayList Templates { get; } = new ArrayList();

#pragma warning disable RECS0154
		/// Parameter is never used
		/// <summary>
		/// Event that change final layout to parametrized templates
		/// </summary>
		/// <param name="sender">Unused</param>
		/// <param name="args"></param>
		public void ModifyItemRenderings(object sender, EventArgs args)
#pragma warning restore RECS0154 // Parameter is never used
		{
			var context = args is ItemProcessedEventArgs itemProcessedEventArgs ?
				itemProcessedEventArgs.Context :
				null;

			var web = Sitecore.Configuration.Factory.GetDatabase("web");
			//get the item to process
			var itemToProcess = web.GetItem(context.ItemId);
			//validate item template with parametrized list of templates
			if (itemToProcess != null && !ID.IsNullOrEmpty(itemToProcess.TemplateID) && Templates.Contains(itemToProcess.TemplateID.ToString())) {
				try {
					//execute process for all languages
					foreach (var itemLanguage in itemToProcess.Languages) {
						var itemLANG = itemToProcess.Database.GetItem(itemToProcess.ID, itemLanguage);
						if (itemLANG.Versions.Count > 0) {
							//gets the final layout field of the item
							var layoutField = new LayoutField(itemLANG.Fields[Sitecore.FieldIDs.FinalLayoutField]);
							//note: here could be master if we want to get the last layout definition
							var homePage = web.GetItem(HOME_PAGE);
							//gets layout field of the home page
							var HomelayoutField = new LayoutField(homePage.Fields[Sitecore.FieldIDs.FinalLayoutField]);
							//gets the field as xml to proccess from home page
							var xmlHomeRendering = XElement.Parse(HomelayoutField.Value).ToXmlNode();
							//gets specific node of the xml from home page
							var loginBannerRenderingXml = xmlHomeRendering.SelectSingleNode("d/r[@ph='login-banner']");
							//gets the field as xml to proccess item
							var xmlItemRendering = XElement.Parse(layoutField.Value).ToXmlNode();

							//load xmlnode as xml document (item)
							var xml = new XmlDocument();
							xml.LoadXml(xmlItemRendering.OuterXml);
							//import xmlnode to document 
							var impotedNode = xml.ImportNode(loginBannerRenderingXml, true);
							//select specific node from item final layout field if exist
							var hasRendering = xml.SelectSingleNode("r/d/r[@ph='login-banner']");

							if (hasRendering == null) {//add the selected node from home to item at the end of the layout
								var nodeAfter = xml.DocumentElement.SelectSingleNode(@"//d[last()]");
								nodeAfter.AppendChild(impotedNode);
							} else {//replace the selected node for the xmlnode from home page
								var toReplace = xml.SelectSingleNode("r/d");
								toReplace.ReplaceChild(impotedNode, hasRendering);
							}

							using (new SecurityDisabler()) {//save changes
								itemLANG.Editing.BeginEdit();
								layoutField.Value = xml.OuterXml;
								itemLANG.Editing.EndEdit();
							}
						}
					}


				} catch (Exception ex) {
					Log.Error(ex.Message, this);
				}

			}
		}
	}
}