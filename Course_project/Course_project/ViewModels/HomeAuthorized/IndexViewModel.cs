using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.HomeAuthorized
{
    public class IndexViewModel
    {
        [Display(Name = "Nickname")]
        [DataType(DataType.Text)]
        public string Nickname { get; set; }
    }
}
