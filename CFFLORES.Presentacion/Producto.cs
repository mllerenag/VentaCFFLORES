﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFFLORES.Presentacion
{
    public class Producto
    {
       public int IdProducto { get; set; }

        public string codigobarra { get; set; }

        public string Nombre { get; set; }

        public Int32 Stock { get; set; }
        public Int32 Cantidad { get; set; }

        public Double Precio { get; set; }
        public Double Total { get; set; }
        /*
              public string Estado { get; set; }

              public string Tipo { get; set; }*/
    }
}
