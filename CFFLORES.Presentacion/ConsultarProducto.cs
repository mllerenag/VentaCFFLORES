using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace CFFLORES.Presentacion
{
    public partial class ConsultarProducto : Form
    {

        public int idProducto;
        public string desProducto;
        public ConsultarProducto()
        {
            InitializeComponent();
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProductoWSC.ProductoClient proxy = new ProductoWSC.ProductoClient();
            string codigobarra = txtCodigoBarra.Text;
            string nombre = txtDescripcion.Text;
            string tipo = (cboTipo.Text.Equals("Ninguno") )? "" : cboTipo.Text;
            try
            {

                ProductoWSC.EProducto[] ObProducto = proxy.ObtenerProducto(codigobarra, nombre, tipo);
                dgvProducto.AutoGenerateColumns = false;
                dgvProducto.DataSource = ObProducto;


            }
            catch (FaultException<ProductoWSC.ProductoInexistente> error)
            {
                /*Por error se valida los datos*/
                if (error.Detail.exCodigo == 1)
                    MessageBox.Show(error.Reason.ToString(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                else
                    MessageBox.Show(error.Reason.ToString(),
                   "Adventencia",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Exclamation,
                   MessageBoxDefaultButton.Button1);
            }
        }

        private void ConsultarProducto_Load(object sender, EventArgs e)
        {
            cboTipo.SelectedIndex = 0;


        }

        private void dgvProducto_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string idproducto = dgvProducto[0, e.RowIndex].Value.ToString();
            string codigobarra = dgvProducto[1, e.RowIndex].Value.ToString();
            MessageBox.Show(idproducto);
        }
    }
}
