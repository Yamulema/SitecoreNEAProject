using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.Product.Model;

namespace Neambc.Neamb.Foundation.Product.Interfaces
{
    public interface IPdfManager {
        byte[] GetPdfFile(
            string materialId,
            string uniqueName,
            PdfRequest pdfRequest,
            string bucketName,
            string custom1 = "",
            string custom2 = "",
            bool isNeamb = true
        );
        byte[] VerifyExistencePdfFile(string uniqueName, string bucketName, bool isNeamb = true);
        string GetPdfUrl(string uniqueName, bool isNeamb = true);
    }
}