using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.ReviewsFilterSortPagination
{
    public class SortViewModel
    {
        public List<SelectListItem> SortBySelect { get; set; }

        public int? SelectedSort { get; set; }

        public SortState CurrentSort { get; set; }

        public SortViewModel(SortState sortOrder)
        {
            CurrentSort = sortOrder;
        }
    }
}
