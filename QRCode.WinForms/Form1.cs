using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCode.Encoder;

namespace QRCode.WinForms {
	public partial class Form1 : Form {
		public Form1 () {
			InitializeComponent ();
		}

		private void Form1_Load ( object sender , EventArgs e )
		{
			this.comboBoxErrorCorrection.SelectedIndex = 0;
			this.comboBoxMode.SelectedIndex = 1;
			this.comboBoxMaskPattern.SelectedIndex = 0;
		}

		private void buttonGenerate_Click ( object sender , EventArgs e )
		{
			ErrorCorrectionLvl errorCorrectionLvl = ErrorCorrectionLvl.Lvl7;//by default
			#region determine err correction lvl
			switch ( this.comboBoxErrorCorrection.Text ) {
				case "L(7%)":
					errorCorrectionLvl = ErrorCorrectionLvl.Lvl7;
					break;
				case "M(15%)":
					errorCorrectionLvl = ErrorCorrectionLvl.Lvl15;
					break;
				case "Q(25%)":
					errorCorrectionLvl = ErrorCorrectionLvl.Lvl25;
					break;
				case "H(30%)":
					errorCorrectionLvl = ErrorCorrectionLvl.Lvl30;
					break;
				default:
					break;
			} 
			#endregion

			if ((EncodeMode)this.comboBoxMode.SelectedItem == EncodeMode.Alphanumeric)
			{
				this.textBoxInput.Text = this.textBoxInput.Text.ToUpper();
			}
			int Qrversion;

			try {
				this.pictureBoxQRCode.Image = Encode.GetImage (
						new EncodeProperties {
							Mode = (EncodeMode) this.comboBoxMode.SelectedItem ,
							ErrorCorrectionLvl = errorCorrectionLvl ,
							Text = this.textBoxInput.Text ,
							Size = (int) this.numericUpDownBitmapSize.Value
						} ,
						out Qrversion ,
						(MaskPatterns) this.comboBoxMaskPattern.SelectedItem
					);
				this.Text = "version of Qr code: " + Qrversion.ToString ();
			}
			catch ( Exception ex )
			{
				MessageBox.Show(ex.Message);
			}
			
		}
	}
}
