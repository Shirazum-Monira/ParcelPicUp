using System.ComponentModel.DataAnnotations;

namespace ParcelPicUp.Models
{
    public class Common
    {
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Creation Time")]
        public DateTime CTime { get; set; }
        [Display(Name = "Creator")]
        public string? CreatedBy { get; set; }
        [Display(Name = "Modification Time")]
        public DateTime MTime { get; set; }
        [Display(Name = "Modifier")]
        public string? ModifiedBy { get; set; }
    }
}
