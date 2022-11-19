using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Shell.Framework.Commands;

namespace Neambc.Neamb.Feature.GeneralContent.Commands
{
    [Serializable]
    public class DuplicateItemNoPreviousVersions : Command
    {
        public override void Execute(CommandContext context)
        {
            if (context.Items.Length != 1 || context.Items[0] == null) return;
            var item = context.Items[0];
            var parameters = new NameValueCollection();
            parameters.Add("database", item.Database.Name);
            parameters.Add("id", item.ID.ToString());
            parameters.Add("language", item.Language.ToString());
            parameters.Add("version", item.Version.ToString());
            parameters.Add("latestversiontype", "all");
            Context.ClientPage.Start("uiDuplicateItemNoVersions", parameters);
        }
    }
}