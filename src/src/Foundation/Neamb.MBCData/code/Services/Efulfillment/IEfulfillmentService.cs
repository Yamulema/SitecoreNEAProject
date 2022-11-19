using Neambc.Neamb.Foundation.MBCData.Model;
using Neambc.Neamb.Foundation.MBCData.Model.CreateResetToken;

namespace Neambc.Neamb.Foundation.MBCData.Services.Efulfillment
{
    public interface IEfulfillmentService {
        /// <summary>
        /// Create a password reset token for a user.
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="unionId">Seiumb/Neamb</param>
        /// <returns></returns>
        /// <returns>Response of the AWS WebService</returns>
        byte[] GetPdfFile(PdfRequest pdfRequest);
    }
}
