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

		public class OaraiTerminal //大洗フェリーターミナル
		{
			public static readonly double LatitudeStart = 36.309;
			public static readonly double LatitudeEnd = 36.319;
			public static readonly double LongitudeStart = 140.575;
			public static readonly double LongitudeEnd = 140.585;
		}

		public class TomakomaiTerminal //苫小牧フェリーターミナル
		{
			public static readonly double LatitudeStart = 42.635;
			public static readonly double LatitudeEnd = 42.645;
			public static readonly double LongitudeStart = 141.63;
			public static readonly double LongitudeEnd = 141.64;
		}

		public class RoytonSapporo //ロイトン札幌ホテル
		{
			public static readonly double LatitudeStart = 43.06;
			public static readonly double LatitudeEnd = 43.07;
			public static readonly double LongitudeStart = 141.339;
			public static readonly double LongitudeEnd = 141.349;
		}

		public class HotelTaisetsu //ホテル大雪
		{
			public static readonly double LatitudeStart = 43.715;
			public static readonly double LatitudeEnd = 43.725;
			public static readonly double LongitudeStart = 142.945;
			public static readonly double LongitudeEnd = 142.955;
		}

		public class Oehonke //大江本家（ホテル）
		{
			public static readonly double LatitudeStart = 43.755;
			public static readonly double LatitudeEnd = 43.765;
			public static readonly double LongitudeStart = 143.508;
			public static readonly double LongitudeEnd = 143.518;
		}

		public class KKRKawayu //KKRかわゆ（ホテル）
		{
			public static readonly double LatitudeStart = 43.635;
			public static readonly double LatitudeEnd = 43.645;
			public static readonly double LongitudeStart = 144.435;
			public static readonly double LongitudeEnd = 144.445;
		}

		public class MichinoEkiMashu //道の駅摩周温泉
		{
			public static readonly double LatitudeStart = 43.492;
			public static readonly double LatitudeEnd = 43.502;
			public static readonly double LongitudeStart = 144.445;
			public static readonly double LongitudeEnd = 144.455;
		}

		/***箱根走行ログインサート用***/
		public class ToujiIkkyu //かよい湯治一休
		{
			public static readonly double LatitudeStart = 35.219;
			public static readonly double LatitudeEnd = 35.229;
			public static readonly double LongitudeStart = 139.084;
			public static readonly double LongitudeEnd = 139.094;
		}

		/***銚子走行実験インサート用***/

		public class Inubozaki //犬吠埼灯台
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


		//public class ShimoSeyaNichome//東名出発地点
		//{
		//	public static readonly double LatitudeEnd = 35.51194857;
		//	public static readonly double LatitudeStart = 35.49194857;
		//	public static readonly double LongitudeEnd = 139.4882452;
		//	public static readonly double LongitudeStart = 139.4682452;
		//}

		//public class ShimoSeyaNichome//小田厚木出発地点
		//{
		//	public static readonly double LatitudeEnd = 35.43327353;
		//	public static readonly double LatitudeStart = 35.41327353;
		//	public static readonly double LongitudeEnd = 139.3764588;
		//	public static readonly double LongitudeStart = 139.3564588;
		//}


		public class ShimoSeyaNichome//東海道国道30号線出発
		{
			public static readonly double LatitudeEnd = 35.42235987;
			public static readonly double LatitudeStart = 35.40235987;
			public static readonly double LongitudeEnd = 139.5425512;
			public static readonly double LongitudeStart = 139.5225512;
		}


		//public class ShimoSeyaNichome//homeward用保土ヶ谷BPのみ
		//{
		//	public static readonly double LatitudeEnd = 35.45232953;
		//	public static readonly double LatitudeStart = 35.45032953;
		//	public static readonly double LongitudeEnd = 139.5681481;
		//	public static readonly double LongitudeStart = 139.5661481;
		//}

		//public class ShimoSeyaNichome//オロロンライン
		//{
		//	public static readonly double LatitudeEnd = 45.326535;
		//	public static readonly double LatitudeStart = 45.306535;
		//	public static readonly double LongitudeEnd = 141.644306;
		//	public static readonly double LongitudeStart = 141.624306;
		//}

		//public class ShimoSeyaNichome//小田厚木出発地
		//{
		//	public static readonly double LatitudeEnd = 35.43327353;
		//	public static readonly double LatitudeStart = 35.41327353;
		//	public static readonly double LongitudeEnd = 139.3764588;
		//	public static readonly double LongitudeStart = 139.3564588;
		//}


		/*public class ShimoSeyaNichome//273号線出発地
        {
            public static readonly double LatitudeEnd = 43.84986136;
            public static readonly double LatitudeStart = 43.82986136;
            public static readonly double LongitudeEnd = 142.7946763;
            public static readonly double LongitudeStart = 142.7746763;
        }*/

		//public class ShimoSeyaNichome//39号線出発地
		//{
		//    public static readonly double LatitudeEnd = 43.79050569;
		//    public static readonly double LatitudeStart = 43.77050569;
		//    public static readonly double LongitudeEnd = 143.603703;
		//    public static readonly double LongitudeStart = 143.583703;

		//public class ShimoSeyaNichome//横浜新道出発地点
		//{
		//	public static readonly double LatitudeEnd = 35.47489053;
		//	public static readonly double LatitudeStart = 35.45489053;
		//	public static readonly double LongitudeEnd = 139.6008231;
		//	public static readonly double LongitudeStart = 139.5808231;
		//}

		//public class ShimoSeyaNichome//横浜新道出発地点(戸塚)
		//{
		//	public static readonly double LatitudeEnd = 35.46277291;
		//	public static readonly double LatitudeStart = 35.44277291;
		//	public static readonly double LongitudeEnd = 139.5872564;
		//	public static readonly double LongitudeStart = 139.5672564;
		//}

		//public class ShimoSeyaNichome//東海道出発地点
		//{
		//	public static readonly double LatitudeEnd = 35.42235987;
		//	public static readonly double LatitudeStart = 35.40235987;
		//	public static readonly double LongitudeEnd = 139.5425512;
		//	public static readonly double LongitudeStart = 139.5225512;
		//}

		//public class ShimoSeyaNichome//新湘南バイパス出発地点
		//{
		//	public static readonly double LatitudeEnd = 35.35812688;
		//	public static readonly double LatitudeStart = 35.33812688;
		//	public static readonly double LongitudeEnd = 139.4617146;
		//	public static readonly double LongitudeStart = 139.4417146;
		//}

		//public class ShimoSeyaNichome//国道134号線出発地点
		//{
		//	public static readonly double LatitudeEnd = 35.33002731;
		//	public static readonly double LatitudeStart = 35.31002731;
		//	public static readonly double LongitudeEnd = 139.3852237;
		//	public static readonly double LongitudeStart = 139.3652237;
		//}

		//public class ShimoSeyaNichome//西湘バイパス出発地点
		//{
		//	public static readonly double LatitudeEnd    = 35.32268608;
		//	public static readonly double LatitudeStart  = 35.30268608;
		//	public static readonly double LongitudeEnd   = 139.3366197;
		//	public static readonly double LongitudeStart = 139.3166197;
		//}





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







		//public class S01S//homeward用全部＆保土ヶ谷バイパスのみ到着地点
		//{
		//	public static readonly double LatitudeEnd = 35.48175021;
		//	public static readonly double LatitudeStart = 35.47975021;
		//	public static readonly double LongitudeEnd = 139.5174919;
		//	public static readonly double LongitudeStart = 139.5154919;
		//}

		//public class S01S//オロロンライン
		//{
		//	public static readonly double LatitudeEnd = 44.98637063;
		//	public static readonly double LatitudeStart = 44.96637063;
		//	public static readonly double LongitudeEnd = 141.7060291;
		//	public static readonly double LongitudeStart = 141.6860291;
		//}


		//public class S01S//小田厚木到達地点
		//{
		//	public static readonly double LatitudeEnd = 35.25591438;
		//	public static readonly double LatitudeStart = 35.23591438;
		//	public static readonly double LongitudeEnd = 139.1430423;
		//	public static readonly double LongitudeStart = 139.1230423;
		//}



		//public class S01S//町田IC
		//{
		//	public static readonly double LatitudeEnd = 35.51551755;
		//	public static readonly double LatitudeStart = 35.49551755;
		//	public static readonly double LongitudeEnd = 139.4925436;
		//	public static readonly double LongitudeStart = 139.4725436;
		//}


		//public class S01S//厚木IC
		//{
		//	public static readonly double LatitudeEnd = 35.43321533;
		//	public static readonly double LatitudeStart = 35.41321533;
		//	public static readonly double LongitudeEnd = 139.3768755;
		//	public static readonly double LongitudeStart = 139.3568755;
		//}

		//public class S01S//小田原西
		//{
		//	public static readonly double LatitudeEnd = 35.25612339;
		//	public static readonly double LatitudeStart = 35.23612339;
		//	public static readonly double LongitudeEnd = 139.1429704;
		//	public static readonly double LongitudeStart = 139.1229704;
		//}

		//public class S01S//小田原西
		//{
		//	public static readonly double LatitudeEnd = 35.32995424;
		//	public static readonly double LatitudeStart = 35.30995424;
		//	public static readonly double LongitudeEnd = 139.3849821;
		//	public static readonly double LongitudeStart = 139.3649821;
		//}


		//public class S01S//273号線到着地
		//{
		//	public static readonly double LatitudeEnd = 43.90005692;
		//	public static readonly double LatitudeStart = 43.88005692;
		//	public static readonly double LongitudeEnd = 142.9790281;
		//	public static readonly double LongitudeStart = 142.9590281;
		//}

		//public class S01S//北海道到達
		//	{
		//		public static readonly double LatitudeEnd = 44.03468273;
		//		public static readonly double LatitudeStart = 44.01468273;
		//		public static readonly double LongitudeEnd = 143.5184957;
		//		public static readonly double LongitudeStart = 143.4984957;
		//	}

		//public class S01S//戸塚
		//{
		//	public static readonly double LatitudeEnd = 35.42241955;
		//	public static readonly double LatitudeStart = 35.40241955;
		//	public static readonly double LongitudeEnd = 139.5427005;
		//	public static readonly double LongitudeStart = 139.5227005;
		//}

		//public class S01S//東海道到達地点
		//{
		//	public static readonly double LatitudeEnd = 35.35827491;
		//	public static readonly double LatitudeStart = 35.33827491;
		//	public static readonly double LongitudeEnd = 139.4632238;
		//	public static readonly double LongitudeStart = 139.4432238;
		//}

		//public class S01S//新湘南バイパス到達地点
		//{
		//	public static readonly double LatitudeEnd = 35.34335417;
		//	public static readonly double LatitudeStart = 35.32335417;
		//	public static readonly double LongitudeEnd = 139.3941454;
		//	public static readonly double LongitudeStart = 139.3741454;
		//}

		//public class S01S//国道134号線到達地点
		//{
		//	public static readonly double LatitudeEnd = 35.32283345;
		//	public static readonly double LatitudeStart = 35.30283345;
		//	public static readonly double LongitudeEnd = 139.3371098;
		//	public static readonly double LongitudeStart = 139.3171098;
		//}

		//public class S01S//西湘バイパス到達地点
		//{
		//	public static readonly double LatitudeEnd    = 35.25550068;
		//	public static readonly double LatitudeStart  = 35.23550068;
		//	public static readonly double LongitudeEnd   = 139.1438304;
		//	public static readonly double LongitudeStart = 139.1238304;
		//}

		public class S01S//東海道国道30号線到達地点
		{
			public static readonly double LatitudeEnd = 35.32996271;
			public static readonly double LatitudeStart = 35.30996271;
			public static readonly double LongitudeEnd = 139.3850102;
			public static readonly double LongitudeStart = 139.3650102;
		}


		//TODO: 三浦に行った時のデータ挿入、DBに目的地格納
	}

}