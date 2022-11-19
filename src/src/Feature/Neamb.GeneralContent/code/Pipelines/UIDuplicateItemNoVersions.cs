using System;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Web.UI.Sheer;

namespace Neambc.Neamb.Feature.GeneralContent.Pipelines
{
    public class UIDuplicateItemNoVersions
    {
        public void Execute(ClientPipelineArgs args)
        {
            //The following code corresponding to the preexisting duplicate functionality taken from Sitecore.Kernel library
            Language language;
            Assert.ArgumentNotNull(args, "args");
            Database database = Factory.GetDatabase(args.Parameters["database"]);
            Assert.IsNotNull(database, args.Parameters["database"]);
            string itemInput = args.Parameters["id"];
            if (!Language.TryParse(args.Parameters["language"], out language))
            {
                language = Context.Language;
            }
            Item item = database.GetItem(ID.Parse(itemInput), language);
            if (item == null)
            {
                SheerResponse.Alert("Item not found.", Array.Empty<string>());
                args.AbortPipeline();
                return;
            }
            Item parent = item.Parent;
            if (parent == null)
            {
                SheerResponse.Alert("Cannot duplicate the root item.", Array.Empty<string>());
                args.AbortPipeline();
                return;
            }
            if (!parent.Access.CanCreate())
            {
                SheerResponse.Alert(Translate.Text("You do not have permission to duplicate \"{0}\".", new object[] { item.DisplayName }), Array.Empty<string>());
                args.AbortPipeline();
                return;
            }
            Log.Debug("Duplicate item: {0}", new string[] { AuditFormatter.FormatItem(item) });
            Item ims = Context.Workflow.DuplicateItem(item, args.Parameters["name"]);
            ims.DeleteChildren();

            //---------------------------------------------------------- end preexisting duplicate functionality

            //Verify the args passed to this method
            if (args.Parameters["latestversiontype"] != null)
            {
                var latestOnlyInCurrent = args.Parameters["latestversiontype"].Equals("current", StringComparison.InvariantCultureIgnoreCase);
                //Get the versions
                var versions = ims.Versions.GetVersions(true);
                // Get the language name
                var currentLanguageName = args.Parameters["language"];
                //foreach to remove the old versions
                foreach (var version in versions)
                {
                    if (!version.Versions.IsLatestVersion() || (latestOnlyInCurrent && !version.Language.Name.Equals(currentLanguageName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        version.Versions.RemoveVersion();
                    }
                }
            }
        }
    }
}