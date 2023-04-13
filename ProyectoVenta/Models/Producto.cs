namespace ProyectoVenta.Models
{
    public class Producto
    {
   

        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public Categoria oCategoria { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
    }
}
