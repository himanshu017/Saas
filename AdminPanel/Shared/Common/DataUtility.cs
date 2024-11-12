
using System;
using System.Data;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Collections;
using System.Web;

namespace AdminPanel.Shared
{
    public class DataUtility
    {
        #region DataRow Related Functions

        public static string GetStringFromDataRow(DataRow row, string fieldName, string defaultValue = "")
        {
            string dbValue;
            if (!row.IsNull(fieldName))
            {
                dbValue = row[fieldName].ToString();
                if (dbValue != "")
                {
                    return dbValue;
                }
            }

            return defaultValue;
        }

        public static string GetStringFromDataRow(DataRow row, int fieldPosition)
        {
            string dbValue;
            if (!row.IsNull(fieldPosition))
            {
                dbValue = row[fieldPosition].ToString();
                if (dbValue != "")
                {
                    return dbValue;
                }
            }

            return "";
        }

        public static object GetDataFromDataRow(DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
            {
                return row[fieldName];
            }

            return null;
        }

        public static int GetIntFromDataRow(DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
            {
                return Convert.ToInt32(row[fieldName]);
            }

            return -1;
        }

        public static long GetLongFromDataRow(DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
            {
                long data = 0;
                if (long.TryParse((row[fieldName]).ToString(), out data))
                    return data;
            }

            return 0;
        }

        public static int GetIntFromDataRow(DataRow row, int fieldPosition)
        {
            if (!row.IsNull(fieldPosition))
            {
                return Convert.ToInt32(row[fieldPosition]);
            }

            return -1; //Model.Constants.INVALID_ID;
        }

        public static short GetInt16FromDataRow(DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
            {
                return Convert.ToInt16(row[fieldName]);
            }

            return -1;
        }

        public static short GetInt16FromDataRow(DataRow row, int fieldPosition)
        {
            if (!row.IsNull(fieldPosition))
            {
                return Convert.ToInt16(row[fieldPosition]);
            }

            return -1; //Model.Constants.INVALID_ID;
        }

        public static decimal GetDecimalFromDataRow(DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
            {
                return Convert.ToDecimal(row[fieldName]);
            }

            return 0m;
        }

        public static decimal GetDecimalFromDataRow(DataRow row, int fieldPosition)
        {
            if (!row.IsNull(fieldPosition))
            {
                return Convert.ToDecimal(row[fieldPosition]);
            }

            return 0m;
        }

        public static DateTime GetDateTimeFromDataRow(DataRow row, string fieldName)
        {
            return GetDateTimeFromDataRow(row, row.Table.Columns.IndexOf(fieldName));
        }

        public static DateTime GetDateTimeFromDataRow(DataRow row, int fieldPosition)
        {
            object val = row[fieldPosition];

            if (val != null &&
                val != DBNull.Value)
            {
                if (val is DateTime)
                {
                    return (DateTime)val;
                }

                return DateTime.Parse(val.ToString());
            }

            return DateTime.MinValue;
        }

        public static DateTimeOffset GetDateTimeOffsetFromDataRow(DataRow row, string fieldName)
        {
            return GetDateTimeOffsetFromDataRow(row, row.Table.Columns.IndexOf(fieldName));
        }

        public static DateTimeOffset GetDateTimeOffsetFromDataRow(DataRow row, int fieldPosition)
        {
            object val = row[fieldPosition];

            return GetDatetimeOffset(val);
        }

        public static DateTimeOffset GetDatetimeOffset(object val)
        {
            var datetimeOffSet = DateTimeOffset.MinValue;

            if (val != null &&
                val != DBNull.Value)
            {
                if (val is DateTimeOffset)
                {
                    return (DateTimeOffset)val;
                }

                DateTimeOffset.TryParse(Convert.ToString(val), null, System.Globalization.DateTimeStyles.AssumeUniversal,
                out datetimeOffSet);
            }

            return datetimeOffSet;
        }

