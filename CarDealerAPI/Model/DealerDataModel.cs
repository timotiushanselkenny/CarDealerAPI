using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealerAPI.Model
{
    public class DealerDataModel
    {
        public int DealerID { get; set; } 

        public string DealerName { get; set; }

        public string DealerUsername { get; set; }

        public string DealerSecretKey { get; set; }

        public bool IsDeleted { get; set; } 

        public string InsertBy { get; set; } 

        public DateTime InsertTime { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
    public class CreateDealerDataModel
    {
        [Required(ErrorMessage = "Dealer Name is required.")]
        public string DealerName { get; set; }

        [Required(ErrorMessage = "Dealer Username is required.")]
        public string DealerUsername { get; set; }

        [Required(ErrorMessage = "Dealer Secret Key is required.")]
        public string DealerSecretKey { get; set; }
    }
}
