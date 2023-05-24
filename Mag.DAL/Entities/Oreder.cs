using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mag.DAL.Entities
{
    public class Order
    {

        public int Id { get; set; }

        public Nom Nom { get; set; }

        public AppUser AppUser { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
