using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode.Encoder.StaticDataTables {
	public static class FormatInfoStringsList
	{
		private static Dictionary <ErrorCorrectionLvl, Dictionary<MaskPatterns, bool[]>> Dict = new Dictionary<ErrorCorrectionLvl, Dictionary<MaskPatterns, bool[]>>
		{
			{
				ErrorCorrectionLvl.Lvl7,
				new Dictionary<MaskPatterns, bool[]>
				{
					{
						MaskPatterns.Pattern0,
						new bool[] {true, true, true, false, true, true, true, true, true, false, false, false, true, false, false}
					}
				}
			},
			{
				ErrorCorrectionLvl.Lvl15,
				new Dictionary<MaskPatterns, bool[]>
				{
					{
						MaskPatterns.Pattern0,
						new bool[] {true, false, true, false, true, false, false, false, false, false, true, false, false, true, false}
					}
				}
			},
			{
				ErrorCorrectionLvl.Lvl25,
				new Dictionary<MaskPatterns, bool[]>
				{
					{
						MaskPatterns.Pattern0,
						new bool[] {false, true, true, false, true, false, true, false, true, false, true, true, true, true, true}
					},
					{
						MaskPatterns.Pattern1,
						new bool[] {false, true, true, false, false, false, false, false, true, true, false, true, false, false, false}
					}
				}
			},
			{
				ErrorCorrectionLvl.Lvl30,
				new Dictionary<MaskPatterns, bool[]>
				{
					{
						MaskPatterns.Pattern0,
						new bool[] {false, false, true, false, true, true, false, true, false, false, false, true, false, false, true}
					}
				}
			}
		};

		public static bool[] GetInfoString(int version,ErrorCorrectionLvl errorCorrectionLvl,MaskPatterns maskPattern)
		{
			if (version<7)
			{
				try {
					return Dict[errorCorrectionLvl][maskPattern];
				}
				catch ( Exception ) {
					
					throw new NotImplementedException("format not implemented");
				}
			}
			else
			{
				throw new NotImplementedException();
			}
		}

	}
}
