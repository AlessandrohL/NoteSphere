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
    public static class NotesQuery
    {
        public static IQueryable<Note> Generate(IQueryable<Note> query, NotesFilter request)
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

        public static Expression<Func<Note, object>> GetSortProperty(NotesFilter request)
        {
            return request.SortColumn?.ToLower() switch
            {
                "title" => note => note.Title!,
                "created" => note => note.CreatedAt,
                "modified" => note => note.ModifiedAt,
                _ => note => note.Title!
            };
        }
    }
}
