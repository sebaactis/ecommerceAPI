namespace Capa.Aplicacion.DTO
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public ICollection<CartItemDTO> CartItems { get; set; }

    }
}
