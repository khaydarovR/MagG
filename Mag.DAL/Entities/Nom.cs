using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mag.DAL.Entities
{
    public class Nom
    {
        public int Id { get; set; }

        public NomType NType { get; set; }

        public Stock Stock { get; set; }

        [Required]
        public string Title { get; set; }

        public string PhotoName { get; set; }

        public int ShelfLife { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
