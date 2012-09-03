using System;
using NDWR.ServiceStruct;
using NDWR.Web;
using Newtonsoft.Json;

namespace NDWR.Validator {

    /// <summary>
    /// 值类型转换辅助类
    /// </summary>
    public static class TypeHelper {

        /// <summary>
        /// 判断是否为基础类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBaseType(Type type) {
            return type.IsValueType || type == typeof(string) ? true : false;
        }


        public static TypeCategory GetTypeCategory(Type type) {
            return TypeHelper.IsBaseType(type) ? TypeCategory.SimplyType : // 简单类型[值类型+string]
                    typeof(TransferFile) == type ? TypeCategory.BinaryType : // 输入流类型[TransferFile类型]
                                                   TypeCategory.EntityType;  // 实体类型[默认]
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
