using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode.Encoder {
	public class EncodeProperties
	{
		public string Text { get; set; }
		public EncodeMode Mode{get; set;}
		public ErrorCorrectionLvl ErrorCorrectionLvl {get; set;}
		public int Size { get; set; }
	}
}
