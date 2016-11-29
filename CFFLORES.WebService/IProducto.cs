﻿using CFFLORES.WebService.Dominio;
using CFFLORES.WebService.Errores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CFFLORES.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProducto" in both code and config file together.
    [ServiceContract]
    public interface IProducto
    {
        [OperationContract]
        [FaultContract(typeof(ProductoInexistente))]
        List<EProducto> ObtenerProducto(string codigobarra, string nombre, string tipo);
        
        
        // Listado de Productos
        [OperationContract]
        List<EProducto> ListarProducto();

    }
}
