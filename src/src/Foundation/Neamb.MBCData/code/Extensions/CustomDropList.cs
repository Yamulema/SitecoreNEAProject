using System.Web.UI;
using Neambc.Neamb.Foundation.MBCData.Managers;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;

namespace Neambc.Neamb.Foundation.MBCData.Extensions
{
    public class CustomDropList : Sitecore.Web.UI.HtmlControls.Control {
        private ISeminarRepository _seminarRepository;

        public CustomDropList()
        {
            this.Class = "scContentControl";
            this.Activation = true;
        }

        protected override void DoRender(HtmlTextWriter output) {
            Log.Info("Starting CustomDropList",this);
            string err = null;
            _seminarRepository =
                    (ISeminarRepository)ServiceLocator.ServiceProvider.GetService(typeof(ISeminarRepository));

            var resultSeminarsView = _seminarRepository.GetSeminaries();
            
            if (resultSeminarsView == null) {
                err = Sitecore.Globalization.Translate.Text(
                    "Value not in the selection list.");
            } else {


                output.Write("<select" + this.GetControlAttributes() + ">");
                output.Write("<option value=\"\"></option>");
                bool valueFound = string.IsNullOrEmpty(this.Value);
                string key = "";
                string value = "";

                foreach (var seminarView in resultSeminarsView) {
                    key = seminarView.SeminarId;
                    //key = $"{seminarView.LeaCode}|{seminarView.SeminarId}" ;
                    value = $"{seminarView.SeminarName}-{seminarView.LeaCode}";
                    valueFound = valueFound || key == this.Value;
                    output.Write(string.Format(
                        @"<option value=""{0}"" {1}>{2}</option>",
                        key,
                        this.Value == key ? " selected=\"selected\"" : string.Empty,
                        value));

                }

                if (!valueFound) {
                    err = Sitecore.Globalization.Translate.Text(
                        "Value not in the selection list.");
                }
            }
            if (err != null)
            {
                output.Write("<optgroup label=\"" + err + "\">");
                output.Write(
                    "<option value=\"" + this.Value + "\" selected=\"selected\">" + this.Value + "</option>");
                output.Write("</optgroup>");
            }

            output.Write("</select>");

            if (err != null)
            {
                output.Write("<div style=\"color:#999999;padding:2px 0px 0px 0px\">{0}</div>", err);
            }
        }
        protected override bool LoadPostData(string value)
        {
            if (value == null)
            {
                return false;
            }

            if (this.GetViewStateString("Value") != value)
            {
                CustomDropList.SetModified();
            }

            this.SetViewStateString("Value", value);
            return true;
        }

        private static void SetModified()
        {
            Sitecore.Context.ClientPage.Modified = true;
        }
    }
}