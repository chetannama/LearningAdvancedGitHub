using System.ComponentModel.DataAnnotations;

namespace EmailSpamDetectionService.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
