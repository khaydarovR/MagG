using System.ComponentModel.DataAnnotations;

namespace Mag.DAL.Entities
{
    public class Supply
    {
        public long Id { get; set; }
        [Required]
        [Display(Name = "Склад")]
        public Stock Stock { get; set; }

        [Required]
        [Display(Name = "Товар")]
        public Nom Nom { get; set; }

        [Required]
        [Display(Name = "Дата поставки")]
        public DateTime DateTime { get; set; }

        [Required]
        [Display(Name = "Количество")]
        public int Capacity { get; set; }

        [Required]
        [Display(Name = "Принял")]
        public AppUser User { get; set; }
    }
}
