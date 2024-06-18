namespace Capa.Datos.Entidades
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }

    }
}
