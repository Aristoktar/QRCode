using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode.Encoder.StaticDataTables {
	public static class ErrorCorrectionTable {
		/*version,
		 * level,(datatype)
		 * Total Number of Data Codewords for this Version and EC Level,(var1)
		 * EC Codewords Per Block,                                      (var2)
		 * Number of Blocks in Group 1,                                 (var3)
		 * Number of Data Codewords in Each of Group 1's Blocks         (var4)
		 * Number of Blocks in Group 2                                  (var5)
		 * Number of Data Codewords in Each of Group 2's Blocks         (var6)
		 * 
		 */

		public static int[,] table = { { 1, 0, 19,  7, 1, 19, 0, 0 },
                                       { 1, 1, 16, 10, 1, 16, 0, 0 },
                                       { 1, 2, 13, 13, 1, 13, 0, 0 },
                                       { 1, 3,  9, 17, 1,  9, 0, 0 },
                                       { 2, 0, 34, 10, 1, 34, 0, 0 },
                                       { 2, 1, 28, 16, 1, 28, 0, 0 },
                                       { 2, 2, 22, 22, 1, 22, 0, 0 },
                                       { 2, 3, 16, 28, 1, 16, 0, 0 },
                                       { 3, 0, 55, 15, 1, 55, 0, 0 },
                                       { 3, 1, 44, 26, 1, 44, 0, 0 },
                                       { 3, 2, 34, 18, 2, 17, 0, 0 },
                                       { 3, 3, 26, 22, 2, 13, 0, 0 },
                                       { 4, 0, 80, 20, 1, 80, 0, 0 },
                                       { 4, 1, 64, 18, 2, 32, 0, 0 },
                                       { 4, 2, 48, 26, 2, 24, 0, 0 },
                                       { 4, 3, 36, 16, 4,  9, 0, 0 },
									   { 5, 0, 108, 26, 1, 108, 0, 0 },
                                       { 5, 1, 86, 24, 2, 43, 0, 0 },
                                       { 5, 2, 62, 18, 2, 15, 2, 16 },
                                       { 5, 3, 46, 22, 2,  11, 2, 12 }
                                        };
		public static int getTotalDataCodeBits ( int errCorrection , int version ) {
			version--;
			//total = (var4*var3 + var6*var5) * 8 bit;
			int ans = table[version * 4 + errCorrection , 5] * table[version * 4 + errCorrection , 4]
					+ table[version * 4 + errCorrection , 7] * table[version * 4 + errCorrection , 6];
			return ans * 8;

		}
		public static int GetErrorCorrectionCodewordsPerBlock ( ErrorCorrectionLvl errorCorrectionLvl , int version ) {
			version--;
			return table[version * 4 + (int)errorCorrectionLvl , 3];
		}

		public static int GetNumOfBlocksInGroup1(ErrorCorrectionLvl errorCorrectionLvl, int version)
		{
			version--;
			return table[version * 4 + (int)errorCorrectionLvl , 4];
		}

		public static int GetNumOfBlocksInGroup2 ( ErrorCorrectionLvl errorCorrectionLvl , int version ) {
			version--;
			return table[version * 4 + (int) errorCorrectionLvl , 6];
		}

		public static int GetNumOfCodeWordsInBlocksInGroup1 ( ErrorCorrectionLvl errorCorrectionLvl , int version ) {
			version--;
			return table[version * 4 + (int) errorCorrectionLvl , 5];
		}

		public static int GetNumOfCodeWordsInBlocksInGroup2 ( ErrorCorrectionLvl errorCorrectionLvl , int version ) {
			version--;
			return table[version * 4 + (int) errorCorrectionLvl , 7];
		}
		
	}
}
