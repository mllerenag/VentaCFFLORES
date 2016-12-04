﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CFFLORES.WebService.Errores;
using CFFLORES.WebService.Persistencia;
using CFFLORES.WebService.Dominio;

namespace CFFLORES.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Producto" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Producto.svc or Producto.svc.cs at the Solution Explorer and start debugging.
    public class Producto : IProducto
    {

        private DAOProducto daoproducto = new DAOProducto();
        public List<EProducto> ObtenerProducto(string codigobarra, string nombre, string tipo)
        {
            if (String.IsNullOrEmpty(codigobarra)) codigobarra = "";
            if (String.IsNullOrEmpty(nombre)) nombre = "";
            if (String.IsNullOrEmpty(tipo)) tipo = "";

            /*Se valida que exista Producto*/
            if (codigobarra.Length == 0 && nombre.Length == 0 && tipo.Length == 0)
            {
                throw new FaultException<ProductoInexistente>(
                    new ProductoInexistente()
                    {
                        exCodigo = 14,
                        exError = "Por lo menos debe llenar un campo para buscar."
                    }
                , new FaultReason("Por lo menos debe llenar un campo para buscar."));

            }

            /*Se valida que exista Producto*/
            if (codigobarra.Length != 0 && (nombre.Length !=0 || tipo.Length !=0))
            {
                throw new FaultException<ProductoInexistente>(
                    new ProductoInexistente()
                    {
                        exCodigo = 1,
                        exError = "Para buscar un producto por Nombre o Tipo, no se debe registrar código de Barras"
                    }
                , new FaultReason("Para Buscar un producto por Nombre o Tipo, no se debe registrar código de Barras"));

            }

            List<EProducto> ObProducto = new List<EProducto>();
            ObProducto = daoproducto.ObtenerProducto(codigobarra,nombre,tipo);
            /*Se valida que exista Producto*/
            if (ObProducto.Count() == 0)
            {
                throw new FaultException<ProductoInexistente>(
                    new ProductoInexistente()
                    {
                        exCodigo = 10,
                        exError = "El producto buscado No existe"
                    }
                , new FaultReason("El producto buscado No existe"));

            }

            if (ObProducto.Count() == 1)
            {
                /*Se valida que el producto este habilitado*/
                //0 : Habilitado
                //1: Deshabilitado
                if (String.IsNullOrEmpty(ObProducto[0].Estado) || ObProducto[0].Estado.Equals("1"))
                {
                    throw new FaultException<ProductoInexistente>(
                        new ProductoInexistente()
                        {
                            exCodigo = 13,
                            exProducto = ObProducto[0].Nombre.ToString(),
                            exError = "El producto " + ObProducto[0].Nombre + " se encuentra deshabilitado"
                        }
                    , new FaultReason("El producto " + ObProducto[0].Nombre + " se encuentra deshabilitado"));

                }
                /*Se valida que exista Stock*/
                if (ObProducto[0].Stock <= 0)
                {
                    throw new FaultException<ProductoInexistente>(
                        new ProductoInexistente()
                        {
                            exCodigo = 11,
                            exProducto = ObProducto[0].Nombre.ToString(),
                            exError = "El producto " + ObProducto[0].Nombre + " no cuenta con Stock disponible en este momento"
                        }
                    , new FaultReason("El producto " + ObProducto[0].Nombre + " no cuenta con Stock disponible en este momento"));

                }
                /*Se valida que  se va agotar Stock*/
                if (ObProducto[0].Stock <= 10)
                {
                    throw new FaultException<ProductoInexistente>(
                        new ProductoInexistente()
                        {
                            exCodigo = 12,
                            exProducto = ObProducto[0].Nombre.ToString(),
                            exError = "El stock del producto " + ObProducto[0].Nombre + " está por agotarse"
                        }
                    , new FaultReason("El stock del producto " + ObProducto[0].Nombre + " está por agotarse"));

                }
            }

            return ObProducto;
        }
        /*
        public EProducto CrearProducto(EProducto productos)
        {
            if (daoproducto.ObtenerProducto(productos.codigobarra) != null)
            {
                throw new FaultException<RepetidoException>(
                    new RepetidoException()
                    {
                        excodigobarra = productos.codigobarra,
                        exNombreProducto = "El producto No existe",
                        exStock = 0
                    },
                    new FaultReason("Error al intentar Insertar"));

            }
            return daoproducto.CrearProducto(productos);
        }

        public EProducto ModificarProducto(EProducto productos)
        {
            if (daoproducto.ObtenerProducto(productos.codigobarra) == null)
            {
                throw new FaultException<RepetidoException>(
                    new RepetidoException()
                    {
                        excodigobarra = productos.codigobarra,
                        exNombreProducto = "El producto No existe",
                        exStock = 0
                    },
                    new FaultReason("Error al intentar Modificar"));

            }
            return daoproducto.ModificarProducto(productos);
        }
        */
        public List<EProducto> ListarProducto()
        {
            return daoproducto.ListarProducto();
        }

    }
}
