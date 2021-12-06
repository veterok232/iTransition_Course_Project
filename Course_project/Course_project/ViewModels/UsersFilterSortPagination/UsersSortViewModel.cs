using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.UsersFilterSortPagination
{
    public class UsersSortViewModel
    {
        public List<SelectListItem> SortBySelect { get; set; }

        public int? SelectedSort { get; set; }

        public UsersSortState CurrentSort { get; set; }

        public UsersSortViewModel(UsersSortState sortOrder)
        {
            CurrentSort = sortOrder;
        }
    }
}
