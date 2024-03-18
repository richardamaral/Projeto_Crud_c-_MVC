using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Framework.DataAcess
{
    public static class GenericDA
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Pcl"].ConnectionString; }
        }

        public static T Get<T>(object obj)
        {
            return Get<T>(obj, null);
        }
        public static T Get<T>(object obj, string procedureName)
        {
            if (string.IsNullOrEmpty(procedureName))
                procedureName = string.Format("Get{0}", obj.GetType().Name);

            SqlParameter[] param = GetParameter(obj, procedureName);
            return CommonDA.ExecuteDataTable(procedureName, param, CommandType.StoredProcedure).ToList<T>(obj.GetType());
        }

        public static List<T> GetReader<T>(object obj, string procedureName)
        {
            SqlParameter[] param = GetParameter(obj, procedureName);
            List<T> list = new List<T>();

            var reader = CommonDA.ExecuteReader(procedureName, param, CommandType.StoredProcedure);

            while (reader.Read())
            {
                T item = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] properties = ((Type)item.GetType()).GetProperties();

                for (int j = 0; j < reader.FieldCount; j++)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        if (reader.GetName(j) == properties[i].Name)
                        {
                            if (reader[j] != DBNull.Value)
                            {
                                //string tipo = reader.GetDataTypeName(j);
                                if (reader.GetDataTypeName(j).Equals("bigint"))
                                    properties[i].SetValue(item, Convert.ToInt32(reader[j]), null);
                                else
                                    properties[i].SetValue(item, reader[j], null);
                                break;
                            }
                        }
                    }
                }
                list.Add(item);
            }

            reader.Close();

            return list.ToList();
        }

        public static T GetQuery<T>(object obj, string Query)
        {
            return CommonDA.ExecuteDataTable(Query, null, CommandType.Text).ToList<T>(obj.GetType());
        }

        public static T Set<T>(object obj)
        {
            return Set<T>(obj, null);
        }
        public static T Set<T>(object obj, string procedureName)
        {
            if (string.IsNullOrEmpty(procedureName))
                procedureName = string.Format("Set{0}", obj.GetType().Name);

            SqlParameter[] param = GetParameter(obj, procedureName);
            return Parse<T>(CommonDA.ExecuteScalar(procedureName, param, CommandType.StoredProcedure));
        }

        #region "Parse Objects"
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

            return Convert.ChangeType(input, Type.GetType(type));
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
            else
            {
                if (input.Equals(DBNull.Value) || input == null)
                    throw new ArgumentNullException("input value is NULL, but the destination type is not nullable. You are missing '?' key in variable type? (ex: int?, DateTime?)");
            }

            return Parse(input, type.FullName);
        }
        public static T Parse<T>(object input)
        {
            return (T)Parse(input, typeof(T));
        }
        #endregion

        #region "Factory Objects"
        public static SqlParameter[] GetParameter(object obj, string procedureName)
        {
            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(ConnectionString, procedureName);
            Type inputType = obj.GetType();

            foreach (SqlParameter parameter in parameters)
            {
                PropertyInfo property = inputType.GetProperty(parameter.ParameterName.Replace("@", ""));

                if (property == null)
                    property = inputType.GetProperty(parameter.ParameterName.Replace("@", "").ToUpper());

                if (property == null)
                    property = inputType.GetProperty(parameter.ParameterName.Replace("@", "").ToLower());

                if (property != null)
                    parameter.Value = property.GetValue(obj, null);
            }

            return parameters;
        }

        /// <summary>
        /// Popula dinamicamente uma lista 
        /// </summary>
        /// <param name="ContentType"></param>
        /// <param name="ListType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T ToList<T>(this DataTable data, Type ContentType)
        {
            object list = Activator.CreateInstance<T>();

            foreach (DataRow r in data.Rows)
            {
                object item = Activator.CreateInstance(ContentType);
                item = FactoryObject(ContentType, r);
                list.GetType().InvokeMember("Add", BindingFlags.InvokeMethod, null, list, new object[] { item });
            }
            return (T)list;
        }

        /// <summary>
        /// Cria e retorna dinamicamente um objeto
        /// </summary>
        /// <param name="ContentType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object FactoryObject(Type ContentType, DataRow data)
        {
            object item = Activator.CreateInstance(ContentType);

            if (item != null)
            {
                foreach (var prop in item.GetType().GetProperties())
                {
                    if (data.Table.Columns.Contains(prop.Name) && !data.IsNull(prop.Name) && prop.CanWrite)
                    {
                        prop.SetValue(item, Parse(data[prop.Name], prop.PropertyType), null);
                    }
                }
            }

            return item;
        }
        #endregion
    }

    public sealed class SqlHelperParameterCache
    {
        #region private methods, variables, and constructors

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new SqlHelperParameterCache()".
        private SqlHelperParameterCache() { }

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// resolve at run time the appropriate set of SqlParameters for a stored procedure
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
        /// <returns></returns>
        private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(spName, cn))
            {
                
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                SqlCommandBuilder.DeriveParameters(cmd);

                if (!includeReturnValueParameter)
                {
                    cmd.Parameters.RemoveAt(0);
                }

                SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count]; ;

                cmd.Parameters.CopyTo(discoveredParameters, 0);

                return discoveredParameters;
            }
        }

        //deep copy of cached SqlParameter array
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion private methods, variables, and constructors

        #region caching functions

        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters to be cached</param>
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// retrieve a parameter array from the cache
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <returns>an array of SqlParamters</returns>
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;

            SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion caching functions

        #region Parameter Discovery Functions

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <returns>an array of SqlParameters</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <remarks>
        /// This method will query the database for this information, and then store it in a cache for future requests.
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>an array of SqlParameters</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            string hashKey = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            SqlParameter[] cachedParameters;

            cachedParameters = (SqlParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                cachedParameters = (SqlParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
            }

            return CloneParameters(cachedParameters);
        }

        #endregion Parameter Discovery Functions
    }
}
