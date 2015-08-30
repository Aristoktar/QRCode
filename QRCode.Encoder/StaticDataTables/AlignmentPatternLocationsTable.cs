using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode.Encoder.StaticDataTables {
	public static class AlignmentPatternLocationsTable {
		public static int[,] table = { { 0, 0, 0 }, { 6, 18, 0 },
                                { 6, 22, 0 }, { 6, 26, 0 },
                                { 6, 30, 0 }, { 6, 34, 0 },
                                { 6, 22, 28 }, {6, 24, 42} };

		public static int getAlignment ( int version ) {

			return table[version , 1];
		}
	}
}
