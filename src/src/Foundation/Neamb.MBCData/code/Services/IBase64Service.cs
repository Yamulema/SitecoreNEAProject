namespace Neambc.Neamb.Foundation.MBCData.Services
{
	public interface IBase64Service {
		string Encode(string plain);
		string Decode(string encoded);
        string EncodeImage(byte[] inputFile);
        byte[] ConvertBytes(string encoded);
    }
}
