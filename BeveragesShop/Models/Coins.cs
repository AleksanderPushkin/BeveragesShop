using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeveragesShop.Models
{
    public partial class Coins
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите название, которое будет использоваться для отображения")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Введите стоимость монет")]
        [Range(0, int.MaxValue, ErrorMessage = "Стоимость монет должено быть положительным")]
        public int Cost { get; set; }
        [Required(ErrorMessage = "Введите текущее количество  монет")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество монет должено быть положительным")]
        public int Count { get; set; }
        [Required(ErrorMessage = "Введите максимальное количество монет данной цеености в автомате")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество монет должено быть положительным")]
        public int MaxQty { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public int CurrentCount { get; set; }
    }
}
