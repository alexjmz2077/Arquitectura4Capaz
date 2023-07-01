using Entidades.Usuarios;
using LogicaNegocio.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Presentacion.Formularios
{
    public partial class FrmUsuario : Form
    {
        private ClsUsuario ObjUsuario = null;
        private readonly ClsUsuarioLn ObjUsuarioLn = new ClsUsuarioLn(); 
        public FrmUsuario()
        {
            InitializeComponent();
            CargaListaUsuarios();
        }

        private void CargaListaUsuarios()
        {
            ObjUsuario = new ClsUsuario();
            ObjUsuarioLn.Index(ref ObjUsuario);
            if(ObjUsuario.MensajeError == null)
            {
                DgvUsuarios.DataSource = ObjUsuario.DtResultados;
            }
            else
            {
                MessageBox.Show(ObjUsuario.MensajeError, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {

        }

        private void TxtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            //CONSTRUCCION DEL OBJETO
            ObjUsuario = new ClsUsuario()
            {
                Nombre = TxtNombre.Text,
                Apellido1 = TxtApellido1.Text,
                Apellido2 = TxtApellido2.Text,
                FechaNacimiento = DtpFechaNacimiento.Value,
                Estado = ChkEstado.Checked
            };
            ObjUsuarioLn.Create(ref ObjUsuario);
            if (ObjUsuario.MensajeError == null)
            {
                MessageBox.Show("El ID: " + ObjUsuario.ValorScalar + " fue agregado correctamente :) ");
                //Actualiza la lista
                CargaListaUsuarios();
            }
            else
            {
                MessageBox.Show(ObjUsuario.MensajeError, "Mensaje de Error :(", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBorrar_Click(object sender, EventArgs e)
        {
            ObjUsuario = new ClsUsuario()
            {
                IdUsuario = Convert.ToByte(LblidUsuario.Text)
            };
            ObjUsuarioLn.Delete(ref ObjUsuario);
            CargaListaUsuarios();
        }

        private void DgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (DgvUsuarios.Columns[e.ColumnIndex].Name == "Editar")
                {
                    ObjUsuario = new ClsUsuario()
                    {
                        IdUsuario = Convert.ToByte(DgvUsuarios.Rows[e.RowIndex].Cells["IdUsuario"].Value.ToString())
                    };

                    LblidUsuario.Text = ObjUsuario.IdUsuario.ToString();    

                    ObjUsuarioLn.Read(ref ObjUsuario);

                    TxtNombre.Text = ObjUsuario.Nombre;
                    TxtApellido1.Text = ObjUsuario.Apellido1;
                    TxtApellido2.Text = ObjUsuario.Apellido2;
                    DtpFechaNacimiento.Value = ObjUsuario.FechaNacimiento;
                    ChkEstado.Checked = ObjUsuario.Estado;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            ObjUsuario = new ClsUsuario()
            {
                IdUsuario = Convert.ToByte(LblidUsuario.Text),
                Nombre = TxtNombre.Text,
                Apellido1 = TxtApellido1.Text,
                Apellido2 = TxtApellido2.Text,
                FechaNacimiento = DtpFechaNacimiento.Value,
                Estado = ChkEstado.Checked
            };
            ObjUsuarioLn.Update(ref ObjUsuario);

            if (ObjUsuario.MensajeError == null)
            {
                MessageBox.Show("El registro fue actualizado correctamente.");

                //Actualiza la lista
                CargaListaUsuarios();
            }
            else
            {
                MessageBox.Show(ObjUsuario.MensajeError, "Mensaje de Error :(", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
