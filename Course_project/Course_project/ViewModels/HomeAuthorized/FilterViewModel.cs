using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.HomeAuthorized
{
    public class FilterViewModel
    {
        public IEnumerable<SelectListItem> ReviewGroupsSelect { get; set; }

        public int? SelectedGroup { get; set; }

        public string TitleFilter { get; set; }

        public string AuthorFilter { get; set; }
    }
}
