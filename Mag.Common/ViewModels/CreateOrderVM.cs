using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Mag.Common.ViewModels
{
    public class CreateOrderVM
    {
        public long SupplyId { get; set; }
        [Required]
        [Display(Name = "Категория")]
        public string NType { get; set; }

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


        [Required]
        [Display(Name = "Адрес")]
        public string Adres { get; set; }

        [Required]
        [Display(Name = "Количество")]
        public int Capacity { get; set; }

    }
}
