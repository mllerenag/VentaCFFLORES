using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;



namespace CFFLORES.Presentacion
{
    public partial class ConsultarCliente : Form
    {
        public ConsultarCliente()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.Close();
        }


        public void Listar(string busqueda, string valor)
        {
            try
            {
                dgvCliente.AutoGenerateColumns = false;
                //dgvVenta.DataSource = daoproducto.ListarProducto();
                string URLAuth = "http://localhost:24832/Cliente.svc/Clientes?Gbusqueda=" + busqueda.ToString() + "&Gvalor=" + valor.ToString();

                HttpWebRequest req = (HttpWebRequest)WebRequest.
                    Create(URLAuth);
                req.Method = "GET";
                req.ContentType = "application/json";
                var res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());
                string clienteJson = reader.ReadToEnd();
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                List<Cliente> registros = new List<Cliente>();
                registros = JsonConvert.Deserialize<List<Cliente>>(clienteJson);
                dgvclientebuscar.DataSource = registros;


            }
            catch (WebException ex)
            {
                HttpStatusCode code = ((HttpWebResponse)ex.Response).StatusCode;
                string message = ((HttpWebResponse)ex.Response).StatusDescription;
                StreamReader reader = new StreamReader(ex.Response.GetResponseStream());
                string error = reader.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string mensaje = js.Deserialize<string>(error);

                MessageBox.Show(mensaje,
                "Advertencia",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1);

            }
        }







        private void button1_Click(object sender, EventArgs e)
        {


                Buscar();
                
                    
        }

        //Listar("1", txtBusCliente1.Text);

        private void Buscar()
        {
            if (txtBusCliente1.Text.Trim().Length!=0)
            Listar("1", txtBusCliente1.Text);

            if (txtBusCliente2.Text.Trim().Length != 0)
                Listar("2", txtBusCliente2.Text);

        }





       
        private void rbDni1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtBusCliente1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

     

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ConsultarCliente_Load(object sender, EventArgs e)
        {
            


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }



    }
}
