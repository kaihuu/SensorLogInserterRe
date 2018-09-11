using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Constant
{
    class Coordinate
    {
        public class Ynu
        {
            public static readonly double LatitudeStart = 35.47;
            public static readonly double LatitudeEnd = 35.476;
            public static readonly double LongitudeStart = 139.58;
            public static readonly double LongitudeEnd = 139.60;
        }

        public class TommyHome
        {
            public static readonly double LatitudeStart = 35.43;
            public static readonly double LatitudeEnd = 35.435;
            public static readonly double LongitudeStart = 139.40;
            public static readonly double LongitudeEnd = 139.42;
        }

        public class MoriHome
        {
            public static readonly double LatitudeStart = 35.527;
            public static readonly double LatitudeEnd = 35.538;
            public static readonly double LongitudeStart = 139.428;
            public static readonly double LongitudeEnd = 139.443;
        }

        public class TamuraHomeBefore
        {
            public static readonly double LatitudeStart = 35.58;
            public static readonly double LatitudeEnd = 35.59;
            public static readonly double LongitudeStart = 139.65;
            public static readonly double LongitudeEnd = 139.668;
            public static readonly DateTime EndDate = DateTime.Parse("2013-09-10");
        }

        public class TamuraHomeAfter
        {
            public static readonly double LatitudeStart = 35.342;
            public static readonly double LatitudeEnd = 35.35;
            public static readonly double LongitudeStart = 139.51;
            public static readonly double LongitudeEnd = 139.525;
            public static readonly DateTime StartDate = DateTime.Parse("2013-09-10");
        }

        public class AyaseCityHall
        {
            public static readonly double LatitudeStart = 35.43;
            public static readonly double LatitudeEnd = 35.445;
            public static readonly double LongitudeStart = 139.42;
            public static readonly double LongitudeEnd = 139.435;
            //public static readonly DateTime StartDate = DateTime.Parse("2013-11-22");
            //public static readonly DateTime EndDate = DateTime.Parse("2013-11-23");
            //public static readonly int SensorId = 14;
        }

        public class UemuraHome
        {
            public static readonly double LatitudeStart = 35.517;
            public static readonly double LatitudeEnd = 35.522;
            public static readonly double LongitudeStart = 139.68;
            public static readonly double LongitudeEnd = 139.685;
        }

        /***北海道走行実験インサート用***/

        public class OaraiTerminal　//大洗フェリーターミナル
        {
            public static readonly double LatitudeStart = 36.309;
            public static readonly double LatitudeEnd = 36.319;
            public static readonly double LongitudeStart = 140.575;
            public static readonly double LongitudeEnd = 140.585;
        }

        public class TomakomaiTerminal　//苫小牧フェリーターミナル
        {
            public static readonly double LatitudeStart = 42.635;
            public static readonly double LatitudeEnd = 42.645;
            public static readonly double LongitudeStart = 141.63;
            public static readonly double LongitudeEnd = 141.64;
        }

        public class RoytonSapporo　//ロイトン札幌ホテル
        {
            public static readonly double LatitudeStart = 43.06;
            public static readonly double LatitudeEnd = 43.07;
            public static readonly double LongitudeStart = 141.339;
            public static readonly double LongitudeEnd = 141.349;
        }

        public class HotelTaisetsu　//ホテル大雪
        {
            public static readonly double LatitudeStart = 43.715;
            public static readonly double LatitudeEnd = 43.725;
            public static readonly double LongitudeStart = 142.945;
            public static readonly double LongitudeEnd = 142.955;
        }

        public class Oehonke　//大江本家（ホテル）
        {
            public static readonly double LatitudeStart = 43.755;
            public static readonly double LatitudeEnd = 43.765;
            public static readonly double LongitudeStart = 143.508;
            public static readonly double LongitudeEnd = 143.518;
        }

        public class KKRKawayu　//KKRかわゆ（ホテル）
        {
            public static readonly double LatitudeStart = 43.635;
            public static readonly double LatitudeEnd = 43.645;
            public static readonly double LongitudeStart = 144.435;
            public static readonly double LongitudeEnd = 144.445;
        }

        public class MichinoEkiMashu　//道の駅摩周温泉
        {
            public static readonly double LatitudeStart = 43.492;
            public static readonly double LatitudeEnd = 43.502;
            public static readonly double LongitudeStart = 144.445;
            public static readonly double LongitudeEnd = 144.455;
        }

        /***箱根走行ログインサート用***/
        public class ToujiIkkyu　//かよい湯治一休
        {
            public static readonly double LatitudeStart = 35.219;
            public static readonly double LatitudeEnd = 35.229;
            public static readonly double LongitudeStart = 139.084;
            public static readonly double LongitudeEnd = 139.094;
        }

        /***銚子走行実験インサート用***/

        public class Inubozaki　//犬吠埼灯台
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }

		/***シミュレーションデータインサート用***/




		//		public class ShimoSeyaNichome//outward用出発地点
		//		{
		//            public static readonly double LatitudeEnd = 35.43454338;
		//			public static readonly double LatitudeStart = 35.43254338;
		//			public static readonly double LongitudeEnd = 139.5547467;
		//			public static readonly double LongitudeStart = 139.5527467;
		//		}


		//public class ShimoSeyaNichome//homeward用全部＆横浜新道のみ出発地点
		//{
		//	public static readonly double LatitudeEnd = 35.46589053;
		//	public static readonly double LatitudeStart = 35.46389053;
		//	public static readonly double LongitudeEnd = 139.5918231;
		//	public static readonly double LongitudeStart = 139.5898231;
		//}

		public class ShimoSeyaNichome//homeward用保土ヶ谷BPのみ
		{
			public static readonly double LatitudeEnd = 35.45232953;
			public static readonly double LatitudeStart = 35.45032953;
			public static readonly double LongitudeEnd = 139.5681481;
			public static readonly double LongitudeStart = 139.5661481;
		}



		/*public class S02S
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }

        public class S03S
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }

        public class S04S
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }

        public class S05S
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }

        public class S06S
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }

        public class S07S
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }

        public class S08S
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }
        

        public class S09S
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }

        public class S10S
        {
            public static readonly double LatitudeStart = 35.70;
            public static readonly double LatitudeEnd = 35.71;
            public static readonly double LongitudeStart = 140.865;
            public static readonly double LongitudeEnd = 140.875;
        }*/

		//public class S01S
		//{
		//          public static readonly double LatitudeEnd = 35.43736766;
		//	public static readonly double LatitudeStart = 35.43536766;
		//	public static readonly double LongitudeEnd = 139.5559428;
		//	public static readonly double LongitudeStart = 139.5539428;
		//}






		public class S01S//homeward用全部＆保土ヶ谷バイパスのみ到着地点
		{
			public static readonly double LatitudeEnd = 35.48175021;
			public static readonly double LatitudeStart = 35.47975021;
			public static readonly double LongitudeEnd = 139.5174919;
			public static readonly double LongitudeStart = 139.5154919;
		}
		//TODO: 三浦に行った時のデータ挿入、DBに目的地格納
	}
}
