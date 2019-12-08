using System;
#if UNITY_ENGINE
using UnityEngine;
#endif

namespace TinyJSON
{
	public sealed class ProxyString : Variant
	{
		readonly string value;


		public ProxyString( string value )
		{
			this.value = value;
		}


		public override string ToString( IFormatProvider provider )
		{
			return value;
		}

		public override char ToChar( IFormatProvider provider )
		{
			return value[0];
		}


#if UNITY_ENGINE
		public override object ToType(Type conversionType, IFormatProvider provider)
		{
			if (conversionType == typeof(Color32))
			{
				return ToColor32();
			}
			
			if (conversionType == typeof(Color))
			{
				return (Color)ToColor32();
			}
			
			return base.ToType( conversionType, provider );
		}

		private Color32 ToColor32()
		{
			// Decode hex number format "0xRRGGBB[AA]"
			return new Color32(
				Convert.ToByte( value.Substring( 2, 2 ), 16 ), // R
				Convert.ToByte( value.Substring( 4, 2 ), 16 ), // G
				Convert.ToByte( value.Substring( 6, 2 ), 16 ), // B
				value.Length == 10 ? Convert.ToByte( value.Substring( 8, 2 ), 16 ) : byte.MaxValue // A, if present (else 255)
			);
		}
#endif
	}
}
