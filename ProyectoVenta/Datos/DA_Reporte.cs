using ProyectoVenta.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace ProyectoVenta.Datos
{
    public class DA_Reporte
    {
        public List<Reporte> Listar(string fechaInicio, string fechaFin)
        {

            var oLista = new List<Reporte>();

            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_reporte_venta", conexion);
                cmd.Parameters.AddWithValue("fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("fechaFin", fechaFin);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oLista.Add(new Reporte()
                        {
                            TipoPago = dr["TipoPago"].ToString(),
                            NumeroDocumento = dr["NumeroDocumento"].ToString(),
                            MontoTotal = Convert.ToDecimal(dr["MontoTotal"], new CultureInfo("es-PE")),
                            FechaRegistro = dr["FechaRegistro"].ToString(),
                            DesProducto = dr["DesProducto"].ToString(),
                            Cantidad = Convert.ToInt32(dr["Cantidad"]),
                            PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"], new CultureInfo("es-PE")),
                            Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-PE")),
                        });
                    }
                }
            }

            return oLista;
        }


    }
}
