using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Permissions;
using QRCode.Encoder.StaticDataTables;

namespace QRCode.Encoder
{
	public enum Direction{Up,Down}
	public enum Location {Left , Right}

	public static class Encode
    {
		public static Bitmap GetImage ( EncodeProperties properties , out int QRCodeVersion , MaskPatterns maskPattern = MaskPatterns.Pattern0 )
	    {
		    if (properties == null) throw new NullReferenceException("empty properties");

		    // ReSharper disable once InconsistentNaming
			Bitmap QRCodeBitmap = new Bitmap(properties.Size,properties.Size);

		    int maxLength;
		    int version;
		    bool[] modeSequence;

		    bool[] InputData;
			int RequestedCapacity;

			bool[] Terminator;
			bool[] Additoin0s;

			bool[] FullData;
			bool[] CharacterCountIndicator;
		    int CharacterCountIndicatorLength;
	
			CharacterCapacities.GetMaxCapacityAndVersion ( properties.Text , properties.ErrorCorrectionLvl , properties.Mode, out version, out maxLength );
			CharacterCountIndicatorLength = CharacterCapacities.GetCharacterCountIndicatorLength ( version , properties.Mode );
			switch ( properties.Mode ) {
				case EncodeMode.Numeric:
					InputData = Encode.Numeric ( properties.Text );
					modeSequence = new[] {false, false, false, true};
					break;
				case EncodeMode.Alphanumeric:
					InputData = Encode.Alphanumeric ( properties.Text);
					modeSequence = new[] { false , false , true , false};
					break;
				default:
					throw new NotImplementedException();
					break;
			}
			CharacterCountIndicator = GetCharacterCountIndicator(properties.Text, CharacterCountIndicatorLength);

		    #region fill Data to Max Capacity (Terminator)

			RequestedCapacity = ErrorCorrectionTable.getTotalDataCodeBits ( (int)properties.ErrorCorrectionLvl , version );

			int missingSymbCount = RequestedCapacity - InputData.Length - CharacterCountIndicatorLength - modeSequence.Length;

			if ( missingSymbCount > 4 ) {
				Terminator = new bool[4];
				Terminator[0] = false;
				Terminator[1] = false;
				Terminator[2] = false;
				Terminator[3] = false;
				int adding = 8 - ( InputData.Length + CharacterCountIndicatorLength + modeSequence.Length + Terminator.Length ) % 8;
				if ( adding == 8 )
					adding = 0;
				Additoin0s = new bool[adding];
				for ( int i = 0 ; i < adding ; i++ ) {
					Additoin0s[i] = false;
				}
				Helper.joinArrs<bool> ( ref Terminator , Additoin0s );
				bool[] Pad1 = { true , true , true , false , true , true , false , false };
				bool[] Pad2 = { false , false , false , true , false , false , false , true };
				missingSymbCount = RequestedCapacity - InputData.Length - CharacterCountIndicatorLength - modeSequence.Length - Terminator.Length;
				while ( missingSymbCount > 0 ) {
					Helper.joinArrs<bool> ( ref Terminator , Pad1 );
					missingSymbCount = RequestedCapacity - InputData.Length - CharacterCountIndicatorLength - modeSequence.Length - Terminator.Length;
					if ( missingSymbCount > 0 ) {
						Helper.joinArrs<bool> ( ref Terminator , Pad2 );
					}
					else {
						break;
					}
					missingSymbCount = RequestedCapacity - InputData.Length - CharacterCountIndicatorLength - modeSequence.Length - Terminator.Length;
				}


			}
			else {
				Terminator = new bool[missingSymbCount];
				Additoin0s = new bool[0];
				for ( int i = 0 ; i < missingSymbCount ; i++ ) {
					Terminator[i] = false;
				}

			}
			#endregion

			FullData = modeSequence;
		    Helper.joinArrs<bool>(ref FullData, CharacterCountIndicator);
			Helper.joinArrs<bool> ( ref FullData , InputData );
			Helper.joinArrs<bool> ( ref FullData , Terminator );


			#region BLOCKING and polynomial devisions
			int numOfBlocksInGroup1 = ErrorCorrectionTable.GetNumOfBlocksInGroup1(properties.ErrorCorrectionLvl,version);
			int numOfBlocksInGroup2 =ErrorCorrectionTable.GetNumOfBlocksInGroup2(properties.ErrorCorrectionLvl,version);
			int numOfCodeWordsInBlocksInGroup1 = ErrorCorrectionTable.GetNumOfCodeWordsInBlocksInGroup1( properties.ErrorCorrectionLvl , version );
			int numOfCodeWordsInBlocksInGroup2 = ErrorCorrectionTable.GetNumOfCodeWordsInBlocksInGroup2( properties.ErrorCorrectionLvl , version );
			int numOfECCodewordsPerBlock = ErrorCorrectionTable.GetErrorCorrectionCodewordsPerBlock ( properties.ErrorCorrectionLvl , version );

			int iteratorFullData = 0;
			BlockOfCodeWords[] groupOfDataCodewords1 = null;
			BlockOfCodeWords[] groupOfDataCodewords2 = null;
			BlockOfCodeWords[] groupOfECCodewords1 = null;
			BlockOfCodeWords[] groupOfECCodewords2 = null;

			int[] FullDataInts = Encode.UnBinary ( FullData , 8 );

			
			if (numOfBlocksInGroup1 > 0)
		    {
				groupOfDataCodewords1 = new BlockOfCodeWords[numOfBlocksInGroup1];
				groupOfECCodewords1 = new BlockOfCodeWords[numOfBlocksInGroup1];
			    for ( int i = 0 ; i < numOfBlocksInGroup1 ; i++ )
			    {
					groupOfDataCodewords1[i]= new BlockOfCodeWords();
					groupOfDataCodewords1[i].CodeWords = new int[numOfCodeWordsInBlocksInGroup1];
				    for (int j = 0; j < numOfCodeWordsInBlocksInGroup1; j++)
				    {
						groupOfDataCodewords1[i].CodeWords[j] = FullDataInts[iteratorFullData];
					    iteratorFullData++;
				    }
					groupOfECCodewords1[i] = new BlockOfCodeWords();
				    groupOfECCodewords1[i].CodeWords = PolynomialDevision(properties.ErrorCorrectionLvl, version,
					    groupOfDataCodewords1[i].CodeWords);

			    }
		    }
			if ( numOfBlocksInGroup2 > 0 ) {
				groupOfDataCodewords2 = new BlockOfCodeWords[numOfBlocksInGroup2];
				groupOfECCodewords2 = new BlockOfCodeWords[numOfBlocksInGroup2];
				for ( int i = 0 ; i < numOfBlocksInGroup2 ; i++ ) {
					groupOfDataCodewords2[i]=new BlockOfCodeWords();
					groupOfDataCodewords2[i].CodeWords = new int[numOfCodeWordsInBlocksInGroup2];
					for ( int j = 0 ; j < numOfCodeWordsInBlocksInGroup2 ; j++ ) {
						groupOfDataCodewords2[i].CodeWords[j] = FullDataInts[iteratorFullData];
						iteratorFullData++;
					}
					groupOfECCodewords2[i]=new BlockOfCodeWords();
					groupOfECCodewords2[i].CodeWords = PolynomialDevision(properties.ErrorCorrectionLvl, version,
						groupOfDataCodewords2[i].CodeWords);
				}
			}
			int[] FullMessageData = new int[FullDataInts.Length];
		    int[] FullMessageEC =
			    new int[
				    groupOfDataCodewords2 == null
					    ? numOfBlocksInGroup1*numOfECCodewordsPerBlock
					    : (numOfBlocksInGroup1 + numOfBlocksInGroup2)*numOfECCodewordsPerBlock];

		    int iteratorFullMessageData = 0;
			int iteratorFullMessageEC = 0;
		    int forMam = numOfCodeWordsInBlocksInGroup2 > 0 ? numOfCodeWordsInBlocksInGroup2 : numOfCodeWordsInBlocksInGroup1;
			for ( int i = 0 ; i <forMam ; i++ ) {
				if ( groupOfDataCodewords1 != null && numOfCodeWordsInBlocksInGroup1 > i) {
					for (int j = 0; j < numOfBlocksInGroup1; j++)
					{
						FullMessageData[iteratorFullMessageData] = groupOfDataCodewords1[j].CodeWords[i];
						iteratorFullMessageData++;
					}
				}
				if ( groupOfDataCodewords2 != null ) {
					for ( int j = 0 ; j < numOfBlocksInGroup2 ; j++ ) {
						FullMessageData[iteratorFullMessageData] = groupOfDataCodewords2[j].CodeWords[i];
						iteratorFullMessageData++;
					}
				} 
			}
			for ( int i = 0 ; i < numOfECCodewordsPerBlock ; i++ ) {
				if ( groupOfECCodewords1 != null ) {
					for ( int j = 0 ; j < numOfBlocksInGroup1 ; j++ ) {
						FullMessageEC[iteratorFullMessageEC] = groupOfECCodewords1[j].CodeWords[i];
						iteratorFullMessageEC++;
					}
				}
				if ( groupOfECCodewords2 != null ) {
					for ( int j = 0 ; j < numOfBlocksInGroup2 ; j++ ) {
						FullMessageEC[iteratorFullMessageEC] = groupOfECCodewords2[j].CodeWords[i];
						iteratorFullMessageEC++;
					}
				}
			}
		    var LastFullMessageInts = Helper.joinArrs(FullMessageData, FullMessageEC);
			var LastFullMessageBools = new bool[0];
			for ( int i = 0 ; i < LastFullMessageInts.Length ; i++ ) {
				Helper.joinArrs<bool> ( ref LastFullMessageBools , Encode.Binary ( LastFullMessageInts[i] ) );
			}
		    #endregion

		    

			//var XORresult = PolynomialDevision ( properties.ErrorCorrectionLvl , version , FullDataInts );
			//var ErrCorrKeyWord = new bool[0];
			//for ( int i = 0 ; i < XORresult.Length ; i++ ) {
			//	Helper.joinArrs<bool> ( ref ErrCorrKeyWord , Encode.Binary ( XORresult[i] ) );
			//}
			QRCodeVersion = version;
			return Generate ( version , CharacterCountIndicator , InputData , Terminator , modeSequence , LastFullMessageBools , 5 , properties.ErrorCorrectionLvl,maskPattern);
	    }
		private static bool[] GetCharacterCountIndicator ( string text , int CharacterCountIndicatorLength)
	    {
		    bool[] CharacterCountIndicator;
		    bool[] CharacterCountIndicatorTemp = Encode.Binary(text.Length);

		    CharacterCountIndicator = new bool[CharacterCountIndicatorLength];

		    for (int i = 0; i < CharacterCountIndicatorLength; i++)
		    {
			    if (i < CharacterCountIndicatorLength - CharacterCountIndicatorTemp.Length)
			    {
				    CharacterCountIndicator[i] = false;
			    }
			    else
			    {
				    CharacterCountIndicator[i] =
					    CharacterCountIndicatorTemp[i - (CharacterCountIndicatorLength - CharacterCountIndicatorTemp.Length)];
			    }
		    }
		    return CharacterCountIndicator;
	    }
	    private static bool[] Alphanumeric(string str)
	    {
			string[] AlphanumericValues = {"0","1","2","3","4","5","6","7","8","9",
                                                      "A","B","C","D","E","F","G","H","I","J",
                                                      "K","L","M","N","O","P","Q","R","S","T",
                                                      "U","V","W","X","Y","Z"," ","$","%","*",
                                                      "+","-",".","/",":"};

			bool[] ans = new bool[0];
			int t1;
			int t2;
			int temp;
			int len = 11;
			bool[] tempArr = new bool[len];
			for ( int i = 0 ; i < str.Length ; i += 2 ) {
				if ( i < str.Length - 2 ) {
					t1 = Array.IndexOf ( AlphanumericValues , str.Substring ( i , 1 ) );
					t2 = Array.IndexOf ( AlphanumericValues , str.Substring ( i + 1 , 1 ) );
					temp = ( 45 * t1 ) + t2;
					tempArr = Binary ( temp , len );
					Helper.joinArrs<bool> ( ref ans , tempArr );
				}
				else {
					if ( str.Length % 2 == 1 ) {
						t1 = Array.IndexOf ( AlphanumericValues , str.Substring ( i , 1 ) );
						tempArr = Binary ( t1 , 6 );
						Helper.joinArrs<bool> ( ref ans , tempArr );
					}
					else {
						t1 = Array.IndexOf ( AlphanumericValues , str.Substring ( i , 1 ) );
						t2 = Array.IndexOf ( AlphanumericValues , str.Substring ( i + 1 , 1 ) );
						temp = ( 45 * t1 ) + t2;
						tempArr = Binary ( temp , len );
						Helper.joinArrs<bool> ( ref ans , tempArr );

					}
				}
			}
			return ans;
	    }
		public static bool[] Numeric ( string str ) {
			bool[] ans = new bool[0];
			int t1;
			int len = 11;
			bool[] tempArr = new bool[len];
			for ( int i = 0 ; i < str.Length ; i += 3 ) {
				if ( i < str.Length - 3 ) {
					t1 = Convert.ToInt32 ( str.Substring ( i , 3 ) );
					tempArr = Binary ( t1 , 8 );
					Helper.joinArrs<bool> ( ref ans , tempArr );
				}
				else {

					if ( str.Length % 3 == 1 ) {
						t1 = Convert.ToInt32 ( str.Substring ( i , 1 ) );
						tempArr = Binary ( t1 , 8 );
						Helper.joinArrs<bool> ( ref ans , tempArr );
						continue;
					}
					if ( str.Length % 3 == 2 ) {
						t1 = Convert.ToInt32 ( str.Substring ( i , 2 ) );
						tempArr = Binary ( t1 , 8 );
						Helper.joinArrs<bool> ( ref ans , tempArr );
						continue;
					}
				}

			}
			return ans;

		}
		private static bool[] Binary ( int src ) {
			bool[] ans;
			int Src = Convert.ToInt32 ( Convert.ToString ( src , 2 ) );
			string sss = Convert.ToString ( src , 2 );
			ans = new bool[8];
			int delta = 8 - sss.Length;
			for ( int i = 0 ; i < delta ; i++ )
				ans[i] = false;


			for ( int i = 0 ; i < sss.Length ; i++ ) {
				if ( sss.Substring ( i , 1 ) == "0" ) {
					ans[delta + i] = false;
				}
				else {
					ans[delta + i] = true;
				}
			}
			return ans;

		}
		private static bool[] Binary ( int src , int length ) {
			bool[] ansTemp;
			bool[] ans;
			//int Src = Convert.ToInt32(Convert.ToString(src, 2));
			string sss = Convert.ToString ( src , 2 );
			ansTemp = new bool[sss.Length];
			for ( int i = 0 ; i < sss.Length ; i++ ) {
				if ( sss.Substring ( i , 1 ) == "0" ) {
					ansTemp[i] = false;
				}
				else {
					ansTemp[i] = true;
				}
			}
			ans = new bool[length];
			for ( int i = 0 ; i < length ; i++ ) {
				if ( i < length - ansTemp.Length ) {
					ans[i] = false;
				}
				else {
					ans[i] = ansTemp[i - ( length - ansTemp.Length )];
				}
			}
			return ans;
		}
		private static int[] UnBinary ( bool[] src , int separator ) {
			int[] ans = new int[src.Length / separator];
			for ( int i = 0 ; i < src.Length / separator ; i++ ) {
				string Symb = "";
				for ( int j = 0 ; j < separator ; j++ ) {
					if ( src[i * separator + j] )
						Symb += "1";
					else
						Symb += "0";
				}
				ans[i] = Convert.ToInt32 ( Symb , 2 );
			}
			return ans;
		}
		private static int[] PolynomialDevision ( ErrorCorrectionLvl ErrCorr , int version , int[] FullDataInts )
	    {

	    
			int ErrCorrectionCodewordsCount;
			int[] MessagePolynomial;
			AlphaPow[] GeneratorPolinomial;
			int DevisionSteps;
			int[] GeneratorPolinomialInt;
			int[] XORresult;
			ErrCorrectionCodewordsCount = ErrorCorrectionTable.GetErrorCorrectionCodewordsPerBlock ( ErrCorr , version );
			MessagePolynomial = FullDataInts;
			GeneratorPolinomial = Helper.generatePolinomial ( ErrCorrectionCodewordsCount );

			Array.Reverse ( GeneratorPolinomial );
			DevisionSteps = MessagePolynomial.Length;//
			int[] mult = new int[ErrCorrectionCodewordsCount];//DevisionSteps->ErrCorrectionCodewordsCount
			MessagePolynomial = Helper.joinArrs<int> ( MessagePolynomial , mult );
			AlphaPow[] mult1 = new AlphaPow[MessagePolynomial.Length - GeneratorPolinomial.Length];
			for ( int i = 0 ; i < mult1.Length ; i++ )
				mult1[i] = new AlphaPow ( -1 , false );
			GeneratorPolinomial = Helper.joinArrs<AlphaPow> ( GeneratorPolinomial , mult1 );
			GeneratorPolinomialInt = new int[GeneratorPolinomial.Length];
			bool[][] Codewords = new bool[DevisionSteps][];

			//first step
			AlphaPow[] GenPolRes = new AlphaPow[GeneratorPolinomial.Length];
			Array.Copy ( GeneratorPolinomial , GenPolRes , GeneratorPolinomial.Length );
			for ( int j = 0 ; j < GeneratorPolinomial.Length ; j++ ) {
				if ( GenPolRes[j].pow == -1 ) {
					GeneratorPolinomialInt[j] = 0;
				}
				else {
					AlphaPow a = new AlphaPow ( AlphaPow.getPow ( MessagePolynomial[0] ) , false );
					GenPolRes[j] *= a;
					GeneratorPolinomialInt[j] = GenPolRes[j].getInt ();
					//GeneratorPolinomial[j] = new AlphaPow( AlphaPow.getPow( GeneratorPolinomialInt[j] ^ MessagePolynomial[j]),false);
				}
			}
			XORresult = new int[GeneratorPolinomial.Length];
			for ( int j = 0 ; j < GeneratorPolinomial.Length ; j++ ) {
				XORresult[j] = GeneratorPolinomialInt[j] ^ MessagePolynomial[j];
			}

			if ( XORresult[0] == 0 ) {
				int[] tempIntArr = new int[XORresult.Length - 1];
				Array.Copy ( XORresult , 1 , tempIntArr , 0 , tempIntArr.Length );
				XORresult = new int[tempIntArr.Length];
				Array.Copy ( tempIntArr , XORresult , tempIntArr.Length );
			}
			AlphaPow[] tempArr = new AlphaPow[GeneratorPolinomial.Length - 1];
			Array.Copy ( GeneratorPolinomial , 0 , tempArr , 0 , GeneratorPolinomial.Length - 1 );
			GeneratorPolinomial = new AlphaPow[tempArr.Length];
			Array.Copy ( tempArr , GeneratorPolinomial , tempArr.Length );
			//////////////////


			bool[][] DataCodewords = new bool[DevisionSteps][];
			DataCodewords[0] = new bool[8];
			bool[] tempT = Encode.Binary ( 
				XORresult[0] );

			for ( int up = 0 ; up < 8 ; up++ )
				DataCodewords[0][up] = tempT[up];
			Array.Copy ( tempT , DataCodewords[0] , 8 );

			for ( int i = 1 ; i < DevisionSteps ; i++ ) {
				DataCodewords[i] = new bool[8];
				//Array.Copy(Encoder.Binary(XORresult[0]), DataCodewords[i], 8);
				for ( int up = 0 ; up < 8 ; up++ )
					DataCodewords[0][up] = Encode.Binary ( XORresult[0] )[up];
				Array.Copy ( GeneratorPolinomial , GenPolRes , GeneratorPolinomial.Length );
				for ( int j = 0 ; j < GeneratorPolinomial.Length ; j++ ) {
					if ( GenPolRes[j].pow == -1 ) {
						GeneratorPolinomialInt[j] = 0;
					}
					else {
						AlphaPow a = new AlphaPow ( AlphaPow.getPow ( XORresult[0] ) , false );
						GenPolRes[j] *= a;
						GeneratorPolinomialInt[j] = GenPolRes[j].getInt ();
						//GeneratorPolinomial[j] = new AlphaPow( AlphaPow.getPow( GeneratorPolinomialInt[j] ^ MessagePolynomial[j]),false);
					}
				}
				int[] XORtemp = new int[XORresult.Length];
				Array.Copy ( XORresult , XORtemp , XORresult.Length );
				XORresult = new int[GeneratorPolinomial.Length];

				for ( int j = 0 ; j < GeneratorPolinomial.Length ; j++ ) {
					XORresult[j] = GeneratorPolinomialInt[j] ^ XORtemp[j];
				}

				if ( XORresult[0] == 0 ) {
					int[] tempIntArr = new int[XORresult.Length - 1];
					Array.Copy ( XORresult , 1 , tempIntArr , 0 , tempIntArr.Length );
					XORresult = new int[tempIntArr.Length];
					Array.Copy ( tempIntArr , XORresult , tempIntArr.Length );
				}
				tempArr = new AlphaPow[GeneratorPolinomial.Length - 1];
				Array.Copy ( GeneratorPolinomial , 0 , tempArr , 0 , GeneratorPolinomial.Length - 1 );
				GeneratorPolinomial = new AlphaPow[tempArr.Length];
				Array.Copy ( tempArr , GeneratorPolinomial , tempArr.Length );
			}

			int size = XORresult.Length;
			//for ( int i = 0 ; i < size ; i++ ) {
			//	if ( XORresult[i] == 0 ) {
			//		int[] Xtemp = new int[i];
			//		Array.Copy ( XORresult , 0 , Xtemp , 0 , i );
			//		XORresult = new int[Xtemp.Length];
			//		Array.Copy ( Xtemp , XORresult , Xtemp.Length );
			//		break;
			//	}
			//}
			//////////////////
			for ( int i = size - 1 ; i >= 0 ; i-- ) {
				if ( XORresult[i] == 0 ) {
					int[] Xtemp = new int[i];
					Array.Copy ( XORresult , 0 , Xtemp , 0 , i );
					XORresult = new int[Xtemp.Length];
					Array.Copy ( Xtemp , XORresult , Xtemp.Length );
				}
				else {
					break;

				}
			}
			return XORresult;
		}
		private static void ApplyMask(int size,MaskPatterns maskPattern, ref bool[,] matrix, Color[,] matrixOfFilled)
		{
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if ( matrixOfFilled[i , j] == Color.Aquamarine ) {
						switch (maskPattern)
						{
							case MaskPatterns.Pattern0:
								if ((i + j)%2 == 0)
									matrix[i, j] = !matrix[i, j];
								break;
							case MaskPatterns.Pattern1:
								if (i%2 == 0)
									matrix[i, j] = !matrix[i, j];
								break;
							case MaskPatterns.Pattern2:
								if ( j % 2 == 0 )
									matrix[i , j] = !matrix[i , j];
								break;
							case MaskPatterns.Pattern3:
								if ( ( i + j ) % 3 == 0 )
									matrix[i , j] = !matrix[i , j];
								break;
							default:
								throw new NotImplementedException();
								break;
								
						}
					}
				}
			}
		}
		public static Bitmap DrawBitmap(int version, bool[] data, bool[] formatInfoData, int resolutionSize, MaskPatterns maskPattern = MaskPatterns.Pattern0)
		{
			int size = 21 + (version - 1)*4;
			int thickness = resolutionSize/size;
			if (thickness < 1) throw new ArgumentException("bad resolution");
			Color[,] matrixOfFilled = new Color[size, size];
			bool[,] matrixOfData = new bool[size, size];

			#region Alligment patterns

			#region Patterns

			int[,] ttt1 =
			{
				{1, 1, 1, 1, 1, 1, 1},
				{1, 0, 0, 0, 0, 0, 1},
				{1, 0, 1, 1, 1, 0, 1},
				{1, 0, 1, 1, 1, 0, 1},
				{1, 0, 1, 1, 1, 0, 1},
				{1, 0, 0, 0, 0, 0, 1},
				{1, 1, 1, 1, 1, 1, 1}
			};


			int[,] ttt4 =
			{
				{1, 1, 1, 1, 1},
				{1, 0, 0, 0, 1},
				{1, 0, 1, 0, 1},
				{1, 0, 0, 0, 1},
				{1, 1, 1, 1, 1}
			};

			#endregion

			for (int i = 0; i < 7; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					matrixOfData[i, j] = ttt1[i, j] == 1;
					matrixOfData[size - 7 + i, j] = ttt1[i, j] == 1;
					matrixOfData[i, size - 7 + j] = ttt1[i, j] == 1;
					matrixOfFilled[i, j] = Color.Black;
					matrixOfFilled[size - 7 + i, j] = Color.Black;
					matrixOfFilled[i, size - 7 + j] = Color.Black;

				}
			}
			for (int i = 0; i < 8; i++)
			{
				matrixOfFilled[i, 7] = Color.Aqua;
				matrixOfFilled[7, i] = Color.Aqua;
				matrixOfFilled[i, size - 8] = Color.Aqua;
				matrixOfFilled[size - 8, i] = Color.Aqua;
				matrixOfFilled[7, size - i - 1] = Color.Aqua;
				matrixOfFilled[size - i - 1, 7] = Color.Aqua;
			}



			var patternsLocation = AligmentPatternLocationTable.GetLocations(version);
			if (patternsLocation != null)
			{
				for (int i = 0; i < patternsLocation.Length; i++)
				{
					for (int j = 0; j < patternsLocation.Length; j++)
					{
						if (matrixOfFilled[patternsLocation[i], patternsLocation[j]].IsEmpty)
						{
//?

							for (int k = patternsLocation[i] - 2; k <= patternsLocation[i] + 2; k++)
							{
								for (int l = patternsLocation[j] - 2; l <= patternsLocation[j] + 2; l++)
								{
									matrixOfData[k, l] =
										ttt4[l - patternsLocation[j] + 2, k - patternsLocation[i] + 2] == 1;
									matrixOfFilled[k, l] = Color.Coral;
								}
							}
						}
					}
				}
			}

			#endregion

			#region Timing patterns

			for (int i = 8; i < size - 8; i++)
			{
				if (i%2 == 0)
				{
					matrixOfData[6, i] = true;
					matrixOfData[i, 6] = true;
				}
				else
				{
					matrixOfData[6, i] = false;
					matrixOfData[6, i] = false;
				}
				matrixOfFilled[6, i] = Color.Red;
				matrixOfFilled[i, 6] = Color.Red;
			}

			#endregion

			#region Reserved area

			if (version < 7)
			{

				for (int i = 0; i < 8; i++)
				{
					matrixOfFilled[i, 8] = Color.Blue;
					matrixOfFilled[8, i] = Color.Blue;
					matrixOfFilled[8, size - i - 1] = Color.Blue;
					matrixOfFilled[size - i - 1, 8] = Color.Blue;
				}
				matrixOfFilled[8, 8] = Color.Blue;
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 6; j++)
					{
						matrixOfFilled[size - 9 - i, j] = Color.Blue;
						matrixOfFilled[j, size - 9 - i] = Color.Blue;
					}

				}
			}


			#endregion

			matrixOfFilled[8, size - 8] = Color.DeepPink; //dark module
			matrixOfData[8, size - 8] = true;

			#region filling with data

			Point point = new Point(size - 1, size - 1);
			Direction direction = Direction.Up;
			Location location = Location.Right;

			int lasttt = 0;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (matrixOfFilled[i, j].IsEmpty)
					{
						lasttt++;
					}
				}
			}
			for (int i = 0; i < data.Length; i++)
			{
				if (matrixOfFilled[point.X, point.Y].IsEmpty)
				{
					matrixOfData[point.X, point.Y] = data[i];
					matrixOfFilled[point.X, point.Y] = Color.Aquamarine;
				}
				else
				{
					i--;
				}
				if (direction == Direction.Up && location == Location.Left)
				{
					point.Y--;

				}
				if (direction == Direction.Down && location == Location.Left)
				{
					point.Y++;
				}
				if (location == Location.Right)
				{
					point.X--;
					location = Location.Left;
				}
				else
				{
					point.X++;
					location = Location.Right;
				}
				if (point.Y == -1)
				{
					direction = Direction.Down;
					location = Location.Right;
					point.Y = 0;
					point.X -= 2;
				}
				if (point.Y == size)
				{
					direction = Direction.Up;
					location = Location.Right;
					point.Y = size - 1;
					point.X -= 2;
				}
				if (point.X == 6)
				{
					point.X = 5;
				}

			}

			#endregion

			ApplyMask(size, maskPattern,ref matrixOfData, matrixOfFilled);

			#region Format placing

			if (version < 7)
			{
				for (int i = 0; i < 6; i++)
				{
					matrixOfData[8, i] = formatInfoData[i];
					matrixOfData[size - 1 - i, 8] = formatInfoData[i];
				}
				matrixOfData[8 , 7] = formatInfoData[6];
				matrixOfData[8 , 8] = formatInfoData[7];
				matrixOfData[7 , 8] = formatInfoData[8];
				matrixOfData[size - 7 , 8] = formatInfoData[6];
				matrixOfData[size - 8 , 8] = formatInfoData[7];
				matrixOfData[8 , size-7] = formatInfoData[8];
				for (int i = 9; i < 15; i++)
				{
					matrixOfData[5-(i-9) , 8] = formatInfoData[i];
					matrixOfData[8 , size-6+(i-9)] = formatInfoData[i];
				}
			}

			#endregion
			#region Drawing image
			Bitmap outputBitmap = new Bitmap ( size * thickness + thickness * 10 , size * thickness + thickness * 10 );
			Graphics g = Graphics.FromImage ( outputBitmap );
			for ( int i = 0 ; i < size + 10 ; i++ ) {
				for ( int j = 0 ; j < size + 10 ; j++ ) {
					if ( i < 5 || i > size + 4 || j < 5 || j > size + 4 ) {
						g.FillRectangle ( Brushes.Beige , i * thickness , j * thickness , thickness , thickness );
					}
				}
			}

			for ( int i = 5 ; i < size + 5 ; i++ ) {
				for ( int j = 5 ; j < size + 5 ; j++ ) {
					//bool temp = matrix[i-5,j-5];
					Color color = matrixOfFilled[i - 5 , j - 5];
					g.FillRectangle ( new SolidBrush ( color ) , i * thickness , j * thickness , thickness , thickness );

					if ( matrixOfData[i - 5 , j - 5] ) {
						g.FillRectangle ( Brushes.Black , i * thickness , j * thickness , thickness , thickness );

					}
					else {
						g.FillRectangle ( Brushes.White , i * thickness , j * thickness , thickness , thickness );

					}
					

				}
			}
		    g.Save();
			#endregion
			
		    return outputBitmap;
	    }
	    public static Bitmap Generate ( int version , bool[] characterCountIndicator , bool[] messageData , bool[] terminator , bool[] datatype , bool[] errCorrKeyWords , int thickness,ErrorCorrectionLvl errorCorrectionLvl,MaskPatterns maskPattern = MaskPatterns.Pattern0 ) {
			Bitmap ans = new Bitmap ( ( 21 + version * 4 ) * thickness + thickness * 10 , ( 21 + version * 4 ) * thickness + thickness * 10 );
			Graphics g = Graphics.FromImage ( ans );
			int pixel = 0;
			bool[] fullData = new bool[0];
		    var formatInfoData = FormatInfoStringsList.GetInfoString(version, errorCorrectionLvl, maskPattern);
			return DrawBitmap ( version , errCorrKeyWords , formatInfoData ,1000);
		}
    }
}
