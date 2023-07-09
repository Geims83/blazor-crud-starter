using System.Collections.Generic;

namespace BlazorCRUD.Data.Models
{
    public class Category : EntityBase
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
    }
}
