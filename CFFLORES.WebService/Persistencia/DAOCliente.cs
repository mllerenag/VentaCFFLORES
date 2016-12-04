using CFFLORES.WebService.Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CFFLORES.WebService.Persistencia
{
    public class DAOCliente
    {
        private string cadenaconexion = "Data Source=a1d03a2d-9c0f-4408-a40e-a6c90072de9e.sqlserver.sequelizer.com;Initial Catalog=dba1d03a2d9c0f4408a40ea6c90072de9e;User Id=tvmfwzevhnftvnla;Password=2Ccx6Hj4f3x55DK6Quii6SixnKvTrFciBYEozCMmQGhaFo3U5qeDwtdiuMkumxKF;";
        //private string cadenaconexion = "Data Source=LAPTOP-C3204AHJ\\SQLEXPRESS;Initial Catalog=CFFLORESDB;Integrated Security=True";

        public List<ECliente> Listar(string busqueda, string Valor)
        {
            List<ECliente> lista = new List<ECliente>();
            string sql = "";
            if (busqueda.Equals("1")) //Listar
                sql = "SELECT * FROM Cliente5  where dni = " + Valor;
            else if (busqueda.Equals("2")) //Por Dni
                sql = "SELECT * FROM Cliente5 where nombre = " + Valor;
               
            try
            {
                using (SqlConnection con = new SqlConnection(cadenaconexion))
                {
                    con.Open();
                    using (SqlCommand com = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ECliente ve = new ECliente();

                                ve.IdCliente = Convert.ToInt32(dr[0]);
                                ve.Dni = dr[1].ToString();
                                ve.Nombre = dr[2].ToString();
                                ve.Direccion = dr[3].ToString();
                                lista.Add(ve);
                            }
                            dr.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;
        }

        //public List<ECliente> Modificar(ECliente cliente)
        //{
        //    /*
        //     Estados:
        //     0: Venta
        //     1: Contabilizado
        //     2: Anulado
        //     */
        //    List<ECliente> resultado = new List<ECliente>();

        //    string sql = "";
        //    if (cliente.Estado == 0)
        //        sql = "update venta set Estado=2 where Idcliente=@idcliente ";
        //    else
        //        sql = "update venta set Estado=Estado where Idcliente=@idcliente ";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(cadenaconexion))
        //        {
        //            con.Open();
        //            using (SqlCommand com = new SqlCommand(sql, con))
        //            {
        //                com.Parameters.Add(new SqlParameter("@idcliente", cliente.IdCliente));
        //                com.ExecuteNonQuery();
        //            }
        //        }
        //        resultado = Listar("4", cliente.IdCliente.ToString(), cliente.Fecha.ToString("yyyyMMdd"));
        //        return resultado;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //public int Insertar(EVenta venta)
        //{



        //    string sql = "insert into Venta ([Dni],[Fecha],[TipoDoc],[NroDoc],[Serie],[Monto],[Estado],[Cliente],[FormaPago]) output INSERTED.ID values(@dni,GETDATE(),@tipodoc,@nrodoc,@serie,@monto,@estado,@cliente,@formapago,@idproducto)";
        //    int idventa;

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(cadenaconexion))
        //        {
        //            con.Open();
        //            using (SqlCommand com = new SqlCommand(sql, con))
        //            {
        //                com.Parameters.Add(new SqlParameter("@dni", venta.Dni));
        //                com.Parameters.Add(new SqlParameter("@fecha", venta.Fecha));
        //                com.Parameters.Add(new SqlParameter("@tipodoc", venta.TipoDoc));
        //                com.Parameters.Add(new SqlParameter("@nrodoc", venta.NroDoc));
        //                com.Parameters.Add(new SqlParameter("@serie", venta.Serie));
        //                com.Parameters.Add(new SqlParameter("@monto", venta.Monto));
        //                com.Parameters.Add(new SqlParameter("@estado", venta.Estado));
        //                com.Parameters.Add(new SqlParameter("@cliente", venta.Cliente));
        //                com.Parameters.Add(new SqlParameter("@formago", venta.FormaPago));
        //                com.ExecuteNonQuery();
        //                idventa = (int)com.ExecuteScalar();
        //            }

        //        }
        //        return idventa;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}


    }
}