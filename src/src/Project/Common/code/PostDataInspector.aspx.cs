using System;
using System.Reflection;
using System.Text;

namespace Neamb.Project.Common
{
    public partial class PostDataInspector : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var propInfo = Request.Form.GetType().GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            propInfo.SetValue(Request.Form, false, new object[] { });

            string[] keys = Request.Form.AllKeys;
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");

            for (int i = 0; i < keys.Length; i++)
            {
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", keys[i], Request.Form[keys[i]]);
                Request.Form.Remove(keys[i]);
            }

            
            sb.Append("</table>");
            divResult.InnerHtml = sb.ToString();
            
            
        }
    }
}