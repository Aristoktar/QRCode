using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode.Encoder {
	public static class Helper {
		public static void joinArrs<T> ( ref T[] arr1 , T[] arr2 ) {
			int pastSize = arr1.Length;
			Array.Resize<T> ( ref arr1 , arr2.Length + arr1.Length );
			for ( int i = 0 ; i < arr2.Length ; i++ ) {
				arr1[pastSize + i] = arr2[i];
			}
		}
		public static T[] joinArrs<T> ( T[] arr1 , T[] arr2 ) {
			int pastSize = arr1.Length;
			Array.Resize<T> ( ref arr1 , arr2.Length + arr1.Length );
			for ( int i = 0 ; i < arr2.Length ; i++ ) {
				arr1[pastSize + i] = arr2[i];
			}
			return arr1;
		}
		/// <summary>
		/// Метод деления многочлена на многочлен
		/// </summary>
		/// <param name="dividend">Коэффициенты многочлена делимого, индекс в массиве - степень элемента многочлена</param>
		/// <param name="divisor">Коэффициенты многочлена делителя, индекс в массиве - степень элемента многочлена</param>
		/// <param name="quotient">Коэффициенты многочлена частного, индекс в массиве - степень элемента многочлена</param>
		/// <param name="remainder">Коэффициенты многочлена остатка, индекс в массиве - степень элемента многочлена</param>
		public static void Deconv ( double[] dividend , double[] divisor , out double[] quotient , out double[] remainder ) {
			if ( dividend.Last () == 0 ) {
				throw new ArithmeticException ( "Старший член многочлена делимого не может быть 0" );
			}
			if ( divisor.Last () == 0 ) {
				throw new ArithmeticException ( "Старший член многочлена делителя не может быть 0" );
			}
			remainder = (double[]) dividend.Clone ();
			quotient = new double[remainder.Length - divisor.Length + 1];
			for ( int i = 0 ; i < quotient.Length ; i++ ) {
				double coeff = remainder[remainder.Length - i - 1] / divisor.Last ();
				quotient[quotient.Length - i - 1] = coeff;
				for ( int j = 0 ; j < divisor.Length ; j++ ) {
					remainder[remainder.Length - i - j - 1] -= coeff * divisor[divisor.Length - j - 1];
				}
			}
		}

		public static double[] multiply ( double[] p1 , double[] p2 ) {
			int outSize = ( p1.Length - 1 ) + ( p2.Length - 1 );
			double[] ans = new double[outSize + 1];
			for ( int i = p1.Length - 1 ; i > -1 ; i-- ) {
				for ( int j = p2.Length - 1 ; j > -1 ; j-- ) {
					ans[i + j] += p1[i] * p2[j];

				}
			}
			return ans;

		}
		public static AlphaPow[] multiply ( AlphaPow[] p1 , AlphaPow[] p2 ) {
			int outSize = ( p1.Length - 1 ) + ( p2.Length - 1 );
			AlphaPow[] ans = new AlphaPow[outSize + 1];
			for ( int i = p1.Length - 1 ; i > -1 ; i-- ) {
				for ( int j = p2.Length - 1 ; j > -1 ; j-- ) {
					if ( ans[i + j] == null ) {
						ans[i + j] = new AlphaPow ();
						ans[i + j] = p1[i] * p2[j];
					}
					else {
						ans[i + j] += p1[i] * p2[j];
					}

				}
			}
			return ans;

		}
		public static AlphaPow[] generatePolinomial ( int pow ) {
			AlphaPow[] ans = new AlphaPow[2];
			ans[0] = new AlphaPow ( 0 , false );
			ans[1] = new AlphaPow ( 0 , false );
			AlphaPow[] bit = new AlphaPow[2];
			bit[1] = new AlphaPow ( 0 , false );
			for ( int i = 0 ; i < pow - 1 ; i++ ) {
				bit[0] = new AlphaPow ( i + 1 , false );
				ans = multiply ( ans , bit );
			}
			return ans;
		}
		public static int[] generateErrCorrectionCodeword ( AlphaPow[] CorrPolinom , bool[] messagePolinom ) {
			int[] messagePolinomInt = new int[messagePolinom.Length / 8];

			for ( int i = 0 ; i < messagePolinom.Length / 8 ; i++ ) {

			}

			return null;
		}

	}
}
