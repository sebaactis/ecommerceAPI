namespace Capa.Aplicacion.DTO
{
    public class OrdenItemDTO
    {
        public Guid ProductoId { get; set; }
        public ProductoDTO Producto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
    }
}
