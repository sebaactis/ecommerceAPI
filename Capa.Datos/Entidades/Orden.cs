namespace Capa.Datos.Entidades
{
    public class Orden
    {
        public int OrdenId { get; set; }
        public DateTime OrdenDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal Total { get; set; }
        public ICollection<OrdenItem> OrdenItems { get; set; }
    }
}
