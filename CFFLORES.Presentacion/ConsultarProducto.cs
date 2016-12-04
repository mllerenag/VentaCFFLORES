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
        public string codigobarras;
        public string desProducto;
        public int stock;
        public double precio;
        public string estado;
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
                   "Advertencia",
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
            idProducto = Convert.ToInt32(dgvProducto[0, e.RowIndex].Value.ToString());
            codigobarras = dgvProducto[1, e.RowIndex].Value.ToString();
            desProducto = dgvProducto[2, e.RowIndex].Value.ToString();
            stock = Convert.ToInt32(dgvProducto[3, e.RowIndex].Value.ToString());
            precio = Convert.ToDouble(dgvProducto[4, e.RowIndex].Value.ToString());
            estado = dgvProducto[5, e.RowIndex].Value.ToString();
        }
    }
}
