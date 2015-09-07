using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode.Encoder.StaticDataTables {
	public static class AligmentPatternLocationTable
	{
		private static int[][] table = {null,
new []{ 6, 18  },
new []{ 6, 22  },
new []{ 6, 26  },
new []{ 6, 30  },
new []{ 6, 34  },
new []{ 6, 22, 38  },
new []{ 6, 24, 42  },
new []{ 6, 26, 46  },
new []{ 6, 28, 50  },
new []{ 6, 30, 54  },
new []{ 6, 32, 58  },
new []{ 6, 34, 62  },
new []{ 6, 26, 46, 66  },
new []{ 6, 26, 48, 70  },
new []{ 6, 26, 50, 74  },
new []{ 6, 30, 54, 78  },
new []{ 6, 30, 56, 82  },
new []{ 6, 30, 58, 86  },
new []{ 6, 34, 62, 90  },
new []{ 6, 28, 50, 72, 94  },
new []{ 6, 26, 50, 74, 98  },
new []{ 6, 30, 54, 78, 102  },
new []{ 6, 28, 54, 80, 106  },
new []{ 6, 32, 58, 84, 110  },
new []{ 6, 30, 58, 86, 114  },
new []{ 6, 34, 62, 90, 118  },
new []{ 6, 26, 50, 74, 98, 122  },
new []{ 6, 30, 54, 78, 102, 126  },
new []{ 6, 26, 52, 78, 104, 130  },
new []{ 6, 30, 56, 82, 108, 134  },
new []{ 6, 34, 60, 86, 112, 138  },
new []{ 6, 30, 58, 86, 114, 142  },
new []{ 6, 34, 62, 90, 118, 146  },
new []{ 6, 30, 54, 78, 102, 126, 150 },
new []{ 6, 24, 50, 76, 102, 128, 154 },
new []{ 6, 28, 54, 80, 106, 132, 158 },
new []{ 6, 32, 58, 84, 110, 136, 162 },
new []{ 6, 26, 54, 82, 110, 138, 166 },
new []{ 6, 30, 58, 86, 114, 142, 170 }
 };

		public static int[] GetLocations(int QRCodeVersion)
		{
			return table[QRCodeVersion - 1];
		}
	}
}
