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
    public static class NoteBookQuery
    {
        public static IQueryable<NoteBook> Generate(IQueryable<NoteBook> query, NoteBooksFilter request)
        {
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(nb =>
                    nb.Title!.Contains(request.SearchTerm));
            }

            if (request.SortOrder?.ToLower() == "desc")
            {
                query = query.OrderByDescending(GetSortProperty(request));
            }
            else
            {
                query = query.OrderBy(GetSortProperty(request));
            }

            query = query.Paginate(request.Page, request.PageSize);

            return query;
        }

        public static Expression<Func<NoteBook, object>> GetSortProperty(NoteBooksFilter request)
        {
            return request.SortColumn?.ToLower() switch
            {
                "title" => notebook => notebook.Title!,
                "created" => notebook => notebook.CreatedAt,
                //"modified" => notebook => notebook.ModifiedTime,
                _ => notebook => notebook.Title!
            };
        }
    }
}
