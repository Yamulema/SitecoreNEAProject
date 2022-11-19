using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Sitecore.Diagnostics;
using Sitecore.Shell.Controls.RADEditor;

namespace Neambc.Neamb.Project.Web.MultisiteEditor
{
    public class MultisiteEditorPreview : Preview
    {
        private const string LinkFormat = "<link href=\"{0}\" rel=\"stylesheet\">";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var stylesheetPath = MultisiteEditorUtility.GetCssPath(MultisiteEditorUtility.GetItem(MultisiteEditorUtility.GetDatabase()));

            foreach (var style in stylesheetPath)
            {
                Stylesheets.Controls.Add(new LiteralControl(string.Format(LinkFormat, style)));
            }

        }
    }
}