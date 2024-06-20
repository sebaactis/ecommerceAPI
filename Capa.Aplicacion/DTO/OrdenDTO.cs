namespace Capa.Aplicacion.DTO
{
    public class OrdenDTO
    {
        public Guid OrdenId { get; set; }
        public decimal Total { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public ICollection<OrdenItemDTO> OrdenItems { get; set; }
    }
}
