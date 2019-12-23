using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
#if UNITY_ENGINE
using UnityEngine;
#if ENABLE_IL2CPP
using UnityEngine.Scripting;
#endif
#endif


namespace TinyJSON
{
	/// <summary>
	/// Mark members that should be included.
	/// Public fields are included by default.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public sealed class Include : Attribute {}


	/// <summary>
	/// Mark members that should be excluded.
	/// Private fields and all properties are excluded by default.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public class Exclude : Attribute {}


	/// <summary>
	/// Mark methods to be called after an object is decoded.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method )]
	public class AfterDecode : Attribute {}


	/// <summary>
	/// Mark methods to be called before an object is encoded.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method )]
	public class BeforeEncode : Attribute {}


	/// <summary>
	/// Mark members to force type hinting even when EncodeOptions.NoTypeHints is set.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public class TypeHint : Attribute {}


	/// <summary>
	/// Provide field and property aliases when an object is decoded.
	/// If a field or property is not found while decoding, this list will be searched for a matching alias.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true )]
	public class DecodeAlias : Attribute
	{
		public string[] Names { get; private set; }


		public DecodeAlias( params string[] names )
		{
			Names = names;
		}


		public bool Contains( string name )
		{
			return Array.IndexOf( Names, name ) > -1;
		}
	}
	
	/// <summary>
	/// Provide alternate names for fields and properties to be encoded under. This name will also be used when decoding.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class EncodeName : Attribute
	{
		public string Name { get; private set; }
		
		public EncodeName(string name)
		{
			Name = name;
		}
	}


	[Obsolete( "Use the Exclude attribute instead." )]
	// ReSharper disable once UnusedMember.Global
	public sealed class Skip : Exclude {}


	[Obsolete( "Use the AfterDecode attribute instead." )]
	// ReSharper disable once UnusedMember.Global
	public sealed class Load : AfterDecode {}


	public sealed class DecodeException : Exception
	{
		public DecodeException( string message )
			: base( message ) {}


		public DecodeException( string message, Exception innerException )
			: base( message, innerException ) {}
	}


#if ENABLE_IL2CPP
	[Preserve]
#endif
	// ReSharper disable once InconsistentNaming
	public static class JSON
	{
		static readonly Type includeAttrType = typeof(Include);
		static readonly Type excludeAttrType = typeof(Exclude);
		static readonly Type encodeNameAttrType = typeof(EncodeName);
		static readonly Type decodeAliasAttrType = typeof(DecodeAlias);

		/// <summary>
		/// Encodes the given object into a JSON string.
		/// </summary>
		public static string Encode( object data, EncodeOptions options = EncodeOptions.None )
		{
			return Encoder.Encode( data, options );
		}

		/// <summary>
		/// Decodes the given JSON string into an object of the given type.
		/// </summary>
		public static T Decode<T>( string json )
		{
			Decode( json, out T item );
			return item;
		}
		
		/// <summary>
		/// Decodes the given JSON string into an object of the given type.
		/// </summary>
		public static void Decode<T>( string json, out T item )
		{
			MakeInto( Load(json), out item );
		}

		/// <summary>
		/// Decodes the given JSON string into a Variant proxy object.
		/// The Variant's data may be accessed directly, and can be converted into a concrete type or re-encoded.
		/// </summary>
		public static Variant Load( string json )
		{
			if (json == null)
			{
				throw new ArgumentNullException( nameof(json) );
			}

			return Decoder.Decode( json );
		}

		/// <summary>
		/// Converts the given Variant proxy object into a concrete object of type T.
		/// </summary>
		public static void MakeInto<T>( Variant data, out T item )
		{
			item = DecodeType<T>( data );
		}

		static readonly Dictionary<string, Type> typeCache = new Dictionary<string, Type>();

		static Type FindType( string fullName )
		{
			if (fullName == null)
			{
				return null;
			}

			Type type;
			if (typeCache.TryGetValue( fullName, out type ))
			{
				return type;
			}

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType( fullName );
				if (type != null)
				{
					typeCache.Add( fullName, type );
					return type;
				}
			}

