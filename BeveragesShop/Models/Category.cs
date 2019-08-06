using System.ComponentModel.DataAnnotations;

namespace BeveragesShop.Models
{
    public enum Category
    {
        [Display(Name="Газировка")] 
        Softdrink,
        [Display(Name = "Сок")]
        Juice,
        [Display(Name = "Минеральная вода")]
        MiniralWater
    }
}
