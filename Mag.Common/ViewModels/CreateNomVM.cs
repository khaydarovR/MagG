using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Mag.Common.ViewModels
{
    public class CreateNomVM
    {
        [Required]
        [Display(Name = "Категория")]
        public string NType { get; set; }

        [Required]
        [Display(Name = "Название товара")]
        public string Title { get; set; }

        [Display(Name = "Изображение")]
        public IFormFile Photo { get; set; }
        [Display(Name = "Срок годности")]
        public int ShelfLife { get; set; }

        [Required]
        [Display(Name = "Цена")]
        public int Price { get; set; }
    }
}
