namespace Capa.Aplicacion.DTO
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public ProductoDTO Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
