using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode.Encoder.StaticDataTables {

	/// <summary>
	/// Very bad solution, no checks, only 4 of 40 versions
	/// </summary>
	public static class CharacterCapacities {
		/// <summary>
		/// Version, errCorrLevel, numeric, alphanumeric, byte, kanji (accordingly)
		/// </summary>
		private static readonly int[,] DataMaxCapacityAndVersion = { { 1, 0, 41, 25, 17, 10 },
                                      { 1, 1, 34, 20, 14, 8 },
                                      { 1, 2, 27, 16, 11, 7 },
                                      { 1, 3, 17, 10, 7,  4 },
                                      { 2, 0, 77, 47, 32, 20 },
                                      { 2, 1, 63, 38, 26, 16 },
                                      { 2, 2, 48, 29, 20, 12 },
                                      { 2, 3, 34, 20, 14, 8 },
                                      { 3, 0, 127, 77, 53, 32 },
                                      { 3, 1, 101, 61, 42, 26 },
                                      { 3, 2, 77, 47, 32, 20 },
                                      { 3, 3, 58, 35, 24, 15 },
                                      { 4, 0, 187, 114, 78, 48 },
                                      { 4, 1, 149, 90, 62, 38 },
                                      { 4, 2, 111, 67, 46, 28 },
                                      { 4, 3, 82, 50, 34, 21 },
									  { -1, 0, 187, 114, 78, 48 },
                                      { -1, 1, 149, 90, 62, 38 },
                                      { -1, 2, 111, 67, 46, 28 },
                                      { 5, 3, 106, 64, 44, 27 }
                                    };
		/// <summary>
		/// Versions from, versions through, numeric, alphanumeric, byte, kanji (accordingly)
		/// </summary>
		private static readonly int[,] CharacterCountIndicatorLength = {
										{1,9,10,9,8,8},
										{10,26,12,11,16,10},
										{27,40,14,13,16,12}
									};

		/// <summary>
		/// Careful use (only 4 of 40 version enabled)
		/// </summary>
		/// <param name="str">string to encode</param>
		/// <param name="errorCorrection">0 - 7%;1 - 15%; 2 - 25%; 3 - 30%</param>
		/// <param name="mode">0- numeric; 1- alphanumeric;2- Byte; 3- Kanji;</param>
		/// <param name="version">varsion of </param> 
		/// <param name="maxCapacity">max capasity for input string</param>
		public static void GetMaxCapacityAndVersion ( string str , ErrorCorrectionLvl errorCorrection , EncodeMode mode , out int version, out int maxCapacity )
		{
			maxCapacity = 0;
			version = 0;
			while ( str.Length > maxCapacity ) {

				maxCapacity = DataMaxCapacityAndVersion[version * 4 + (int) errorCorrection , 2 + (int) mode];
				version++;
			}
		}

		public static int GetCharacterCountIndicatorLength(int version, EncodeMode mode)
		{
			var versionsPart = version >= CharacterCountIndicatorLength[0, 0] && version <= CharacterCountIndicatorLength[0, 1]
				? 0
				: version >= CharacterCountIndicatorLength[1, 0] && version <= CharacterCountIndicatorLength[1, 1]
					? 1
					: version >= CharacterCountIndicatorLength[2, 0] && version <= CharacterCountIndicatorLength[2, 1] ? 2 : -1; //probably throw exception?
			
			return CharacterCountIndicatorLength[versionsPart,2+(int)mode];
		}
	}
}
