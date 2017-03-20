using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


    #region Conexión a BD

    public class SQLConection
    {
        #region Variables

        public string connectionString;
        SqlCommand command = new SqlCommand();
        SqlConnection connection = new SqlConnection();
        List<SqlParameter> parameters = new List<SqlParameter>();

        #endregion

        #region Propiedades

        /// <summary>
        /// Cadena de Conexion
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        /// <summary>
        /// Lista de Parametros
        /// </summary>
        public List<SqlParameter> Parametros
        {
            get { return parameters; }
            set { parameters = value; }
        }


        #endregion

        #region SQLConection

        public SQLConection()
        {
            //this.connectionString = ConfigurationManager.ConnectionStrings["DEFAULT"].ConnectionString;
            this.connectionString = "<add name='INTEGRAPRODUCCIONI' connectionString='Data Source=house-targaryen.database.windows.net;Initial Catalog=DB;User ID=Test0d1b; Password=eBL4H3RS;Connection Lifetime=0;' providerName='System.Data.SqlClient'/>";
        }

        

        #endregion


        

        #region Publics

        /// <summary>
        /// <para>Ejecuta una instrucción de Transact-SQL en la conexión y devuelve el número de filas afectadas.</para>
        /// </summary>
        /// <param name="query">Instrucción de Transact-SQL que será ejecutada.</param>
        /// <remarks><para>Es necesario asignar la cedena de conexión para poder invocar este método.</para></remarks>
        /// <returns>Devuelve un <c>int32</c> que indica el número de filas afectadas.</returns>
        /// <exception cref="SqlException"> Si ocurrió un error en la conexión.</exception>
        public int ExecuteNonQuery(string query)
        {
            try
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                if (query == string.Empty)
                    throw new Exception("El query esta vacio");

                command.CommandTimeout = 0;
                command.CommandText = query;
                command.Connection = connection;
                return command.ExecuteNonQuery();
            }
            catch (Exception oe)
            {
                
                throw oe;
                

            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// <para>Ejecuta un procedimiento almacenado de Transact-SQL en la conexión y devuelve el número de filas afectadas.</para>
        /// </summary>
        /// <param name="storeProcedure">Nombre del procedimiento almacenado que se ejecutará.</param>
        /// <param name="withParameters">Indica si la conexión debe usar los parametros para ejecutar el procedimiento almacenado.</param>
        /// <remarks>
        /// <para>Es necesario asignar la cedena de conexión y los parametros correspondientes para poder invocar este método.</para>
        /// <para>Se debe hacer uso de la propiedad <c>Parameters</c> para asignar los parametros del procedimiento almacenado.</para>
        /// </remarks>
        /// <returns>Devuelve un <c>int32</c> que indica el número de filas afectadas.</returns>
        /// <exception cref="SqlException"> Si ocurrió un error en la conexión.</exception>
        public int ExecuteNonQuery(string storeProcedure, bool withParameters)
        {
            try
            {
                connection.ConnectionString = connectionString;

                connection.Open();

                if (storeProcedure == string.Empty)
                    throw new Exception("El storeProcedure esta vacio");

                command.CommandText = storeProcedure;
                command.Parameters.Clear();

                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                return command.ExecuteNonQuery();
            }
            catch (Exception oe)
            {

                throw oe;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// <para>Ejecuta una instrucción de consulta en la conexión y devuelve el resultado en un <c>DataTable</c>.</para>
        /// </summary>
        /// <param name="query">Instrucción de consulta que será ejecutada.</param>
        /// <remarks><para>Es necesario asignar la cedena de conexión para poder invocar este método.</para></remarks>
        /// <returns>Devuelve un <c>DataTable</c> con los resultados de la consulta.</returns>
        /// <exception cref="SqlException"> Si ocurrió un error en la conexión.</exception>
        public DataTable ExecuteQuery(string query)
        {
            try
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                if (query == string.Empty)
                    throw new Exception("El query esta vacio");

                command.CommandText = query;
                command.CommandTimeout = 0;
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// <para>Ejecuta un procedimiento almacenado de consulta en la conexión y devuelve el resultado en un <c>DataTable</c>.</para>
        /// </summary>
        /// <param name="storeProcedure">Nombre del procedimiento almacenado que se ejecutará.</param>
        /// <param name="withParameters">Indica si la conexión debe usar los parametros para ejecutar el procedimiento almacenado.</param>
        /// <remarks>
        /// <para>Es necesario asignar la cedena de conexión y los parametros correspondientes para poder invocar este método.</para>
        /// <para>Se debe hacer uso de la propiedad <c>Parameters</c> para asignar los parametros del procedimiento almacenado.</para>
        /// </remarks>
        /// <returns>Devuelve un <c>DataTable</c> con los resultados de la consulta.</returns>
        /// <exception cref="SqlException"> Si ocurrió un error en la conexión.</exception>
        public DataTable ExecuteProcedure(string storeProcedure, bool withParameters)
        {
            try
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                if (storeProcedure == string.Empty)
                    throw new Exception("El storeProcedure esta vacio");

                command.CommandText = storeProcedure;
                command.Parameters.Clear();
                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                command.CommandTimeout = 0;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();

                adapter.Fill(ds);
                if (ds.Tables.Count != 0)
                    return ds.Tables[0];
                else
                    return new DataTable();
            }
            catch (SqlException oe)
            {
                throw oe;

            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// <para>Ejecuta un procedimiento almacenado de consulta en la conexión y devuelve el resultado en un <c>DataTable</c>.</para>
        /// </summary>
        /// <param name="storeProcedure">Nombre del procedimiento almacenado que se ejecutará.</param>
        /// <param name="withParameters">Indica si la conexión debe usar los parametros para ejecutar el procedimiento almacenado.</param>
        /// <remarks>
        /// <para>Es necesario asignar la cedena de conexión y los parametros correspondientes para poder invocar este método.</para>
        /// <para>Se debe hacer uso de la propiedad <c>Parameters</c> para asignar los parametros del procedimiento almacenado.</para>
        /// </remarks>
        /// <returns>Devuelve un <c>DataTable</c> con los resultados de la consulta.</returns>
        /// <exception cref="SqlException"> Si ocurrió un error en la conexión.</exception>
        public DataSet ExecuteStoredProcedure(string storeProcedure, bool withParameters)
        {
            try
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                if (storeProcedure == string.Empty)
                    throw new Exception("El storeProcedure esta vacio");

                command.CommandText = storeProcedure;
                command.Parameters.Clear();
                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Connection = connection;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();

                adapter.Fill(ds);
                if (ds.Tables.Count != 0)
                    return ds;
                else
                    return new DataSet();
            }
            catch (SqlException oe)
            {
                throw oe;
            }
            finally
            {
                connection.Close();
            }
        }

        #endregion

        

        

        
    }

    #endregion
