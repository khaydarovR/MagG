using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mag.DAL.Entities
{
    public class NomType
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название категории")]
        public string Title { get; set; }


        public override string ToString()
        {
            return Title;
        }
    }
}