			return null;
		}


#if ENABLE_IL2CPP
		[Preserve]
#endif
		static T DecodeType<T>( Variant data )
		{
			if (data == null)
			{
				return default(T);
			}

			var type = typeof(T);

			if (type.IsEnum)
			{
				return (T) Enum.Parse( type, data.ToString( CultureInfo.InvariantCulture ) );
			}

			if (type.IsPrimitive
				|| type == typeof(string)
				|| type == typeof(char)
				|| type == typeof(decimal)
				|| type == typeof(DateTime)
#if UNITY_ENGINE
				|| type == typeof(Color)
				|| type == typeof(Color32)
#endif
			)
			{
				return (T) Convert.ChangeType( data, type );
			}

			if (type.IsArray)
			{
				if (type.GetArrayRank() == 1)
				{
					var makeFunc = decodeArrayMethod.MakeGenericMethod( type.GetElementType() );
					return (T) makeFunc.Invoke( null, new object[] { data } );
				}

				var arrayData = data as ProxyArray;
				if (arrayData == null)
				{
					throw new DecodeException( "Variant is expected to be a ProxyArray here, but it is not." );
				}

				var arrayRank = type.GetArrayRank();
				var rankLengths = new int[arrayRank];
				if (arrayData.CanBeMultiRankArray( rankLengths ))
				{
					var elementType = type.GetElementType();
					if (elementType == null)
					{
						throw new DecodeException( "Array element type is expected to be not null, but it is." );
					}

					var array = Array.CreateInstance( elementType, rankLengths );
					var makeFunc = decodeMultiRankArrayMethod.MakeGenericMethod( elementType );
					try
					{
						makeFunc.Invoke( null, new object[] { arrayData, array, 1, rankLengths } );
					}
					catch (Exception e)
					{
						throw new DecodeException( "Error decoding multidimensional array. Did you try to decode into an array of incompatible rank or element type?", e );
					}

					return (T) Convert.ChangeType( array, typeof(T) );
				}

				throw new DecodeException( "Error decoding multidimensional array; JSON data doesn't seem fit this structure." );
			}

			if (typeof(IList).IsAssignableFrom( type ))
			{
				List<Type> typeArguments = new List<Type>{ type };
				typeArguments.AddRange( type.GetGenericArguments() );
				var makeFunc = decodeListMethod.MakeGenericMethod( typeArguments.ToArray() );
				return (T) makeFunc.Invoke( null, new object[] { data } );
			}

			if (typeof(IDictionary).IsAssignableFrom( type ))
			{
				List<Type> typeArguments = new List<Type>{ type };
				typeArguments.AddRange( type.GetGenericArguments() );
				var makeFunc = decodeDictionaryMethod.MakeGenericMethod( typeArguments.ToArray() );
				return (T) makeFunc.Invoke( null, new object[] { data } );
			}

			// At this point we should be dealing with a class or struct.
			T instance;
			var proxyObject = data as ProxyObject;
			if (proxyObject == null)
			{
				throw new InvalidCastException( "ProxyObject expected when decoding into '" + type.FullName + "'." );
			}

			// If there's a type hint, use it to create the instance.
			var typeHint = proxyObject.TypeHint;
			if (typeHint != null && typeHint != type.FullName)
			{
				var makeType = FindType( typeHint );
				if (makeType == null)
				{
					throw new TypeLoadException( "Could not load type '" + typeHint + "'." );
				}

				if (type.IsAssignableFrom( makeType ))
				{
					instance = (T) Activator.CreateInstance( makeType );
					type = makeType;
				}
				else
				{
					throw new InvalidCastException( "Cannot assign type '" + typeHint + "' to type '" + type.FullName + "'." );
				}
			}
			else
			{
				// We don't have a type hint, so just instantiate the type we have.
				instance = Activator.CreateInstance<T>();
			}


			// Now decode fields and properties.
			foreach (var pair in (ProxyObject) data)
			{
				var field = type.GetField( pair.Key, instanceBindingFlags );
				
				// If the field doesn't exist or is excluded, search for an [EncodeName] or any [DecodeAlias]
				if (field == null || Attribute.IsDefined( field, typeof(Exclude) ))
				{
					foreach (var fieldInfo in type.GetFields( instanceBindingFlags ))
					{
						foreach (var attribute in fieldInfo.GetCustomAttributes( true ))
						{
							if (encodeNameAttrType.IsInstanceOfType( attribute ))
							{
								if (((EncodeName)attribute).Name == pair.Key)
								{
									field = fieldInfo;
									break;
								}
							}
							
							if (decodeAliasAttrType.IsInstanceOfType( attribute ))
							{
								if (((DecodeAlias) attribute).Contains( pair.Key ))
								{
									field = fieldInfo;
									break;
								}
							}
						}
					}
				}

				if (field != null)
				{
					var shouldDecode = field.IsPublic;
					foreach (var attribute in field.GetCustomAttributes( true ))
					{
						if (excludeAttrType.IsInstanceOfType( attribute ))
						{
							shouldDecode = false;
						}

						if (includeAttrType.IsInstanceOfType( attribute ))
						{
							shouldDecode = true;
						}
					}

					if (shouldDecode)
					{
						var makeFunc = decodeTypeMethod.MakeGenericMethod( field.FieldType );
						if (type.IsValueType)
						{
							// Type is a struct.
							var instanceRef = (object) instance;
							field.SetValue( instanceRef, makeFunc.Invoke( null, new object[] { pair.Value } ) );
							instance = (T) instanceRef;
						}
						else
						{
							// Type is a class.
							field.SetValue( instance, makeFunc.Invoke( null, new object[] { pair.Value } ) );
						}
					}
				}

				var property = type.GetProperty( pair.Key, instanceBindingFlags );

				// If the property doesn't exist or is excluded, search for an [EncodeName] or any [DecodeAlias]
				if (property == null || Attribute.IsDefined( property, typeof(Exclude) ))
				{
					foreach (var propertyInfo in type.GetProperties( instanceBindingFlags ))
					{
						foreach (var attribute in propertyInfo.GetCustomAttributes( false ))
						{
							if (encodeNameAttrType.IsInstanceOfType( attribute ))
							{
								if (((EncodeName)attribute).Name == pair.Key)
								{
									property = propertyInfo;
									break;
								}
							}
							
							if (decodeAliasAttrType.IsInstanceOfType( attribute ))
							{
								if (((DecodeAlias) attribute).Contains( pair.Key ))
								{
									property = propertyInfo;
									break;
								}
							}
						}
					}
				}

				if (property != null)
				{
					if (property.CanWrite && property.GetCustomAttributes( false ).AnyOfType( includeAttrType ))
					{
						var makeFunc = decodeTypeMethod.MakeGenericMethod( new Type[] { property.PropertyType } );
						if (type.IsValueType)
						{
							// Type is a struct.
							var instanceRef = (object) instance;
							property.SetValue( instanceRef, makeFunc.Invoke( null, new object[] { pair.Value } ), null );
							instance = (T) instanceRef;
						}
						else
						{
							// Type is a class.
							property.SetValue( instance, makeFunc.Invoke( null, new object[] { pair.Value } ), null );
						}
					}
				}
			}

			// Invoke methods tagged with [AfterDecode] attribute.
			foreach (var method in type.GetMethods( instanceBindingFlags ))
			{
				if (Attribute.IsDefined( method, typeof(AfterDecode) ))
				{
					method.Invoke( instance, method.GetParameters().Length == 0 ? null : new object[] { data } );
				}
			}

			return instance;
		}


#if ENABLE_IL2CPP
		[Preserve]
#endif
		// ReSharper disable once UnusedMethodReturnValue.Local
		static TList DecodeList<TList, T>( Variant data ) where TList : IList<T>, new()
		{
			var list = new TList();

			var proxyArray = data as ProxyArray;
			if (proxyArray == null)
			{
				throw new DecodeException( "Variant is expected to be a ProxyArray here, but it is not." );
			}

			foreach (var item in proxyArray)
			{
				list.Add( DecodeType<T>( item ) );
			}

			return list;
		}


#if ENABLE_IL2CPP
		[Preserve]
#endif
		// ReSharper disable once UnusedMethodReturnValue.Local
		static TDictionary DecodeDictionary<TDictionary, TKey, TValue>( Variant data ) where TDictionary : IDictionary<TKey, TValue>, new()
		{
			var dict = new TDictionary();
			var type = typeof(TKey);

			var proxyObject = data as ProxyObject;
			if (proxyObject == null)
			{
				throw new DecodeException( "Variant is expected to be a ProxyObject here, but it is not." );
			}

			foreach (var pair in proxyObject)
			{
				var k = (TKey) (type.IsEnum ? Enum.Parse( type, pair.Key ) : Convert.ChangeType( pair.Key, type ));
				var v = DecodeType<TValue>( pair.Value );
				dict.Add( k, v );
			}

			return dict;
		}


#if ENABLE_IL2CPP
		[Preserve]
#endif
		// ReSharper disable once UnusedMethodReturnValue.Local
		static T[] DecodeArray<T>( Variant data )
		{
			var arrayData = data as ProxyArray;
			if (arrayData == null)
			{
				throw new DecodeException( "Variant is expected to be a ProxyArray here, but it is not." );
			}

			var arraySize = arrayData.Count;
			var array = new T[arraySize];

			var i = 0;
			foreach (var item in arrayData)
			{
				array[i++] = DecodeType<T>( item );
			}

			return array;
		}


#if ENABLE_IL2CPP
		[Preserve]
#endif
		// ReSharper disable once UnusedMember.Local
		static void DecodeMultiRankArray<T>( ProxyArray arrayData, Array array, int arrayRank, int[] indices )
		{
			var count = arrayData.Count;
			for (var i = 0; i < count; i++)
			{
				indices[arrayRank - 1] = i;
				if (arrayRank < array.Rank)
				{
					DecodeMultiRankArray<T>( arrayData[i] as ProxyArray, array, arrayRank + 1, indices );
				}
				else
				{
					array.SetValue( DecodeType<T>( arrayData[i] ), indices );
				}
			}
		}


