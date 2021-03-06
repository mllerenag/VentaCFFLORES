﻿using CFFLORES.WebService.Dominio;
using CFFLORES.WebService.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CFFLORES.WebService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Venta" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Venta.svc o Venta.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Venta : IVenta
    {
        private DAOVenta dao = new DAOVenta();
        public List<EVenta> Listar(string busqueda, string Valor, string fecha)
        {
            /*
             busqueda:
             1: Listar
             2: Por Dni
             3: Por Serie
             4: Por Id
             */
            if (String.IsNullOrEmpty(busqueda)) busqueda = "1";
            if (String.IsNullOrEmpty(Valor)) Valor = "1";

            if (busqueda.Equals("2") &&  Valor.Length != 8)//Busqueda por DNI
            {
                throw new WebFaultException<string>("La Búsqueda por DNI debe contener 8 Caracteres", HttpStatusCode.InternalServerError);
            }

            if (busqueda.Equals("3") && Valor.Length != 5)//Busqueda por NroVenta
            {
                throw new WebFaultException<string>("La Búsqueda por Serie debe contener 5 Caracteres", HttpStatusCode.InternalServerError);
            }

            List<EVenta> obobVenta = new List<EVenta>();
            obobVenta = dao.Listar(busqueda, Valor,fecha);

            if (obobVenta.Count == 0)
            {
                throw new WebFaultException<string>("No Existe la Venta según los parámetros ingresados", HttpStatusCode.InternalServerError);

            }



            return obobVenta;


        }
        
        public List<EVenta> Modificar(EVenta beventa)
        {

            /*
             Estados:
             0: Venta
             1: Contabilizado
             2: Anulado
             */
             if (beventa == null)
            {
                beventa = new EVenta { IdVenta = 0,Estado=0 };
            }

            List<EVenta> obobVenta = new List<EVenta>();

            obobVenta = dao.Listar("4", beventa.IdVenta.ToString(), beventa.Fecha.ToString("yyyyMMdd"));

            if (obobVenta.Count() == 0)
            {
                throw new WebFaultException<string>("No Existe la Venta según los parámetros ingresados", HttpStatusCode.InternalServerError);

            }

            string estado = obobVenta[0].Estado.ToString();
            if (estado.Equals("1"))
            {
                throw new WebFaultException<string>("No se puede Anular una Venta con estado Contabilizado", HttpStatusCode.InternalServerError);

            }
            if (estado.Equals("2"))
            {
                throw new WebFaultException<string>("No se puede anular una venta ya anulada", HttpStatusCode.InternalServerError);

            }

            List<EVenta> obobVentaresult = new List<EVenta>();
            obobVentaresult = dao.Modificar(beventa);

            
            return obobVentaresult;
        }

        public int Insertar(EVenta beventa)
        {

            try
            {
                if (beventa == null)
                    return 0;

                if (String.IsNullOrEmpty(beventa.Dni))
                {
                    throw new WebFaultException<string>("Debe ingresar el Cliente", HttpStatusCode.InternalServerError);

                }

                if (beventa.Monto == 0)
                {
                    throw new WebFaultException<string>("El monto debe ser mayor a Cero", HttpStatusCode.InternalServerError);

                }


                int idventa;
                idventa = dao.Insertar(beventa);
                return idventa;
            }
            catch(WebException ex)
            {
                throw new WebFaultException<string>(ex.ToString(), HttpStatusCode.InternalServerError);

            }
          

        }


    }
}
