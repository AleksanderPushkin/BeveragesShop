using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BeveragesShop.Models
{
    public partial class Beverages
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите название напитка")]
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        [Required(ErrorMessage = "Введите цену напитка автомате")]
        [Range(0, int.MaxValue, ErrorMessage = "Цена напитка должена положительным")]
        public int Price { get; set; }
        public int? ProducerId { get; set; }
        public Category Category { get; set; }
        [Required(ErrorMessage = "Введите текущее количество бутылок автомате")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество бутылок должено положительным")]
        public int Qty { get; set; }

        public virtual Producers Producer { get; set; }
    }
}
