using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Controls.RichTextEditor;
using Telerik.Web.UI;

namespace Neambc.Neamb.Project.Web.MultisiteEditor {
	public class MultisiteEditorConfiguration : EditorConfiguration {
		public MultisiteEditorConfiguration(Item profile) : base(profile) {
		}

		protected override void SetupStylesheets() {
            Log.Info("MultisiteEditorConfiguration SetupStylesheets", this);
            var current = MultisiteEditorUtility.GetItem(MultisiteEditorUtility.GetDatabase());
			if (current != null && current.Parent != null) {
                //The order of the following methods matters.
                Log.Info("MultisiteEditorConfiguration SetToolsFile", this);
                SetToolsFile(MultisiteEditorUtility.GetToolsFilePath(current.Parent));
                Log.Info("MultisiteEditorConfiguration SetSnippets", this);
                SetSnippets(MultisiteEditorUtility.GetSnippetsRootId(current.Parent));
                Log.Info("MultisiteEditorConfiguration SetStyleSheet", this);
                SetStyleSheet(MultisiteEditorUtility.GetCssPath(current.Parent));
			}
		}
		private void SetStyleSheet(string[] cssPaths) {
			foreach (var style in cssPaths) {
                Log.Info($"MultisiteEditorConfiguration SetStyleSheet: {style}", this);
                Editor.CssFiles.Add(style);
			}

			base.SetupStylesheets();
		}
		private void SetToolsFile(string toolsFilePath) {
			if (!string.IsNullOrEmpty(toolsFilePath)) {
				Editor.ToolsFile = toolsFilePath;
                Log.Info($"MultisiteEditorConfiguration SetToolsFile: {toolsFilePath}", this);
            }
		}

		private void SetSnippets(ID rootId) {
			if (!ID.IsNullOrEmpty(rootId)) {
				try {
					var database = Sitecore.Configuration.Factory.GetDatabase(Configuration.SitecoreCoreDatabase);
					var rootItem = database.GetItem(rootId);
					if (rootItem == null) {
						return;
					}
                    
                    var snippets = rootItem.Axes.GetDescendants();
                    Log.Info($"MultisiteEditorConfiguration SetSnippets: {snippets}", this);
                    foreach (var snippet in snippets) {
						Editor.Snippets.Add(new EditorSnippet() {
							Name = snippet.Fields["Header"]?.Value ?? string.Empty,
							Value = snippet.Fields["Value"]?.Value ?? string.Empty
						});
					}
				} catch (Exception e) {
					Log.Error("Multisite editor failed to determine Snippets", e, typeof(MultisiteEditorConfiguration));
					return;
				}
			}
		}
	}
}