namespace Capa.Aplicacion.DTO
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public ProductoDTO Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
