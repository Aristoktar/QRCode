using QRCode.Encoder;

namespace QRCode.WinForms {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose ();
			}
			base.Dispose ( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent () {
			this.textBoxInput = new System.Windows.Forms.TextBox();
			this.pictureBoxQRCode = new System.Windows.Forms.PictureBox();
			this.comboBoxErrorCorrection = new System.Windows.Forms.ComboBox();
			this.comboBoxMode = new System.Windows.Forms.ComboBox();
			this.buttonGenerate = new System.Windows.Forms.Button();
			this.numericUpDownBitmapSize = new System.Windows.Forms.NumericUpDown();
			this.comboBoxMaskPattern = new System.Windows.Forms.ComboBox();
			this.labelMaskPattern = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBitmapSize)).BeginInit();
			this.SuspendLayout();
			// 
			// textBoxInput
			// 
			this.textBoxInput.Location = new System.Drawing.Point(12, 43);
			this.textBoxInput.Name = "textBoxInput";
			this.textBoxInput.Size = new System.Drawing.Size(168, 20);
			this.textBoxInput.TabIndex = 0;
			this.textBoxInput.Text = "hello world";
			// 
			// pictureBoxQRCode
			// 
			this.pictureBoxQRCode.Location = new System.Drawing.Point(12, 100);
			this.pictureBoxQRCode.Name = "pictureBoxQRCode";
			this.pictureBoxQRCode.Size = new System.Drawing.Size(200, 200);
			this.pictureBoxQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxQRCode.TabIndex = 1;
			this.pictureBoxQRCode.TabStop = false;
			// 
			// comboBoxErrorCorrection
			// 
			this.comboBoxErrorCorrection.FormattingEnabled = true;
			this.comboBoxErrorCorrection.Items.AddRange(new object[] {
            "L(7%)",
            "M(15%)",
            "Q(25%)",
            "H(30%)"});
			this.comboBoxErrorCorrection.Location = new System.Drawing.Point(321, 40);
			this.comboBoxErrorCorrection.Name = "comboBoxErrorCorrection";
			this.comboBoxErrorCorrection.Size = new System.Drawing.Size(121, 21);
			this.comboBoxErrorCorrection.TabIndex = 2;
			// 
			// comboBoxMode
			// 
			this.comboBoxMode.FormattingEnabled = true;
			this.comboBoxMode.Items.AddRange(new object[] {
            QRCode.Encoder.EncodeMode.Numeric,
            QRCode.Encoder.EncodeMode.Alphanumeric,
            QRCode.Encoder.EncodeMode.Byte,
            QRCode.Encoder.EncodeMode.Kanji});
			this.comboBoxMode.Location = new System.Drawing.Point(321, 106);
			this.comboBoxMode.Name = "comboBoxMode";
			this.comboBoxMode.Size = new System.Drawing.Size(121, 21);
			this.comboBoxMode.TabIndex = 3;
			// 
			// buttonGenerate
			// 
			this.buttonGenerate.Location = new System.Drawing.Point(205, 40);
			this.buttonGenerate.Name = "buttonGenerate";
			this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
			this.buttonGenerate.TabIndex = 4;
			this.buttonGenerate.Text = "Generate!";
			this.buttonGenerate.UseVisualStyleBackColor = true;
			this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
			// 
			// numericUpDownBitmapSize
			// 
			this.numericUpDownBitmapSize.Location = new System.Drawing.Point(321, 189);
			this.numericUpDownBitmapSize.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.numericUpDownBitmapSize.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.numericUpDownBitmapSize.Name = "numericUpDownBitmapSize";
			this.numericUpDownBitmapSize.Size = new System.Drawing.Size(120, 20);
			this.numericUpDownBitmapSize.TabIndex = 5;
			this.numericUpDownBitmapSize.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			// 
			// comboBoxMaskPattern
			// 
			this.comboBoxMaskPattern.FormattingEnabled = true;
			this.comboBoxMaskPattern.Items.AddRange(new object[] {
            MaskPatterns.Pattern0,
			MaskPatterns.Pattern1,
			MaskPatterns.Pattern2,
			MaskPatterns.Pattern3,
			MaskPatterns.Pattern4,
			MaskPatterns.Pattern5,
			MaskPatterns.Pattern7} );
			this.comboBoxMaskPattern.Location = new System.Drawing.Point(321, 256);
			this.comboBoxMaskPattern.Name = "comboBoxMaskPattern";
			this.comboBoxMaskPattern.Size = new System.Drawing.Size(121, 21);
			this.comboBoxMaskPattern.TabIndex = 6;
			// 
			// labelMaskPattern
			// 
			this.labelMaskPattern.AutoSize = true;
			this.labelMaskPattern.Location = new System.Drawing.Point(262, 259);
			this.labelMaskPattern.Name = "labelMaskPattern";
			this.labelMaskPattern.Size = new System.Drawing.Size(41, 13);
			this.labelMaskPattern.TabIndex = 7;
			this.labelMaskPattern.Text = "Pattern";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(517, 428);
			this.Controls.Add(this.labelMaskPattern);
			this.Controls.Add(this.comboBoxMaskPattern);
			this.Controls.Add(this.numericUpDownBitmapSize);
			this.Controls.Add(this.buttonGenerate);
			this.Controls.Add(this.comboBoxMode);
			this.Controls.Add(this.comboBoxErrorCorrection);
			this.Controls.Add(this.pictureBoxQRCode);
			this.Controls.Add(this.textBoxInput);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBitmapSize)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBoxInput;
		private System.Windows.Forms.PictureBox pictureBoxQRCode;
		private System.Windows.Forms.ComboBox comboBoxErrorCorrection;
		private System.Windows.Forms.ComboBox comboBoxMode;
		private System.Windows.Forms.Button buttonGenerate;
		private System.Windows.Forms.NumericUpDown numericUpDownBitmapSize;
		private System.Windows.Forms.ComboBox comboBoxMaskPattern;
		private System.Windows.Forms.Label labelMaskPattern;
	}
}

