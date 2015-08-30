using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode.Encoder.StaticDataTables {
	public static class AligmentPatternLocationTable
	{
		private static int[][] table = {null,new []{6,18},new[]{6,22},new[]{6,26},null,null,new int[]{6,22,38} };

		public static int[] GetLocations(int QRCodeVersion)
		{
			return table[QRCodeVersion - 1];
		}
	}
}
