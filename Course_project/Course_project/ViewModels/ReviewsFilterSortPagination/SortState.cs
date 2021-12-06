using System;

namespace Course_project.ViewModels.ReviewsFilterSortPagination
{
    /// <summary>
    /// Sort states for sorting reviews
    /// </summary>
    public enum SortState
    {
        TitleAsc,
        TitleDesc,
        AuthorAsc,
        AuthorDesc,
        RatingAsc,
        RatingDesc,
        PublicationDateAsc, 
        PublicationDateDesc
    }
}
