namespace Capa.Datos.Entidades
{
    public class OrdenItem
    {
        public int OrdenItemId { get; set; }
        public Guid OrdenId { get; set; }
        public Orden Orden { get; set; }
        public Guid ProductoId { get; set; }
        public Producto Producto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
    }
}
