#nullable disable
using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Product : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; } // zorunlu özellik

        public double UnitPrice { get; set; } // zorunlu özellik

        public int StockAmount { get; set; } // zorunlu özellik

        public DateTime? ExpirationDate { get; set; } 
        // zorunlu olmayan (tabloya null kaydedilebilen) özellik

        public int CategoryId { get; set; } // zorunlu özellik

        public Category Category { get; set; } // başka entity'e referans
    }
}
