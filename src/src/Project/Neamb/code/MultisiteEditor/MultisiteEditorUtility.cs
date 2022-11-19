using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web;

namespace Neambc.Neamb.Project.Web.MultisiteEditor
{
    public static class MultisiteEditorUtility
    {
        private const string ID = "id";
        public static Database GetDatabase()
        {
            return Sitecore.Configuration.Factory.GetDatabase(Configuration.SitecoreDatabase);
        }
        public static Item GetItem(Database database)
        {
            var itemId = WebUtil.GetQueryString(ID);
            if (itemId is null || itemId == string.Empty)
            {
                Log.Warn("itemID is null or empty", database);
                return null;
            }
            return database.GetItem(new ID(itemId));
        }
        public static string[] GetCssPath(Item parent)
        {
            try
            {
                if (parent.ID == Configuration.NeambItemId)
                {
                    return SplitCssPath(Configuration.NeambCssPath);
                }

                if (parent.ID == Configuration.SeiumbItemId)
                {
                    return SplitCssPath(Configuration.SeiumbCssPath);
                }

                return parent.ID == Configuration.SitecoreItemId ?
                    new string[] { } :
                    GetCssPath(parent.Parent);
            }
            catch (Exception ex)
            {
                Log.Error("Multisite editor failed to determine parent css path", ex, typeof(MultisiteEditorUtility));
                return new string[] { };
            }
        }

        private static string[] SplitCssPath(string cssPath)
        {
            return !string.IsNullOrEmpty(cssPath) ? cssPath.Split('|') : new string[] { };
        }
        public static string GetToolsFilePath(Item parent)
        {
            Log.Info($"MultisiteEditorConfiguration GetToolsFilePath Begin", new object());
            try
            {
                if (parent.ID == Configuration.NeambItemId)
                {
                    Log.Info($"MultisiteEditorConfiguration GetToolsFilePath: {Configuration.NeambToolsfilePath}", new object());
                    return Configuration.NeambToolsfilePath;
                }
                else if (parent.ID == Configuration.SeiumbItemId)
                {
                    Log.Info($"MultisiteEditorConfiguration GetToolsFilePath: {Configuration.SeiumbToolsfilePath}", new object());
                    return Configuration.SeiumbToolsfilePath;
                }
                else if (parent.ID == Configuration.SitecoreItemId)
                {
                    Log.Info($"MultisiteEditorConfiguration GetToolsFilePath: {Configuration.SitecoreItemId}", new object());
                    return string.Empty;
                }
                else
                {
                    Log.Info($"MultisiteEditorConfiguration GetToolsFilePath None", new object());
                    return GetToolsFilePath(parent.Parent);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Multisite editor failed to determine parent for ToolsFilePath", ex, typeof(MultisiteEditorUtility));
                return string.Empty;
            }
        }

        public static ID GetSnippetsRootId(Item parent)
        {
            try
            {
                if (parent.ID == Configuration.NeambItemId)
                {
                    return Configuration.NeambSnippetsRootId;
                }
                else if (parent.ID == Configuration.SeiumbItemId)
                {
                    return Configuration.SeiumbSnippetsRootId;
                }
                else if (parent.ID == Configuration.SitecoreItemId)
                {
                    return null;
                }
                else
                {
                    return GetSnippetsRootId(parent.Parent);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Multisite editor failed to determine parent for SnippetsRootId", ex, typeof(MultisiteEditorUtility));
                return null;
            }
        }
    }
}