namespace CQRS.WebApi.Models.Dtos
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}