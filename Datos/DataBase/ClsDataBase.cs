using System;
using System.Data;
using System.Data.SqlClient;

namespace Datos.DataBase
{
    public class ClsDataBase
    {
        #region Variables privadas
        /* conecta a la base de datos y que nos devuelve informacion */
        /* "_" el guion bajo declara que la variable esta bien declarada  */
        private SqlConnection _objSqlConnection; /* crea la conexion con la base de datos */
        private SqlDataAdapter _objSqlDataAdapter; /* Envia una consulta a la BD, hace una lectura a los datos */
        private SqlCommand _objSqlCommand; /* Envia comando CRUD la informacion de la BD */
        private DataSet _dsResultados; /* Es una lista de tablas, va a recibir los resultados y va almacenar los resultados */
        /* DataSet = conjunto de tablas que se eligen */
        /* Datatable = conjunto de registros que se agrupan en una tabla */
        private DataTable _dtParametros; /* Permite crear parametros */
        /* Parametros = caracteristicas */
        private string _nombreTabla, _nombreSP, _mensajeErrorDB, _valorScalar, _nombreDB; /* variables que necesitamos */
        /* valorScalar = registros de los crud que se generan */
        private bool _scalar; /* Permite elegir que procedimiento elegir */

        #endregion

        #region Variables públicas

        /* variables que nos permite usarlos dentro de la aplicacion */
        
        public SqlConnection ObjSqlConnection { get => _objSqlConnection; set => _objSqlConnection = value; }
        public SqlDataAdapter ObjSqlDataAdapter { get => _objSqlDataAdapter; set => _objSqlDataAdapter = value; }
        public SqlCommand ObjSqlCommand { get => _objSqlCommand; set => _objSqlCommand = value; }
        public DataSet DsResultados { get => _dsResultados; set => _dsResultados = value; }
        public DataTable DtParametros { get => _dtParametros; set => _dtParametros = value; }
        public string NombreTabla { get => _nombreTabla; set => _nombreTabla = value; }
        public string NombreSP { get => _nombreSP; set => _nombreSP = value; }
        public string MensajeErrorDB { get => _mensajeErrorDB; set => _mensajeErrorDB = value; }
        public string ValorScalar { get => _valorScalar; set => _valorScalar = value; }
        public string NombreDB { get => _nombreDB; set => _nombreDB = value; }
        public bool Scalar { get => _scalar; set => _scalar = value; }
        #endregion

        #region Constructores

        /*  
            Los constructores permite instanciar las clases para iniciar variables o metodos 
            Inicializa por defecto los siguientes parametros
         */

        public ClsDataBase()
        {
            DtParametros = new DataTable("SpParametros");
            DtParametros.Columns.Add("Nombre");
            DtParametros.Columns.Add("TipoDato");
            DtParametros.Columns.Add("Valor");

            NombreDB = "DB_BasePruebas";
            
        }
        #endregion

        #region Métodos privados
        /* metodos propios de la BD*/

