using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealerAPI.Model
{
    public class CarDataModel
    {
        public int CarID { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public int CarYear { get; set; }
        public int CarStock { get; set; }
        public int DealerID { get; set; }
        public int IsDeleted { get; set; }
        public int InsertBy { get; set; }
        public string InsertTime { get; set; }
        public int? UpdateBy { get; set; }
        public string UpdateTime { get; set; }
    }
    public class CreateCarDataModel
    {
        [Required(ErrorMessage = "Car Make is required.")]
        public string CarMake { get; set; }
        [Required(ErrorMessage = "Car Model is required.")]
        public string CarModel { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Car Year must be greater than 0.")]
        public int CarYear { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Car Stock must be greater than 0.")]
        public int CarStock { get; set; }
    }
}
