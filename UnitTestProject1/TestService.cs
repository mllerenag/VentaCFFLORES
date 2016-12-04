using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class TestService
    {
        [TestMethod]
        public void TestObtenerProductoFault()
        {
            ProductoWSC.ProductoClient proxy = new ProductoWSC.ProductoClient();
            string codigobarra = "121212121212";
            string nombre = "";
            string tipo = "";
            try
            {

                ProductoWSC.EProducto[] ObProducto = proxy.ObtenerProducto(codigobarra, nombre, tipo);
                
                     Assert.AreEqual(codigobarra, ObProducto[0].codigobarra.ToString());

            }
            catch (FaultException<ProductoWSC.ProductoInexistente> error)
            {
                /*Por error se valida los datos*/
                if (error.Detail.exCodigo == 1)
                    Assert.AreEqual("Para Buscar un producto por Nombre o Tipo, no se debe registrar código de barras", error.Reason.ToString());
                if (error.Detail.exCodigo == 10) 
                    Assert.AreEqual("El producto buscado No existe", error.Reason.ToString());
                if (error.Detail.exCodigo == 11)
                    Assert.AreEqual("El producto " + error.Detail.exProducto + " no cuenta con Stock disponible en este momento", error.Reason.ToString());
                if (error.Detail.exCodigo == 12)
                    Assert.AreEqual("El stock del producto " + error.Detail.exProducto + " está por agotarse", error.Reason.ToString());
                if (error.Detail.exCodigo == 13)
                    Assert.AreEqual("El producto " + error.Detail.exProducto + " se encuentra deshabilitado", error.Reason.ToString());
            }

        }
    }
}
