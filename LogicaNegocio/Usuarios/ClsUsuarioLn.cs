using Datos.DataBase;
using Entidades.Usuarios;
using System;
using System.Data;

namespace LogicaNegocio.Usuarios
{
    public class ClsUsuarioLn
    {
        #region Variable Privada 
        /* verifica que este en null la BD */
        private ClsDataBase ObjDataBase = null; /*null= reinicia la BD, para verla por defecto */
        #endregion

        #region Metodo Index
        /* sirve para buscar en la BD a traves un puntero, y lo presenta */

        public void Index(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP ="[SCH_GENERAL].[SP_Usuarios_Index]",
                Scalar = false
            };
            Ejecutar(ref ObjUsuario);
        }
        #endregion
        #region CRUD Usuario

        /*acciones que me permiten hacer en el CRUD */
        public void Create(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Create]",
                Scalar = true
            };
            ObjDataBase.DtParametros.Rows.Add(@"@Nombre", "17", ObjUsuario.Nombre);
            ObjDataBase.DtParametros.Rows.Add(@"@Apellido1", "17", ObjUsuario.Apellido1);
            ObjDataBase.DtParametros.Rows.Add(@"@Apellido2", "17", ObjUsuario.Apellido2);
            ObjDataBase.DtParametros.Rows.Add(@"@FechaNacimiento", "13", ObjUsuario.FechaNacimiento);
            ObjDataBase.DtParametros.Rows.Add(@"@Estado", "1", ObjUsuario.Estado);
            
            Ejecutar(ref ObjUsuario);
        }

        public void Read(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Read]",
                Scalar = false
            };
            ObjDataBase.DtParametros.Rows.Add(@"@IdUsuario", "2", ObjUsuario.IdUsuario);

            Ejecutar(ref ObjUsuario);
        }

        public void Update(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Update]",
                Scalar = true
            };
            ObjDataBase.DtParametros.Rows.Add(@"@IdUsuario", "2", ObjUsuario.IdUsuario);
            ObjDataBase.DtParametros.Rows.Add(@"@Nombre", "17", ObjUsuario.Nombre);
            ObjDataBase.DtParametros.Rows.Add(@"@Apellido1", "17", ObjUsuario.Apellido1);
            ObjDataBase.DtParametros.Rows.Add(@"@Apellido2", "17", ObjUsuario.Apellido2);
            ObjDataBase.DtParametros.Rows.Add(@"@FechaNacimiento", "13", ObjUsuario.FechaNacimiento);
            ObjDataBase.DtParametros.Rows.Add(@"@Estado", "1", ObjUsuario.Estado);

            Ejecutar(ref ObjUsuario);
        }

        public void Delete(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Delete]",
                Scalar = true
            };
            ObjDataBase.DtParametros.Rows.Add(@"@IdUsuario", "2", ObjUsuario.IdUsuario);
            
            Ejecutar(ref ObjUsuario);
        }
        #endregion
        #region Metodos Privados
        private void Ejecutar(ref ClsUsuario ObjUsuario)
        {
            ObjDataBase.CRUD(ref ObjDataBase);

            if (ObjDataBase.MensajeErrorDB == null)
            {
                if (ObjDataBase.Scalar)
                {
                    ObjUsuario.ValorScalar = ObjDataBase.ValorScalar;
                }
                else
                {
                    ObjUsuario.DtResultados = ObjDataBase.DsResultados.Tables[0];
                    if (ObjUsuario.DtResultados.Rows.Count == 1 )
                    {
                        foreach (DataRow item in ObjUsuario.DtResultados.Rows)
                        {
                            ObjUsuario.IdUsuario = Convert.ToByte(item["IdUsuario"].ToString());
                            ObjUsuario.Nombre = item["Nombre"].ToString();
                            ObjUsuario.Apellido1 = item["Apellido1"].ToString();
                            ObjUsuario.Apellido2 = item["Apellido2"].ToString();
                            ObjUsuario.FechaNacimiento = Convert.ToDateTime(item["FechaNacimiento"].ToString());
                            ObjUsuario.Estado = Convert.ToBoolean(item["Estado"].ToString());
                        }
                    }
                }
            }
            else
            {
                ObjUsuario.MensajeError = ObjDataBase.MensajeErrorDB;
            }
        }
        #endregion
    }
}
