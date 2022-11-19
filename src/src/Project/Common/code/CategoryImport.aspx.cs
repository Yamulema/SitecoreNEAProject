using System;
using Neambc.Neamb.Feature.Rakuten.Repositories;

using Sitecore.DependencyInjection;

namespace Neamb.Project.Common
{
    public partial class CategoryImport : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void buttonUpload_Click(object sender, EventArgs e) {
            var categoryImportRepository =
                (ICategoryImportRepository)ServiceLocator.ServiceProvider.GetService(typeof(ICategoryImportRepository));

            var resultProcessImport = categoryImportRepository.ExecuteImportProcess(fileUpload.PostedFile.InputStream);
            if (resultProcessImport.Errors.Count == 0) {
                lblResultProcess.Text = "Processed sucessfully";
            } else {
                lblResultProcess.Text = "Processed with errors";
                foreach (var errorItem in resultProcessImport.Errors) {
                    lblResultProcess.Text = $"{lblResultProcess.Text} <br/> {errorItem}";
                }
            }
            if (resultProcessImport.NewItems.Count > 0) {
                lblResultProcess.Text = $"{lblResultProcess.Text} <br/> Items created {resultProcessImport.NewItems.Count} ";
            }
            if (resultProcessImport.UpdatedItems.Count > 0)
            {
                lblResultProcess.Text = $"{lblResultProcess.Text} <br/> Items updated {resultProcessImport.UpdatedItems.Count} ";
            }
            if (resultProcessImport.DeletedItems.Count > 0)
            {
                lblResultProcess.Text = $"{lblResultProcess.Text} <br/> Items deleted {resultProcessImport.DeletedItems.Count} ";
            }
        }
    }
}