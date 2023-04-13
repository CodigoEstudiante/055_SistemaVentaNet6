namespace ProyectoVenta.Models
{
    public class Venta
    {

        public int IdVenta { get; set; }
        public string TipoPago { get; set; }
        public string NumeroDocumento { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public decimal MontoPagoCon { get; set; }
        public decimal MontoCambio { get; set; }
        public decimal MontoSubTotal { get; set; }

        public decimal MontoIGV { get; set; }
        public decimal MontoTotal { get; set; }

        public string FechaRegistro { get; set; }

        public List<Detalle_Venta> oDetalleVenta { get; set; }
    }
}
