using System;

namespace NDWR.Validator {
    public static class ValueConvert {

        /// <summary>
        /// 判断是否为基础类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBaseType(Type type) {

            if (type == typeof(Char)) {
                return true;
            }
            if (type == typeof(Boolean)) {
                return true;
            }
            if (type == typeof(Byte)) {
                return true;
            }
            if (type == typeof(SByte)) {
                return true;
            }
            if (type == typeof(Int16)) {
                return true;
            }
            if (type == typeof(UInt16)) {
                return true;
            }
            if (type == typeof(Int32)) {
                return true;
            }
            if (type == typeof(UInt32)) {
                return true;
            }
            if (type == typeof(Int64)) {
                return true;
            }
            if (type == typeof(UInt64)) {
                return true;
            }
            if (type == typeof(Single)) {
                return true;
            }
            if (type == typeof(Double)) {
                return true;
            }
            if (type == typeof(Decimal)) {
                return true;
            }
            if (type == typeof(DateTime)) {
                return true;
            }
            if (type == typeof(String)) {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ConvertBaseType(string value, Type type, out object targetValue) {
            bool isSuccess;
            if (type == typeof(String)) {
                targetValue = value;
                return true;
            }
            if (type == typeof(Char)) {
                char result = char.MinValue;
                isSuccess = char.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(Boolean)) {
                bool result = false;
                isSuccess = Boolean.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(Byte)) {
                Byte result = Byte.MinValue;
                isSuccess = Byte.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(SByte)) {
                SByte result = SByte.MinValue;
                isSuccess = SByte.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(Int16)) {
                Int16 result = Int16.MinValue;
                isSuccess = Int16.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(UInt16)) {
                UInt16 result = UInt16.MinValue;
                isSuccess = UInt16.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(Int32)) {
                Int32 result = Int32.MinValue;
                isSuccess = Int32.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(UInt32)) {
                UInt32 result = UInt32.MinValue;
                isSuccess = UInt32.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(Int64)) {
                Int64 result = Int64.MinValue;
                isSuccess = Int64.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(UInt64)) {
                UInt64 result = UInt64.MinValue;
                isSuccess = UInt64.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(Single)) {
                Single result = Single.MinValue;
                isSuccess = Single.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(Double)) {
                Double result = Double.MinValue;
                isSuccess = Double.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(Decimal)) {
                Decimal result = Decimal.MinValue;
                isSuccess = Decimal.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            if (type == typeof(DateTime)) {
                DateTime result = DateTime.MinValue;
                isSuccess = DateTime.TryParse(value, out result);
                targetValue = result;
                return isSuccess;
            }
            targetValue = null;
            return false;
        }
    }
}
