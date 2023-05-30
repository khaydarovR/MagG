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
        [Required]
        [Display(Name = "Категория")]
        public NomType NType { get; set; }

        [Required]
        [Display(Name = "Название товара")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Изображение")]
        public string PhotoName { get; set; }
        [Display(Name = "Срок годности")]
        public int ShelfLife { get; set; }

        [Required]
        [Display(Name = "Цена")]
        public int Price { get; set; }
    }
}
