namespace EmailSpamDetectionService.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
