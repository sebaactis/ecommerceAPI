namespace Capa.Datos.Entidades
{
    public class Cart : ModelBase
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
