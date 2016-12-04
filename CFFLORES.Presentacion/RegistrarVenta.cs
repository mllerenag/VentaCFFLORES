//F
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace CFFLORES.Presentacion
{
    public partial class RegistrarVenta : Form
    {
        public RegistrarVenta()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ConsultarProducto frmProducto = new ConsultarProducto();
            frmProducto.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.tabControl1.TabPages.Add(this.tabPage2);

           tabControl1.SelectedIndex = 1;
            cboTipoDocumento.SelectedIndex = 0;
            cboFormaPago.SelectedIndex = 0;
        }


        private void button5_Click(object sender, EventArgs e)
        {

            insertar();
            Listar("1", "1", dateTimePicker1.Value.ToString("yyyyMMdd"));


            tabControl1.SelectedIndex = 0;
            this.tabControl1.TabPages.Remove(this.tabPage2);
        }

        private void insertar()
        {
            string strdni = txtCliente.Text;
            string strtipdoc = cboTipoDocumento.Text;
            string strnrodoc = txtNroVenta.Text;
            string strserie = txtSerie.Text;
            double doumonto = Convert.ToDouble("150.50");
            string strcliente = "Manuel";
            string strformapago = cboFormaPago.Text;

            string postdata = "{\"Dni\":\"" + strdni +
                "\",\"TipoDoc\":\"" + strtipdoc +
                "\",\"NroDoc\":\"" + strnrodoc +
                "\",\"Serie\":\"" + strserie +
                "\",\"Monto\":\"" + doumonto + 
                "\",\"Estado\":\"" + 0 +
                "\",\"Cliente\":\"" + strcliente +
                "\",\"FormaPago\":\"" + strformapago + "\"}";

            try
            {

                byte[] data = Encoding.UTF8.GetBytes(postdata);
                string URLAuth = "http://localhost:24832/Venta.svc/Ventas";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URLAuth);
                req.Method = "POST";
                req.ContentLength = data.Length;
                req.ContentType = "application/json";
                var reqStream = req.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                var res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());
                string clienteJson = reader.ReadToEnd();
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                int idventa;
                idventa = JsonConvert.Deserialize<int>(clienteJson);
                //dgvVenta.AutoGenerateColumns = false;
                //dgvVenta.DataSource = registros;


                    MessageBox.Show("Id Venta " + idventa.ToString(),
                     "Exito",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information,
                     MessageBoxDefaultButton.Button1);





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

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está Seguro que desea cancelar la Venta?",
            "Atencion",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                this.tabControl1.TabPages.Remove(this.tabPage2);
                tabControl1.SelectedIndex = 0;
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {

            int count = 0;
            for (int i = 0; i <= dgvVenta.RowCount - 1; i++)
            {
                if (Convert.ToBoolean(dgvVenta.Rows[i].Cells["Column1"].Value) == true)
                {
                    ++count;

                }
            }


            if (count == 0)
            {
                MessageBox.Show("Debe seleccionar por lo menos una Venta",
                "Adventencia",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);
                return;
            }
            

            DialogResult result = MessageBox.Show("¿Está Seguro que desea Anular la Venta?",
            "Atención",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                for (int i = 0; i <= dgvVenta.RowCount - 1; i++)
                {
                    if (Convert.ToBoolean(dgvVenta.Rows[i].Cells["Column1"].Value) == true)
                    {
                        
                        try
                        {
                            string id = dgvVenta.Rows[i].Cells["IdVenta"].Value.ToString();
                            Modificar(id, "0");

                        }catch(WebException ex)
                        {
                            string idventa = dgvVenta.Rows[i].Cells["IdVenta"].Value.ToString();
                            string rutacola = @".\private$\cfflores";
                            if (!MessageQueue.Exists(rutacola))
                            {
                                MessageQueue.Create(rutacola);
                            }
                            MessageQueue cola = new MessageQueue(rutacola);
                            System.Messaging.Message mensaje = new System.Messaging.Message();
                            mensaje.Label = "Nueva Nota";
                            mensaje.Body = new Venta { IdVenta = Convert.ToInt32(idventa), Estado = 0 };

                            cola.Send(mensaje);
                            string id = mensaje.Id;
                        }
                    }
                }
                Buscar();
            }




        }

        private void Modificar(string idcliente, string estado)
        {
            string id = idcliente;
            string st = estado;
            string postdata = "{\"IdVenta\":\"" + id + "\",\"Estado\":\"" + st + "\"}";

            try
            {

                byte[] data = Encoding.UTF8.GetBytes(postdata);
                string URLAuth = "http://localhost:24832/Venta.svc/Ventas";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URLAuth);
                req.Method = "PUT";
                req.ContentLength = data.Length;
                req.ContentType = "application/json";
                var reqStream = req.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                var res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());
                string clienteJson = reader.ReadToEnd();
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                List<Venta> registros = new List<Venta>();
                registros = JsonConvert.Deserialize<List<Venta>>(clienteJson);
                //dgvVenta.AutoGenerateColumns = false;
                //dgvVenta.DataSource = registros;

                foreach (var value in registros)
                {
                    MessageBox.Show("Se anulo la Venta N°: " + value.NroDoc.ToString(),
                     "Exito",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information,
                     MessageBoxDefaultButton.Button1);
                }




            }
            catch (WebException ex)
            {
                try
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
                catch(Exception e)
                {
                    string idventa = idcliente;
                    string rutacola = @".\private$\cfflores";
                    if (!MessageQueue.Exists(rutacola))
                    {
                        MessageQueue.Create(rutacola);
                    }
                    MessageQueue cola = new MessageQueue(rutacola);
                    System.Messaging.Message mensaje = new System.Messaging.Message();
                    mensaje.Label = "Nueva Nota";
                    mensaje.Body = new Venta { IdVenta = Convert.ToInt32(idventa), Estado = 2 };

                    cola.Send(mensaje);
               
                }

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ConsultarCliente frmCliente = new ConsultarCliente();
            frmCliente.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ConsultarCliente frmCliente = new ConsultarCliente();
            frmCliente.ShowDialog();
        }

        private void RegistrarVenta_Load(object sender, EventArgs e)
        {
            if (chkFecha.Checked == true)
                dateTimePicker1.Enabled = true;
            else
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker1.Value = DateTime.Now;
            }

            if (this.tabControl1.TabPages.Contains(this.tabPage2))
                this.tabControl1.TabPages.Remove(this.tabPage2);
            else
            {
                this.tabControl1.TabPages.Add(this.tabPage2);
                cboTipoDocumento.SelectedIndex = 0;
                cboFormaPago.SelectedIndex = 0;
            }

            dateTimePicker1.CustomFormat = "dd-MM-yyyy";

            rbDni.Checked = true;
            rbVenta.Checked = false;
            AnularVentas();
            Listar("1","1", dateTimePicker1.Value.ToString("yyyyMMdd"));

        }

        private void AnularVentas()
        {
            string rutacola = @".\private$\cfflores";
            if (!MessageQueue.Exists(rutacola))
            {
                MessageQueue.Create(rutacola);
            }
            MessageQueue cola = new MessageQueue(rutacola);
            cola.Formatter = new XmlMessageFormatter(new Type[] { typeof(Venta) });

            System.Messaging.Message mensaje = cola.Receive();

            if (mensaje != null)
            {
                Venta nota = (Venta)mensaje.Body;
                Modificar(nota.IdVenta.ToString(), nota.Estado.ToString());

            }

        }

        public void Listar( string busqueda, string valor, string fecha)
        {
            try
            {
                dgvVenta.AutoGenerateColumns = false;
                //dgvVenta.DataSource = daoproducto.ListarProducto();
                string URLAuth = "http://localhost:24832/Venta.svc/Ventas?Gbusqueda=" + busqueda.ToString() + "&Gvalor=" + valor.ToString() + "&Gfecha=" + fecha.ToString();

                HttpWebRequest req = (HttpWebRequest)WebRequest.
                    Create(URLAuth);
                req.Method = "GET";
                req.ContentType = "application/json";
                var res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());
                string clienteJson = reader.ReadToEnd();
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                List<Venta> registros = new List<Venta>();
                registros = JsonConvert.Deserialize<List<Venta>>(clienteJson);
                dgvVenta.DataSource = registros;


            }
            catch (WebException ex)
            {
                try
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
                catch (Exception e)
                {
                    MessageBox.Show("En estos momentos tenemos problemas de comunicación");
                }

            }
        }
        private void tabControl1_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.TabPages.Contains(this.tabPage2))
                this.tabControl1.TabPages.Remove(this.tabPage2);
            else
            {
                this.tabControl1.TabPages.Add(this.tabPage2);
                cboTipoDocumento.SelectedIndex = 0;
                cboFormaPago.SelectedIndex = 0;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void Buscar()
        {
            if (txtBusCliente.Text.Trim().Length == 0)
            {
                Listar("1", "1", dateTimePicker1.Value.ToString("yyyyMMdd"));
                return;
            }
            if (rbDni.Checked && chkFecha.Checked == true)
                Listar("20", txtBusCliente.Text, dateTimePicker1.Value.ToString("yyyyMMdd"));
            else if (rbDni.Checked && chkFecha.Checked == false)
                Listar("2", txtBusCliente.Text, dateTimePicker1.Value.ToString("yyyyMMdd"));
            else if (rbVenta.Checked && chkFecha.Checked == true)
                Listar("30", txtBusCliente.Text, dateTimePicker1.Value.ToString("yyyyMMdd"));
            else if (rbVenta.Checked && chkFecha.Checked == false)
                Listar("3", txtBusCliente.Text, dateTimePicker1.Value.ToString("yyyyMMdd"));
        }

        private void chkFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFecha.Checked == true)
                dateTimePicker1.Enabled = true;
            else
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker1.Value = DateTime.Now;
            }
        }

        private void cboTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoDocumento.Text.Equals("BOLETA"))
                txtSerie.Text = "001";
            if (cboTipoDocumento.Text.Equals("FACTURA"))
                txtSerie.Text = "003";
        }
    }
}
