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
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Адрес")]
        public string? Address { get; set; }
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
