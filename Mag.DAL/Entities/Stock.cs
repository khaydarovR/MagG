using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mag.DAL.Entities
{
    public class Stock
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
    }
}