        public static bool GetBoolFromDataRow(DataRow row, string fieldName)
        {
            if (!row.IsNull(fieldName))
            {
                return ToBool(row[fieldName], false);
            }

            return false;
        }

        public static bool GetBoolFromDataRow(DataRow row, int fieldPosition)
        {
            if (!row.IsNull(fieldPosition))
            {
                return ToBool(row[fieldPosition], false);
            }

            return false;
        }

        #endregion

        public static int ToInt(object input)
        {
            try
            {
                return ToInt(input, -1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int ToInt(object input, int valueIfNull)
        {
            int ret = valueIfNull;

            try
            {
                if (input != null)
                {
                    if (!Int32.TryParse(input.ToString(), out ret))
                        ret = valueIfNull;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ret;
        }

        public static short ToInt16(object input)
        {
            try
            {
                return ToInt16(input, -1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static short ToInt16(object input, short valueIfNull)
        {
            short ret = valueIfNull;

            try
            {
                if (input != null)
                {
                    if (input != null)
                    {
                        if (!Int16.TryParse(input.ToString(), out ret))
                            ret = valueIfNull;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ret;
        }

        public static string GetTruncatedString(string displayText, int length)
        {
            if (displayText.Length > length)
            {
                displayText = displayText.Substring(0, length - 3) + "...";
            }

            return displayText;
        }

        public static string DisplayString(string inputString)
        {
            return inputString.Replace("\r\n", "<br />");
        }

        public static string CleanString(string inputString, int maxLength)
        {
            StringBuilder retVal = new StringBuilder(maxLength * 2);

            // check incoming parameters for null or blank string
            if ((inputString != null) &&
                (inputString.Length != 0))
            {
                inputString = inputString.Trim();

                //chop the string incase the client-side max length
                //fields are bypassed to prevent buffer over-runs
                if (inputString.Length > maxLength)
                {
                    inputString = inputString.Substring(0, maxLength);
                }

                //convert some harmful symbols incase the regular
                //expression validators are changed

                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }

                // Replace single quotes with white space
                retVal.Replace("'", " ");
            }

            return retVal.ToString();
        }

        public const int BYTE = 1;
        public const int KB = 1024;
        public const int MB = KB * 1024;
        public const int GB = MB * 1024;

        public static string FormatSize(long nSize)
        {
            string size;

            if (nSize > GB)
            {
                nSize = nSize / GB;
                size = nSize.ToString() + " GB";
            }
            else if (nSize > MB)
            {
                nSize = nSize / MB;
                size = (nSize).ToString() + " MB";
            }
            else if (nSize > KB)
            {
                nSize = nSize / KB;
                size = nSize.ToString() + " KB";
            }
            else
            {
                size = nSize.ToString() + " Bytes";
            }

            return size;
        }

        public static string GetFullName(string firstName, string middleName, string lastName)
        {
            if (middleName.Trim() != "" &&
                firstName.Trim() != "")
            {
                return firstName.Trim() + " " + middleName.Trim() + " " + lastName.Trim();
            }
            else if (firstName.Trim() == "" &&
                     middleName.Trim() != "")
            {
                return middleName.Trim() + " " + lastName.Trim();
            }
            else if (firstName.Trim() != "" &&
                     middleName.Trim() == "")
            {
                return firstName.Trim() + " " + lastName.Trim();
            }

            return lastName.Trim();
        }

        public static string GetFullName(string salutation, string firstName, string middleName, string lastName)
        {
            return string.Format("{0} {1}", salutation, GetFullName(firstName, middleName, lastName)).Trim();
        }

        public static bool IsValidDates(DateTime startDate, DateTime endDate)
        {
            int compareValue = startDate.CompareTo(endDate);
            return compareValue == 0 || compareValue < 0;
        }

        public static string GetFormattedAddress(string streetAdd, string city, string state, string country, string zip)
        {
            string fullAddress = "";

            fullAddress += streetAdd;

            if (fullAddress != "" &&
                city != "")
            {
                fullAddress += ", " + city;
            }
            else
            {
                fullAddress += city;
            }

            if (fullAddress != "" &&
                state != "")
            {
                fullAddress += ", " + state;
            }
            else
            {
                fullAddress += state;
            }

            if (fullAddress != "" &&
                country != "")
            {
                fullAddress += ", " + country;
            }
            else
            {
                fullAddress += country;
            }

            if (fullAddress != "" &&
                zip != "")
            {
                fullAddress += " - " + zip;
            }
            else
            {
                fullAddress += zip;
            }

            return fullAddress;
        }

        public static string ModelCleanString(string inputString, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return string.Empty;

            StringBuilder retVal = new StringBuilder(maxLength * 2);

            inputString = inputString.Trim();

            var chars = new[] { '"', '\'', '<', '>' };

            var fragments = inputString.Split(chars);
            var length = 0;
            var outputLength = inputString.Length;

            if (outputLength > maxLength)
                outputLength = maxLength;

            foreach (var fragment in fragments)
            {
                var newLength = length + fragment.Length + 1;

                if (newLength <= outputLength)
                {
                    retVal.Append(fragment);
                    length = newLength;
                }
                else
                {
                    retVal.Append(fragment, 0, outputLength - length);
                    length = outputLength;
                    break;
                }

                var specialCharacter = inputString[length - 1];

                switch (specialCharacter)
                {
                    case '"':
                        retVal.Append("&quot;");
                        break;
                    case '\'':
                        retVal.Append("&#146;");
                        break;
                    case '<':
                        retVal.Append("&lt;");
                        break;
                    case '>':
                        retVal.Append("&gt;");
                        break;
                }
            }

            var result = retVal.ToString();

            return (result.Length > maxLength) ? result.Substring(0, maxLength) : result;
        }


        //private static bool IsHtmlEncoded(string displayText)
        //{
        //    if (!string.IsNullOrEmpty(displayText))
        //    {
        //        if ((displayText.IndexOf("&lt;") != -1) || (displayText.IndexOf("&gt;") != -1)
        //            || (displayText.IndexOf("&apos;") != -1)
        //            || (displayText.IndexOf("&quot;") != -1)
        //            || (displayText.IndexOf("&amp;") != -1)
        //            || (displayText.IndexOf("&nbsp;") != -1)
        //            || (displayText.IndexOf("&#146;") != -1)
        //            )
        //            return true;
        //    }

        //    return false;
        //}

        public static string ConvertToOrignalString(string inputString)
        {
            if (inputString != null &&
                inputString.Length > 0)
            {
                inputString = inputString.Replace("&amp;", "&");
                inputString = inputString.Replace("&lt;", "<");
                inputString = inputString.Replace("&gt;", ">");
                inputString = inputString.Replace("&nbsp;", " ");
            }

            return inputString;
        }

        public static string CleanHtmlString(string inputString, int maxLength)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                inputString = inputString.Trim();

                inputString.Replace("'", "''");
                inputString.Replace("--", "- -");

                if (inputString.Length > maxLength)
                {
                    inputString = inputString.Substring(0, maxLength);
                }
            }
            return inputString;
        }

        public static XmlDocument SafeParseXml(string xml)
        {
            using (var reader = XmlTextReader.Create(new StringReader(xml)))
            {
                var document = new XmlDocument();
                document.Load(reader);
                return document;
            }
        }

        public static string GetXmlSafeStringOnlyAmpersend(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                data = data.Replace("&", "mp_0_mp");
            }
            return data;
        }

        public static string GetConvertedXmlSafeStringFromAmpersend(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                data = data.Replace("mp_0_mp", "&");
            }
            return data;
        }

        public static string GetXmlSafeString(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                data = data.Replace("<", "lt_0_lt");
                data = data.Replace(">", "gt_0_gt");
                data = data.Replace("&", "mp_0_mp");
                data = data.Replace("\\", "qt_0_qt");
                data = data.Replace("'", "aps_0_aps");
                data = data.Replace("+", "ps_0_ps");
                data = data.Replace("\"", "ps_0_ps");
            }

            return data;
        }

        public static string GetXmlConvertedString(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                data = data.Replace("lt_0_lt", "<");
                data = data.Replace("gt_0_gt", ">");
                data = data.Replace("mp_0_mp", "&");
                data = data.Replace("qt_0_qt", "\\");
                data = data.Replace("aps_0_aps", "'");
                data = data.Replace("ps_0_ps", "\"");

            }
            return data;
        }

        public static object ChangeType(object value, Type fieldType)
        {
            string stringValue = (value.GetType() == typeof(String)) ? (string)value : null;

            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            return Convert.ChangeType(value, fieldType);
        }

        public static bool ToBool(object input, bool valueIfNull)
        {
            if (input == null || input == DBNull.Value)
                return valueIfNull;

            return GetBooleanFlag(input.ToString());
        }

        public static bool GetBooleanFlag(string val)
        {
            bool obj = false;

            if (!string.IsNullOrWhiteSpace(val))
            {
                val = val.Trim().ToLower();
                if (val == "1" || val == "t" || val == "true" || val == "y" || val == "yes" || val == "ok")
                {
                    obj = true;
                }
            }

            return obj;
        }

        public static string ToString(object input)
        {
            return ToString(input, "");
        }

        public static string ToString(object input, string defaultValue)
        {
            if (input == null)
                return defaultValue;

            return input.ToString();
        }


        public static T GetData<T>(object value, T defaultValue)
        {
            if (value == null)
                return defaultValue;

            var typeCode = Type.GetTypeCode(typeof(T));

            if (typeof(T) == value.GetType())
                return (T)value;


            object objectValue = GetDataValue(value, typeCode);


            return (T)objectValue;// Convert.ChangeType(objectValue, typeof(T));
        }

        internal static object GetDataValue(object value, Type fieldType)
        {
            if (value == null)
                return null;

            if (value.GetType() == fieldType)
                return value;

            if (fieldType == typeof(Guid))
                return new Guid(value.ToString());

            if (fieldType.IsEnum)
            {
                if (!string.IsNullOrWhiteSpace(value.ToString()))
                    return Enum.Parse(fieldType, value.ToString().Replace(" ", ""));
            }

            return GetDataValue(value, Type.GetTypeCode(fieldType));
        }


        public static object GetDataValue(object value, TypeCode typeCode)
        {
            if (value == null)
            {
                return null;
            }

            object objectValue = null;
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    //objectValue = ModelBaseHelper.GetBooleanFlag(value.ToString());
                    break;
                case TypeCode.DateTime:
                    var dt = DateTime.MinValue;

                    if (value is DateTime)
                        dt = (DateTime)value;
                    //else
                    //    dt = Acidaes.CRMnext.Model.ModelBaseHelper.ParseCurrentDate(value.ToString());

                    objectValue = dt;

                    break;

                case TypeCode.Decimal:
                    if (value is decimal)
                    {
                        objectValue = value;
                    }
                    else
                    {
                        decimal doubleValue;
                        decimal.TryParse(value.ToString(), out doubleValue);

                        objectValue = doubleValue;
                    }
                    break;
                case TypeCode.Double:
                    if (value is double)
                    {
                        objectValue = value;
                    }
                    else
                    {
                        double doubleValue;
                        double.TryParse(value.ToString(), out doubleValue);

                        objectValue = doubleValue;
                    }
                    break;
                case TypeCode.Int64:

                    if (value is Int64)
                    {
                        objectValue = value;
                    }
                    else if (value is Int32 || value is Int16)
                    {
                        objectValue = Convert.ToInt64(value);
                    }
                    else
                    {
                        long longValue;
                        Int64.TryParse(value.ToString(), out longValue);

                        objectValue = longValue;
                    }


                    break;
                case TypeCode.Int32:
                    if (value is Int32)
                        objectValue = value;
                    else if (value is Int16 || value is decimal || value is double)
                    {
                        objectValue = Convert.ToInt32(value);
                    }
                    else
                    {
                        var valType = value.GetType();
                        int number = 0;

                        if (valType.IsEnum)
                            number = Convert.ToInt32(value);
                        else
                            int.TryParse(value.ToString(), out number);

                        objectValue = number;
                    }
                    break;
                case TypeCode.String:
                    objectValue = value.ToString();
                    break;
                default:
                    objectValue = value;
                    break;
            }
            return objectValue;
        }


        public static decimal GetDecimalRoundedValue(decimal value, short decimalPlaces)
        {
            return Math.Round(value, decimalPlaces);
        }


        public static bool SetData(object model, string propertyName, object value, bool clearDataIfNull = false)
        {
            var type = model.GetType();

            if (type == null || (!clearDataIfNull && value == null))
                return false;

            System.Reflection.PropertyInfo pinfo = type.GetProperty(propertyName);

            var field = type.GetField(propertyName);

            if (pinfo == null && field == null)
                return false;

            SetData(model, value, pinfo, field, clearDataIfNull);

            return true;
        }


        public static bool IsModelProperty(object model, string propertyName)
        {
            var type = model.GetType();

            if (type == null)
                return false;

            System.Reflection.PropertyInfo pinfo = type.GetProperty(propertyName);

            var field = type.GetField(propertyName);

            if (pinfo == null && field == null)
                return false;

            return true;
        }

        internal static void SetData(object obj, object value, PropertyInfo property, System.Reflection.FieldInfo field, bool clearDataIfNull = false)
        {
            Type fieldType = (property != null) ? property.PropertyType : field.FieldType;

            // value = GetNativeDefaultValue(value, clearDataIfNull, fieldType);

            if (value != null)
            {
                value = DataUtility.GetDataValue(value, fieldType);
            }

            if (value == null)
            {
                if (clearDataIfNull)
                {
                    value = GetNativeDefaultValue(fieldType);
                }
                else
                    return;
            }

            if (property != null)
            {
                if (property.GetSetMethod() != null)
                    property.SetValue(obj, value, null);
            }
            else
            {
                field.SetValue(obj, value);
            }
        }

        private static object GetNativeDefaultValue(Type fieldType)
        {
            var typeCode = Type.GetTypeCode(fieldType);

            return GetNativeDefaultVal(typeCode);
        }

        public static object GetNativeDefaultVal(TypeCode typeCode)
        {
            if (typeCode == TypeCode.Int32 || typeCode == TypeCode.Int64 || typeCode == TypeCode.Int16
                || typeCode == TypeCode.Double || typeCode == TypeCode.Decimal)
            {
                return 0;
            }

            if (typeCode == TypeCode.DateTime)
                return DateTime.MinValue;

            if (typeCode == TypeCode.Boolean)
                return false;

            if (typeCode == TypeCode.String)
                return "";

            return null;
        }

        public static bool IsValidNumeric(int data, int min, int max)
        {
            if (min == 0 && max == 0)
                return data > 0;

            return data >= min && data <= max;
        }

        public bool IsValidDecimal(decimal data, decimal min, decimal max)
        {
            if (min == 0 && max == 0)
                return data > 0;

            return data >= min && data <= max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sTime">In utc</param>
        /// <param name="eTime">In utc</param>
        /// <param name="timeOffSet"></param>
        /// <returns></returns>

        /// <summary>
        ///all time should be in same unit
        /// </summary>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public static bool CheckTimeConstraint(DateTime sTime, DateTime eTime, DateTime currentTime)
        {
            bool result = false;

            int sVal = (int)sTime.TimeOfDay.TotalMinutes;
            int eVal = (int)eTime.TimeOfDay.TotalMinutes;
            int cVal = (int)currentTime.TimeOfDay.TotalMinutes;

            if (sVal == 0 && eVal == 0)
            {
                return true;
            }

            if (sVal <= eVal)
            {
                if (cVal >= sVal && cVal <= eVal)
                {
                    result = true;
                }
            }
            else if ((sVal <= cVal && cVal >= eVal) ||
                     (cVal < sVal && cVal < eVal))
            {
                result = true;
            }

            return result;
        }


        public static List<int> ConvertToIntList(string[] list)
        {
            if (list == null)
                return null;

            List<int> numberList = new List<int>();

            foreach (var item in list)
            {
                numberList.Add(ToInt(item));
            }

            return numberList;
        }

        public static T ParseEnum<T>(string enumValue, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(enumValue))
                return defaultValue;

            return (T)Enum.Parse(typeof(T), enumValue, true);
        }

        public static long ToLong(object input, long valueIfNull)
        {
            long ret = valueIfNull;

            try
            {
                if (input != null)
                {
                    if (!long.TryParse(input.ToString(), out ret))
                        ret = valueIfNull;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ret;
        }

        public static ArrayList ConvertToArrayListOfInt(string commaSeperatedList)
        {
            ArrayList list = null;
            if (string.IsNullOrEmpty(commaSeperatedList))
                return null;

            string[] ruleFieldIDs = commaSeperatedList.Split(',');
            list = new ArrayList();
            try
            {
                for (int crt = 0; crt < ruleFieldIDs.Length; crt++)
                {
                    if (ruleFieldIDs[crt] != "")
                    {
                        list.Add(ToInt(ruleFieldIDs[crt]));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return list;
        }

        private const string HtmlCodePattern = @"(<br\/>)|(<b>)|(<p>)|(<br \/>)|(<strong>)|(<STRONG>)|(<span class='WebRupee' >)|(&lt;)|(&gt;)|(&apos;)|(&quot;)|(&amp;)|(&nbsp;)|(&#146;)|(&#39;)";

        private static System.Text.RegularExpressions.Regex RegexHtml = new System.Text.RegularExpressions.Regex(HtmlCodePattern,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);

        private const string HTMLFieldXssCodePattern = @"(<script)|(alert\()|(prompt\()|(confirm\()|(<body)|(<input)|(<iframe)|(<video)|(<math)|(<a)|(<embed)|(<object)|(<style)|(<img)|(</script)|(alert;)|(<svg)|(onerror)|(onload)|(!--)";
        private static System.Text.RegularExpressions.Regex htmlFieldregexXss;

        private static System.Text.RegularExpressions.Regex HtmlFieldregexXss
        {
            get
            {

                if (htmlFieldregexXss == null)
                {
                    htmlFieldregexXss = new System.Text.RegularExpressions.Regex(HTMLFieldXssCodePattern,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
                }

                return htmlFieldregexXss;
            }
        }
        public static bool IsPossibleHTMLFieldXSS(string displayText)
        {
            var hasXssTag = HtmlFieldregexXss.IsMatch(displayText);

            return hasXssTag;
        }

        public static object HtmlEncode(object val)
        {
            if (val != null)
            {
                if (val is string)
                    return HtmlEncode(val.ToString());
            }

            return val;
        }

        public static bool IsHtmlEncoded(string displayText)
        {
            if (!string.IsNullOrEmpty(displayText))
            {
                //if ((displayText.IndexOf("<") == -1 && displayText.IndexOf(">") == -1))
                //    return true;

                if (IsPossibleHTMLFieldXSS(displayText))
                {
                    return false;
                }


                var isMatched = RegexHtml.IsMatch(displayText);

                return isMatched;

            }

            return false;
        }

        public static string HtmlEncode(string text, bool encodeAgain = false)
        {


            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            if (!encodeAgain && IsHtmlEncoded(text)) return text;

            // text = text.Replace("&#39;", "'").Replace("&#146;", "'");
            text = HttpUtility.HtmlEncode(text);// HttpContext.Current.Server.HtmlEncode(text);
            text = text.Replace("&#39;", "'").Replace("&#146;", "'").Replace("&#160;", " "); //39 and 146 are apostrophe and 160 is the numeric value of &nbsp;
            return text;
        }
    }
}