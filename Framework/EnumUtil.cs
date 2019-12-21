using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AdventOfCode {
    public static class EnumUtil {
        public static Array GetValues(Type enumType, bool flagsOnly = true) {
            MethodInfo getValues = typeof(EnumUtil).GetMethod(nameof(GetValues), BindingFlags.Public | BindingFlags.Static,
                null, CallingConventions.Standard, new [] { typeof(bool) }, null)?.MakeGenericMethod(enumType);
            
            return (Array)getValues?.Invoke(null, new object[] { flagsOnly });
        }
        
        public static T[] GetValues<T>(bool flagsOnly = true) {
            Type enumType = typeof(T);
            Debug.Assert(enumType.IsEnum, "Cannot get enum values from a non-enum type: " + enumType);
            
            T[] allValues = (T[])Enum.GetValues(enumType);
        
            return (flagsOnly && Attribute.IsDefined(enumType, typeof(FlagsAttribute))) ? allValues.Where(IsFlag).ToArray() : allValues;
        }
        
        private static bool IsFlag<T>(T value) {
            int n = Convert.ToInt32(value);
            
            bool isBitSet = false;
            while (n != 0) {
                if ((n & 1) != 0) {
                    if (isBitSet) {
                        return false;
                    }
                    
                    isBitSet = true;
                }
                n >>= 1;
            }
            return isBitSet;
        }
    }
}