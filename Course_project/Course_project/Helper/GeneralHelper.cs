using Course_project.Models;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Helper
{
    public class GeneralHelper
    {
        internal const int REVIEWS_PAGE_SIZE = 5;

        internal const int INDEX_PAGE_SIZE = 5;

        internal FilterViewModel GetFilterViewModel(
            List<string> reviewGroups,
            int selectedGroupId,
            string title, 
            string author)
        {
            reviewGroups.Insert(0, "All");
            var items = new List<SelectListItem>();
            for (int i = 0; i < reviewGroups.Count; i++)
            {
                items.Add(new SelectListItem(reviewGroups[i], i.ToString()));
            }
            items[selectedGroupId].Selected = true;

            return new FilterViewModel()
            {
                ReviewGroupsSelect = items,
                TitleFilter = title,
                AuthorFilter = author,
                SelectedGroup = selectedGroupId
            };
        }

        internal virtual SortViewModel GetSortViewModel(SortState sortOrder)
        {
            var viewModel = new SortViewModel(SortState.PublicationDateDesc)
            {
                SortBySelect = new List<SelectListItem>()
                {
                    new SelectListItem("By title asc", SortState.TitleAsc.ToString()),
                    new SelectListItem("By title desc", SortState.TitleDesc.ToString()),
                    new SelectListItem("By author asc", SortState.AuthorAsc.ToString()),
                    new SelectListItem("By author desc", SortState.AuthorDesc.ToString()),
                    new SelectListItem("By rating asc", SortState.RatingAsc.ToString()),
                    new SelectListItem("By rating desc", SortState.RatingDesc.ToString()),
                    new SelectListItem("By publication date asc", SortState.PublicationDateAsc.ToString()),
                    new SelectListItem("By publication date desc", SortState.PublicationDateDesc.ToString())
                },
                CurrentSort = sortOrder
            };
            viewModel.SortBySelect[(int)sortOrder].Selected = true;

            return viewModel;
        }

        internal PageViewModel GetPageViewModel(int count, int page, int pageSize)
        {
            return new PageViewModel(count, page, pageSize);
        }

        internal void FilterReviews(ref IQueryable<Review> reviews, string title, string author, int groupId)
        {
            if (!String.IsNullOrEmpty(title))
            {
                reviews = reviews.Where(p => p.Title.Contains(title));
            }
            if (!String.IsNullOrEmpty(author))
            {
                reviews = reviews.Where(p => p.AuthorUserName.Contains(author));
            }
            if (groupId != 0)
            {
                reviews = reviews.Where(p => p.GroupId == groupId);
            }
        }

        internal void SortReviews(ref IQueryable<Review> reviews, SortState sortOrder)
        {
            switch (sortOrder)
            {
                case SortState.AuthorAsc:
                    reviews = reviews.OrderBy(s => s.AuthorUserName);
                    break;
                case SortState.AuthorDesc:
                    reviews = reviews.OrderByDescending(s => s.AuthorUserName);
                    break;
                case SortState.PublicationDateAsc:
                    reviews = reviews.OrderBy(s => s.PublicationDate);
                    break;
                case SortState.PublicationDateDesc:
                    reviews = reviews.OrderByDescending(s => s.PublicationDate);
                    break;
                case SortState.RatingAsc:
                    reviews = reviews.OrderBy(s => s.AverageRating);
                    break;
                case SortState.RatingDesc:
                    reviews = reviews.OrderByDescending(s => s.AverageRating);
                    break;
                case SortState.TitleAsc:
                    reviews = reviews.OrderBy(s => s.Title);
                    break;
                case SortState.TitleDesc:
                    reviews = reviews.OrderByDescending(s => s.Title);
                    break;
                default:
                    reviews = reviews.OrderByDescending(s => s.PublicationDate);
                    break;
            }
        }

        internal async Task<List<Review>> GetPageReviews(IQueryable<Review> reviews, int page, int pageSize)
        {
            return await reviews.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        internal async Task<int> GetReviewsCount(IQueryable<Review> reviews)
        {
            return await reviews.CountAsync();
        }
    }
}
