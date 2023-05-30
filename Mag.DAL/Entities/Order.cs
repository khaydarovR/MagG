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
        [Required]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Товар")]
        public Supply Supply { get; set; }


        [Required]
        [Display(Name = "Заказал")]
        public AppUser AppUser { get; set; }

        [Required]
        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string Adres { get; set; }

        [Required]
        [Display(Name = "Дата заказа")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
