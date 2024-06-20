namespace Capa.Aplicacion.DTO
{
    public class CartDTO
    {
        public Guid CartId { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public ICollection<CartItemDTO> CartItems { get; set; }

    }
}
