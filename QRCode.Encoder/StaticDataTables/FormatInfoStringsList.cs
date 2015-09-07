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
new bool[] {true,true,true,false, true,true,true,true,true,false, false, false, true,false, false}
 },
{ 
MaskPatterns.Pattern1, 
new bool[] {true,true,true,false, false, true,false, true,true,true,true,false, false, true,true}
 },
{ 
MaskPatterns.Pattern2, 
new bool[] {true,true,true,true,true,false, true,true,false, true,false, true,false, true,false}
 },
{ 
MaskPatterns.Pattern3, 
new bool[] {true,true,true,true,false, false, false, true,false, false, true,true,true,false, true}
 },
{ 
MaskPatterns.Pattern4, 
new bool[] {true,true,false, false, true,true,false, false, false, true,false, true,true,true,true}
 },
{ 
MaskPatterns.Pattern5, 
new bool[] {true,true,false, false, false, true,true,false, false, false, true,true,false, false, false}
 },
{ 
MaskPatterns.Pattern6, 
new bool[] {true,true,false, true,true,false, false, false, true,false, false, false, false, false, true}
 },
{ 
MaskPatterns.Pattern7, 
new bool[] {true,true,false, true,false, false, true,false, true,true,true,false, true,true,false}
 }
	}

			},
			{
				ErrorCorrectionLvl.Lvl15,
				new Dictionary<MaskPatterns, bool[]>
				{
					{ 
MaskPatterns.Pattern0, 
new bool[] {true,false, true,false, true,false, false, false, false, false, true,false, false, true,false }
 },
{ 
MaskPatterns.Pattern1, 
new bool[] {true,false, true,false, false, false, true,false, false, true,false, false, true,false, true}
 },
{ 
MaskPatterns.Pattern2, 
new bool[] {true,false, true,true,true,true,false, false, true,true,true,true,true,false, false }
 },
{ 
MaskPatterns.Pattern3, 
new bool[] {true,false, true,true,false, true,true,false, true,false, false, true,false, true,true}
 },
{ 
MaskPatterns.Pattern4, 
new bool[] {true,false, false, false, true,false, true,true,true,true,true,true,false, false, true}
 },
{ 
MaskPatterns.Pattern5, 
new bool[] {true,false, false, false, false, false, false, true,true,false, false, true,true,true,false }
 },
{ 
MaskPatterns.Pattern6, 
new bool[] {true,false, false, true,true,true,true,true,false, false, true,false, true,true,true}
 },
{ 
MaskPatterns.Pattern7, 
new bool[] {true,false, false, true,false, true,false, true,false, true,false, false, false, false, false }
 }

				}
			},
			{
				ErrorCorrectionLvl.Lvl25,
				new Dictionary<MaskPatterns, bool[]>
				{
					{ 
MaskPatterns.Pattern0, 
new bool[] {false, true,true,false, true,false, true,false, true,false, true,true,true,true,true}
 },
{ 
MaskPatterns.Pattern1, 
new bool[] {false, true,true,false, false, false, false, false, true,true,false, true,false, false, false }
 },
{ 
MaskPatterns.Pattern2, 
new bool[] {false, true,true,true,true,true,true,false, false, true,true,false, false, false, true}
 },
{ 
MaskPatterns.Pattern3, 
new bool[] {false, true,true,true,false, true,false, false, false, false, false, false, true,true,false }
 },
{ 
MaskPatterns.Pattern4, 
new bool[] {false, true,false, false, true,false, false, true,false, true,true,false, true,false, false }
 },
{ 
MaskPatterns.Pattern5, 
new bool[] {false, true,false, false, false, false, true,true,false, false, false, false, false, true,true}
 },
{ 
MaskPatterns.Pattern6, 
new bool[] {false, true,false, true,true,true,false, true,true,false, true,true,false, true,false }
 },
{ 
MaskPatterns.Pattern7, 
new bool[] {false, true,false, true,false, true,true,true,true,true,false, true,true,false, true}
 }
				}
			},
			{
				ErrorCorrectionLvl.Lvl30,
				new Dictionary<MaskPatterns, bool[]>
				{
					{ 
MaskPatterns.Pattern0, 
new bool[] {false, false, true,false, true,true,false, true,false, false, false, true,false, false, true}
 },
{ 
MaskPatterns.Pattern1, 
new bool[] {false, false, true,false, false, true,true,true,false, true,true,true,true,true,false }
 },
{ 
MaskPatterns.Pattern2, 
new bool[] {false, false, true,true,true,false, false, true,true,true,false, false, true,true,true}
 },
{ 
MaskPatterns.Pattern3, 
new bool[] {false, false, true,true,false, false, true,true,true,false, true,false, false, false, false}
 },
{ 
MaskPatterns.Pattern4, 
new bool[] {false, false, false, false, true,true,true,false, true,true,false, false, false, true,false }
 },
{ 
MaskPatterns.Pattern5, 
new bool[] {false, false, false, false, false, true,false, false, true,false, true,false, true,false, true}
 },
{ 
MaskPatterns.Pattern6, 
new bool[] {false, false, false, true,true,false, true,false, false, false, false, true,true,false, false }
 },
{ 
MaskPatterns.Pattern7, 
new bool[] {false, false, false, true,false, false, false, false, false, true,true,true,false, true,true}
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
