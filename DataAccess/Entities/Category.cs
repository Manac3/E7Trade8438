#nullable disable
using AppCore.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Entities
{
    public class Category : RecordBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } // zorunlu (tabloya null kaydedilemeyen) özellik

        public string Description { get; set; } // zorunlu olmayan (tabloya null kaydedilebilen) özellik

        public List<Product> Products { get; set; } // başka entity kolleksiyonuna referans

    }
}
