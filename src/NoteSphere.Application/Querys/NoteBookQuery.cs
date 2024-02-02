using Domain.Entities;
using Domain.Filters;
using Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Querys
{
    public static class NotebookQuery
    {
        public static IQueryable<Notebook> Generate(
            IQueryable<Notebook> query,
            NotebooksFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(nb =>
                    nb.Title!.Contains(filter.SearchTerm));
            }

            if (filter.SortOrder?.ToLower() == "desc")
            {
                query = query.OrderByDescending(GetSortProperty(filter));
            }
            else
            {
                query = query.OrderBy(GetSortProperty(filter));
            }

            query = query.Paginate(filter.Page, filter.PageSize);

            return query;
        }

        public static Expression<Func<Notebook, object>> GetSortProperty(NotebooksFilter filter)
        {
            return filter.SortColumn?.ToLower() switch
            {
                "title" => notebook => notebook.Title!,
                "created" => notebook => notebook.CreatedAt,
                //"modified" => notebook => notebook.ModifiedTime,
                _ => notebook => notebook.Title!
            };
        }
    }
}
