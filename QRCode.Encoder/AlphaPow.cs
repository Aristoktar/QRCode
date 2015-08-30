using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCode.Encoder {
	public class AlphaPow {
		public int pow;
		public AlphaPow () {
			pow = -1;
		}
		public AlphaPow ( int p , bool type ) //0-pow,1 -int
		{
			if ( !type )
				this.pow = p;
			else
				this.pow = getPow ( p );
		}

		public int getInt () {
			int ans = 1;
			for ( int i = 0 ; i < this.pow ; i++ ) {
				ans *= 2;
				if ( ans > 255 ) {
					ans = 285 ^ ans;
				}

			}
			return ans;
		}
		public static int getInt ( int p ) {
			int ans = 1;
			for ( int i = 0 ; i < p ; i++ ) {
				ans *= 2;
				if ( ans > 255 ) {
					ans = 285 ^ ans;
				}

			}
			return ans;
		}
		public static int getPow ( int p ) {
			for ( int i = 0 ; i < 256 ; i++ ) {
				if ( getInt ( i ) == p ) {
					return i;
				}
			}
			return 0;
		}
		public static AlphaPow operator * ( AlphaPow p1 , AlphaPow p2 ) {
			int ans = p1.pow + p2.pow;
			if ( ans > 255 )
				ans = ans % 255;

			return new AlphaPow ( ans , false );
		}
		public static AlphaPow operator + ( AlphaPow p1 , AlphaPow p2 ) {
			int t1 = getInt ( p1.pow );
			int t2 = getInt ( p2.pow );
			int y = t1 ^ t2;
			AlphaPow a = new AlphaPow ( y , true );
			return a;
		}
	}
}
