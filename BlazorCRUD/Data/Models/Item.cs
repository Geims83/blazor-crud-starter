namespace BlazorCRUD.Data.Models
{
    public class Item : EntityBase
    {
        public int ItemId { get; set; }
        public string? Description { get; set; }
        public decimal? Value { get; set; }
        public Category Category { get; set; }
    }
}
