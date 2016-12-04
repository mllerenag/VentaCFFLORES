using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CFFLORES.WebService.Dominio
{
    [DataContract]
    public class ECliente
    {
        [DataMember]
        public int IdCliente { get; set; }
        [DataMember]
        public string Dni { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Direccion { get; set; }



        
    }
}