﻿using CFFLORES.WebService.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.UI;

namespace CFFLORES.WebService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IVenta" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface ICliente
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Clientes?Gbusqueda={busqueda}&Gvalor={Valor}", ResponseFormat = WebMessageFormat.Json)]
        List<ECliente> Listar(string busqueda, string Valor);
        
        //[OperationContract]
        //[WebInvoke(Method = "PUT", UriTemplate = "Ventas", ResponseFormat = WebMessageFormat.Json)]
        //List<EVenta> Modificar(EVenta beventa);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "Ventas", ResponseFormat = WebMessageFormat.Json)]
        //int Insertar(EVenta beventa);

    }
}
