using System.Security.Cryptography;
using System.Text;

namespace AttachedPropertiesWPF
{
	public class PasswordModel
	{
		private readonly RSAParameters privateKey;
		public string PublicKey { get; }
		public string Password { get; private set; }
		private readonly RSACryptoServiceProvider provideRSA = new RSACryptoServiceProvider(4096);
		public PasswordModel()
		{
			privateKey = provideRSA.ExportParameters(true);
			PublicKey = provideRSA.ToXmlString(false);
		}

		public void SetPassword(byte[] encrypt)
		{
			if (encrypt != null)
			{
				provideRSA.ImportParameters(privateKey);
				byte[] decr = provideRSA.Decrypt(encrypt, false);

				Password = Encoding.Unicode.GetString(decr);
			}
			else
				Password = null;

		}

	}
}