        private void CrearConexionBaseDatos(ref ClsDataBase ObjDataBase)
        {
            switch (ObjDataBase.NombreDB)
            {
                case "DB_BasePruebas": /* Si necesito conectar a otra DB, se crea otro "case" */
                    ObjDataBase.ObjSqlConnection = new SqlConnection(Properties.Settings.Default.CadenaConexion_DB_BasePruebas); /* Conecta a la BD */
                    break;
                default:
                    break;
            }
        }
        private void ValidarConexionBaseDatos(ref ClsDataBase ObjDataBase)
        {
            /*verifica el estado de la DB */
            if( ObjDataBase.ObjSqlConnection.State == ConnectionState.Closed )
            {
                ObjDataBase.ObjSqlConnection.Open(); /* se abre la DB */
            }
            else
            {
                ObjDataBase.ObjSqlConnection.Close(); /* se cierra la DB */
                ObjDataBase.ObjSqlConnection.Dispose(); /* se borra de la memoria cache */
            }
        }
        private void AgregarParametros(ref ClsDataBase ObjDataBase)
        {
            if (ObjDataBase.DtParametros != null) /* preguntar si tiene parametros*/
            {
                SqlDbType TipoDatoSQL = new SqlDbType(); /* me permite recorrer la DB */

                foreach (DataRow item in ObjDataBase.DtParametros.Rows)  /* ejecuta y analiza las filas */
                {
                    switch (item[1])
                    {
                        /* enlista los tipos de datos que vayamos a usar */
                        case "1":
                            TipoDatoSQL = SqlDbType.Bit;
                            break;
                        case "2":
                            TipoDatoSQL = SqlDbType.TinyInt;
                            break;
                        case "3":
                            TipoDatoSQL = SqlDbType.SmallInt;
                            break;
                        case "4":
                            TipoDatoSQL = SqlDbType.Int;
                            break;
                        case "5":
                            TipoDatoSQL = SqlDbType.BigInt;
                            break;
                        case "6":
                            TipoDatoSQL = SqlDbType.Decimal;
                            break;
                        case "7":
                            TipoDatoSQL = SqlDbType.SmallMoney;
                            break;
                        case "8":
                            TipoDatoSQL = SqlDbType.Money;
                            break;
                        case "9":
                            TipoDatoSQL = SqlDbType.Float;
                            break;
                        case "10":
                            TipoDatoSQL = SqlDbType.Real;
                            break;
                        case "11":
                            TipoDatoSQL = SqlDbType.Date;
                            break;
                        case "12":
                            TipoDatoSQL = SqlDbType.Time;
                            break;
                        case "13":
                            TipoDatoSQL = SqlDbType.SmallDateTime;
                            break;
                        case "14":
                            TipoDatoSQL = SqlDbType.Char;
                            break;
                        case "15":
                            TipoDatoSQL = SqlDbType.NChar;
                            break;
                        case "16":
                            TipoDatoSQL = SqlDbType.VarChar;
                            break;
                        case "17":
                            TipoDatoSQL = SqlDbType.NVarChar;
                            break;
                        
                        default:
                            break;
                    }
                    if (ObjDataBase.Scalar) /* analiza un parametro, para ver si esta vacio(si lo esta devuelve un nulo, caso contrario se asigan un valor) */
                    {
                        if (item[2].ToString().Equals(string.Empty))
                        {
                            ObjDataBase.ObjSqlCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = DBNull.Value; /* retorna un nulo */ 
                        }
                        else
                        {
                            ObjDataBase.ObjSqlCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = item[2].ToString(); /* retorna un Valor */
                        }
                    }
                    else /* analiza un selectcomand(proceso que esta solicitando de acuerdo a los parametros), si esta vacio o se le asigna datos */
                    {
                        if(item[2].ToString().Equals(string.Empty))
                        {
                            ObjDataBase.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = DBNull.Value; /* retorna un nulo */
                        }
                        else
                        {
                            ObjDataBase.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = item[2].ToString(); /* retorna un Valor */
                        }
                    }
                }
            }
            
        }
        private void PrepararConexionBaseDatos(ref ClsDataBase ObjDataBase)
        {
            /* llamamos a las funciones creadas */
            CrearConexionBaseDatos(ref ObjDataBase); 
            ValidarConexionBaseDatos(ref ObjDataBase);
        }
        private void EjecutarDataAdapter(ref ClsDataBase ObjDataBase)
        {
            try
            {
                /* Creamos la data resultante, para crear el DataSet  */
                PrepararConexionBaseDatos(ref ObjDataBase);
                ObjDataBase.ObjSqlDataAdapter = new SqlDataAdapter(ObjDataBase.NombreSP, ObjDataBase.ObjSqlConnection);
                ObjDataBase.ObjSqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                AgregarParametros(ref ObjDataBase);
                ObjDataBase.DsResultados = new DataSet();
                ObjDataBase.ObjSqlDataAdapter.Fill(ObjDataBase.DsResultados, ObjDataBase.NombreTabla);
            }
            catch (Exception ex)
            {
                /* Mensaje de error */
                ObjDataBase.MensajeErrorDB = ex.Message.ToString();
            }
            finally
            {
                /* si esta la conexion abierta se ejecuta la funcion de validar la conexion. */
                /* Mantiene activa siempre la conexion con la DB */
                if ( ObjDataBase.ObjSqlConnection.State == ConnectionState.Open)
                {
                    ValidarConexionBaseDatos(ref ObjDataBase);
                }
            }
        }
        private void EjecutarCommand(ref ClsDataBase ObjDataBase)
        {
            try
            {
                PrepararConexionBaseDatos(ref ObjDataBase);
                ObjDataBase.ObjSqlCommand = new SqlCommand(ObjDataBase.NombreSP, ObjDataBase.ObjSqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                AgregarParametros(ref ObjDataBase);
                if (ObjDataBase.Scalar)
                {
                    ObjDataBase.ValorScalar = ObjDataBase.ObjSqlCommand.ExecuteScalar().ToString().Trim();
                }
                else
                {
                    ObjDataBase.ObjSqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ObjDataBase.MensajeErrorDB = ex.Message.ToString();
            }
            finally
            {
                if (ObjDataBase.ObjSqlConnection.State == ConnectionState.Open)
                {
                    ValidarConexionBaseDatos(ref ObjDataBase);
                }
            }
        }

        #endregion

        #region Métodos públicos

            /* metodos propios de la aplicacion*/
            /* Permite hacer el CRUD en la DB */
        public void CRUD(ref ClsDataBase ObjDataBase)
        {
            if (ObjDataBase.Scalar)
            {
                EjecutarCommand(ref ObjDataBase);
            }
            else
            {
                EjecutarDataAdapter(ref ObjDataBase);
            }
        }
        #endregion

    }

    
}
