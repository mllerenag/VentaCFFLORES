﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Text;

namespace CFFLORES.TestRest
{
    [TestClass]
    public class VentaTest
    {

        [TestMethod]
        public void TestListar()
        {

            /*
            busqueda:
                1 - Listar
                2 - Dni
                20 -  Dni x fecha
                3 - NroVenta
                30 -  NroVenta x fecha
                4 - IdVenta
            */

            string busqueda = "2";
            string valor = "11111111";
            string fecha = "20160709";
            try
            {

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

                foreach (var value in registros)
                {
                    Assert.AreEqual(valor, value.Dni);
                }

            }
            catch (WebException ex)
            {
                HttpStatusCode code = ((HttpWebResponse)ex.Response).StatusCode;
                string message = ((HttpWebResponse)ex.Response).StatusDescription;
                StreamReader reader = new StreamReader(ex.Response.GetResponseStream());
                string error = reader.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string mensaje = js.Deserialize<string>(error);
                if (busqueda.Equals("2"))
                    Assert.AreEqual("La Búsqueda por DNI debe contener 8 Caracteres", mensaje);
                else if (busqueda.Equals("3"))
                    Assert.AreEqual("La Búsqueda por Serie debe contener 5 Caracteres", mensaje);
                else
                    Assert.AreEqual("No Existe la Venta según los parámetros ingresados", mensaje);

            }
        }
        [TestMethod]
        public void TestModificar()
        {
            string idcliente = "5";
            string estado = "0";// 0: Para Anular, 1 o otro:Mantiene estado

            string postdata = "{\"IdVenta\":\"" + idcliente + "\",\"Estado\":\"" + estado + "\"}";
            //string postdata = "{\"IdVenta\":\"1\",\"Estado\":\"true\"}";

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

                foreach (var value in registros)
                {
                    Assert.AreEqual(idcliente, value.IdVenta.ToString());
                }

            }
            catch (WebException ex)
            {
                HttpStatusCode code = ((HttpWebResponse)ex.Response).StatusCode;
                string message = ((HttpWebResponse)ex.Response).StatusDescription;
                StreamReader reader = new StreamReader(ex.Response.GetResponseStream());
                string error = reader.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string mensaje = js.Deserialize<string>(error);
                if (estado.Equals("1"))
                    Assert.AreEqual("No se puede Anular una Venta con estado Contabilizado", mensaje);
                else if (estado.Equals("2"))
                    Assert.AreEqual("No se puede anular una venta ya anulada", mensaje);
                else if (estado.Equals("3"))
                    Assert.AreEqual("No Existe la Venta según los parámetros ingresados", mensaje);

            }

        }
        [TestMethod]
        public void TestInsertar()
        {
            string strdni = "99999955";
            string strtipdoc = "FACTURA";
            string strnrodoc = "25259";
            string strserie = "003";
            double doumonto = Convert.ToDouble("150.50");
            string strcliente = "Manuel";
            string strformapago = "EFECTIVO";

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
                int registros;
                registros = JsonConvert.Deserialize<int>(clienteJson);



            }
            catch (WebException ex)
            {
                HttpStatusCode code = ((HttpWebResponse)ex.Response).StatusCode;
                string message = ((HttpWebResponse)ex.Response).StatusDescription;
                StreamReader reader = new StreamReader(ex.Response.GetResponseStream());
                string error = reader.ReadToEnd();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string mensaje = js.Deserialize<string>(error);

                    Assert.AreEqual("No se puede Anular una Venta con estado Contabilizada", mensaje);


            }
        }
    }
}