		const BindingFlags instanceBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		const BindingFlags staticBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
		static readonly MethodInfo decodeTypeMethod = typeof(JSON).GetMethod( "DecodeType", staticBindingFlags );
		static readonly MethodInfo decodeListMethod = typeof(JSON).GetMethod( "DecodeList", staticBindingFlags );
		static readonly MethodInfo decodeDictionaryMethod = typeof(JSON).GetMethod( "DecodeDictionary", staticBindingFlags );
		static readonly MethodInfo decodeArrayMethod = typeof(JSON).GetMethod( "DecodeArray", staticBindingFlags );
		static readonly MethodInfo decodeMultiRankArrayMethod = typeof(JSON).GetMethod( "DecodeMultiRankArray", staticBindingFlags );


#if ENABLE_IL2CPP
		[Preserve]
#endif
		// ReSharper disable once InconsistentNaming
		public static void SupportTypeForAOT<T>()
		{
			DecodeType<T>( null );
			DecodeList<List<T>, T>( null );
			DecodeArray<T>( null );
			DecodeDictionary<Dictionary<Int16, T>, Int16, T>( null );
			DecodeDictionary<Dictionary<UInt16, T>, UInt16, T>( null );
			DecodeDictionary<Dictionary<Int32, T>, Int32, T>( null );
			DecodeDictionary<Dictionary<UInt32, T>, UInt32, T>( null );
			DecodeDictionary<Dictionary<Int64, T>, Int64, T>( null );
			DecodeDictionary<Dictionary<UInt64, T>, UInt64, T>( null );
			DecodeDictionary<Dictionary<Single, T>, Single, T>( null );
			DecodeDictionary<Dictionary<Double, T>, Double, T>( null );
			DecodeDictionary<Dictionary<Decimal, T>, Decimal, T>( null );
			DecodeDictionary<Dictionary<Boolean, T>, Boolean, T>( null );
			DecodeDictionary<Dictionary<String, T>, String, T>( null );
		}


#if ENABLE_IL2CPP
		[Preserve]
#endif
		// ReSharper disable once InconsistentNaming
		// ReSharper disable once UnusedMember.Local
		static void SupportValueTypesForAOT()
		{
			SupportTypeForAOT<Int16>();
			SupportTypeForAOT<UInt16>();
			SupportTypeForAOT<Int32>();
			SupportTypeForAOT<UInt32>();
			SupportTypeForAOT<Int64>();
			SupportTypeForAOT<UInt64>();
			SupportTypeForAOT<Single>();
			SupportTypeForAOT<Double>();
			SupportTypeForAOT<Decimal>();
			SupportTypeForAOT<Boolean>();
			SupportTypeForAOT<String>();
		}
	}
}
