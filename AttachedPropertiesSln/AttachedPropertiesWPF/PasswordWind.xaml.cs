using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AttachedPropertiesWPF
{
	/// <summary>
	/// Логика взаимодействия для PasswordWind.xaml
	/// </summary>
	public partial class PasswordWind : Window
	{

		private readonly PasswordModel model = new PasswordModel();

		public PasswordWind()
		{
			InitializeComponent();
		}

		public Func<IEnumerable<char>, byte[]> GetHashDelegate { get; set; }
		public Func<string, byte[]> GetRsaDelegate { get; set; }

		private void HashExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			tbHash.Text =string.Join(" ", GetHashDelegate?.Invoke(tbLogin.Text).Select(b => b.ToString()));
		}

		private void RsaExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			byte[] encr = GetRsaDelegate(model.PublicKey);
			if (encr != null)
			{
				tbEncr.Text = string.Join(" ", encr.Select(b => b.ToString()));

				model.SetPassword(encr);

				tbDecr.Text = model.Password;
			}
			else
				tbEncr.Text = tbDecr.Text = null;
		}

	}
}
