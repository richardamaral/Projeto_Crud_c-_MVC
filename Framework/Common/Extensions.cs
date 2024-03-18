using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common
{
    public class Comum
    {
        public static DateTime LastDayOfCurrentMonth()
        {
            DateTime today = DateTime.Today;
            DateTime lastDayOfThisMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
            return lastDayOfThisMonth;
        }

        public static DateTime LastDayOfMonth(DateTime pDate)
        {
            DateTime lastDayOfThisMonth = new DateTime(pDate.Year, pDate.Month, 1).AddMonths(1).AddDays(-1);
            return lastDayOfThisMonth;
        }
    }

    public static class Extensions
    {
        public static T Cast<T>(this object obj)
        {
            try
            {
                return (T)(obj);
            }
            catch
            {
                return Parse<T>(obj);
            }
        }


        public static int ToInt(this string input)
        {
            int data = 0;
            int.TryParse(input, out data);
            return data;
        }

        public static decimal ToDecimal(this string input)
        {
            decimal data = 0;
            decimal.TryParse(input, out data);
            return data;
        }

        public static double ToDouble(this string input)
        {
            double data = 0;
            double.TryParse(input, out data);
            return data;
        }

        public static float ToFloat(this string input)
        {
            float data = 0;
            float.TryParse(input, out data);
            return data;
        }

        public static bool ToBool(this string input)
        {
            bool data = false;
            bool.TryParse(input, out data);
            return data;
        }

        public static DateTime ToDate(this string input)
        {
            DateTime data = DateTime.MinValue;
            DateTime.TryParse(input, out data);
            return data;
        }


        public static int ToInt(this object input)
        {
            int data = 0;
            int.TryParse(input + "", out data);
            return data;
        }

        public static int TosDateInt(this string input)
        {
            int data = 0;

            if (!string.IsNullOrEmpty(input))
                if (input.Length == 10)
                    data = (input.Substring(6) + "" + input.Substring(3, 2) + "" + input.Substring(0, 2)).ToInt();//ano+mes+dia

            return data;
        }

        public static int TosDate2Int(this string input)
        {
            int data = 0;

            if (!string.IsNullOrEmpty(input))
                if (input.Length == 10)
                    data = (input.Replace("-", "")).ToInt();//ano+mes+dia

            return data;
        }

        public static double ToDouble(this object input)
        {
            double data = 0;
            double.TryParse(input + "", out data);
            return data;
        }

        public static string ToText(this object input)
        {
            return input + "";
        }

        public static decimal ToDecimal(this object input)
        {
            decimal data = 0;
            decimal.TryParse(input + "", out data);
            return data;
        }

        public static DateTime ToDate(this object input)
        {

            DateTime data = DateTime.MinValue;
            DateTime.TryParse(input + "", out data);
            return data;
        }



        public static string toStrZero(this string input)
        {
            string data = "0";
            try
            {
                if (!string.IsNullOrEmpty(input))
                    if (toClearsJunk(input.Replace("'", "")) != "")
                        data = toClearsJunk(input.Replace("'", ""));
            }
            catch (Exception)
            {
                data = "0";
            }

            return data;

        }

        public static string toStr(this string input)
        {
            string data = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(input))
                    data = string.Empty;
                else
                    if (toClearsJunk(input.Replace("'", "")) != "")
                    data = toClearsJunk(input.Replace("'", ""));
            }
            catch (Exception)
            {
                data = string.Empty;
            }

            return data;

        }

        public static string ToStrNIdInt(this string input)
        {
            string data = string.Empty;
            try
            {
                if (input == null)
                    data = "0";
                else if (input == "0")
                    data = "0";
                else
                    if (input.Contains(","))
                {
                    string[] array = input.Split(',');
                    for (int i = 0; i < array.Count(); i++)
                    {
                        data += string.IsNullOrEmpty(data) ? array[i].ToInt().ToString() : "," + array[i].ToInt().ToString();
                    }
                }
                else
                    data = input.ToInt().ToString();
            }
            catch (Exception)
            {
                data = "0";
            }

            return data;

        }

        public static string toClearsJunk(this string input)
        {
            string lixo = null;
            string[] LimpaTexto;
            string data = null;

            lixo = "select|SELECT|update|UPDATE|drop|DROP|;|--|insert|INSERT|delete|DELETE|XP_|xp_";
            LimpaTexto = lixo.Split('|');
            data = input;

            for (int i = 0; i < LimpaTexto.Length; i++)
            {
                data = data.Replace(LimpaTexto[i], "");
            }

            return data;
        }

        public static string toFormatCnpjCpfRg(this string input)
        {
            string data = input;
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    data = data.ToLower().Replace("x", "0");
                    switch (data.Length)
                    {
                        case 14: //CNPJ
                            data = Convert.ToUInt64(data).ToString(@"00\.000\.000\/0000\-00");
                            break;
                        case 11: //CPF
                            data = Convert.ToUInt64(data).ToString(@"000\.000\.000\-00");
                            break;
                        case 9: //RG
                            data = Convert.ToUInt64(data).ToString(@"00\.000\.000\-0");
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }

            return data;
        }

        public static bool ToIn(this string id, string ids)
        {
            if (!string.IsNullOrEmpty(ids))
                if (ids.Contains(","))
                {
                    var array = ids.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i] == id)
                            return true;
                    }
                }
                else
                     if (ids == id)
                    return true;

            return false;
        }

        //// remove "this" if not on C# 3.0 / .NET 3.5 
        public static DataTable ToDataTable<T>(this IList<T> data, string[] FieldsToExclude = null)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            if (FieldsToExclude != null)
                foreach (var s in FieldsToExclude)
                    table.Columns.Remove(s);

            return table;
        }

        public static string toRemoveAccents(this string input)
        {
            if (!string.IsNullOrEmpty(input))
                input = input.Replace("'", "").
                            Replace("Á", "A").
                            Replace("á", "a").
                            Replace("Â", "A").
                            Replace("â", "a").
                            Replace("Ã", "A").
                            Replace("ã", "a").
                            Replace("À", "A").
                            Replace("à", "a").
                            Replace("É", "E").
                            Replace("é", "e").
                            Replace("Ê", "E").
                            Replace("ê", "e").
                            Replace("È", "E").
                            Replace("è", "e").
                            Replace("Ë", "E").
                            Replace("Ì", "I").
                            Replace("í", "i").
                            Replace("Î", "I").
                            Replace("î", "i").
                            Replace("Ì", "I").
                            Replace("ì", "i").
                            Replace("Ó", "O").
                            Replace("ó", "o").
                            Replace("Ô", "O").
                            Replace("ô", "o").
                            Replace("Õ", "O").
                            Replace("õ", "o").
                            Replace("Ò", "O").
                            Replace("ò", "o").
                            Replace("Ú", "U").
                            Replace("ú", "u").
                            Replace("Û", "U").
                            Replace("û", "u").
                            Replace("Ù", "U").
                            Replace("ù", "u").
                            Replace("Ü", "U").
                            Replace("ü", "u").
                            Replace("Ç", "C").
                            Replace("ç", "c");
            else
                input = string.Empty;

            return input;

        }

        public static string ToRemovePunctuation(this string input)
        {
            input = input.Replace("?", "").
                            Replace("&", "e").
                            Replace("$", "").
                            Replace("(", "").
                            Replace(")", "").
                            Replace("+", "").
                            Replace("^", "").
                            Replace("/", "").
                            Replace("\"", "").
                            Replace("*", "").
                            Replace("#", "").
                            Replace("'", "").
                            Replace("´", "").
                            Replace(",", "").
                            Replace("!", "").
                            Replace(":", "").
                            Replace(";", "").
                            Replace("%", "porcento").
                            Replace("  ", " ").
                            Replace("   ", " ").
                            Replace(" ", "-").
                            Replace("---", "-").
                            Replace("°", "").
                            Replace("ª", "").
                            Replace("...", "").
                            Replace("..", "").
                            Replace("º", "").
                            Replace("\t", "").
                            Replace("\n", "").
                            Replace(@"\", "");

            return input;
        }

        public static string ToRemovePonto(this string input)
        {
            input = input.Replace(". ", "-").
                            Replace(".", "-");

            return input;
        }

        public static string ToClearCnpjCpf(this string input)
        {
            input = input.Replace(".", "")
                         .Replace("/", "")
                         .Replace("-", "");

            return input;
        }


        #region "Conversion Functions"
        public static object Parse(object input, string type)
        {
            switch (type)
            {
                case "System.Boolean":
                    if (input.GetType().FullName == "System.String")
                        input = Parse<int>(input);
                    break;

                default:
                    break;
            }
            try
            {
                return Convert.ChangeType(input, Type.GetType(type));
            }
            catch
            {
                if (input.GetType().IsPrimitive)
                    return 0;
                else
                    return null;
            }
        }

        public static object Parse(object input, Type type)
        {
            if (type == null)
                throw new ArgumentNullException("conversionType is NULL");

            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (input.Equals(DBNull.Value) || input == null || type == null)
                    return null;

                NullableConverter nullableConverter = new NullableConverter(type);
                type = nullableConverter.UnderlyingType;
            }
            else if (input.Equals(DBNull.Value) || input == null)
            {
                throw new ArgumentNullException("input value is NULL, but the destination type is not nullable. You are missing '?' key in variable type? (ex: int?, DateTime?)");
            }

            return Parse(input, type.FullName);
        }

        public static T Parse<T>(object input)
        {
            return (T)Parse(input, typeof(T));
        }
        #endregion


    }
}
