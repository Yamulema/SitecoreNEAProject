namespace Neambc.Neamb.Foundation.MBCData.Services.Aws {
    public interface ICloudFrontFactory {
        ICloudFrontProxy GetClient();
    }
}