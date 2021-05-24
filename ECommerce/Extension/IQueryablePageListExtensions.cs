using ECommerce.Models.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Extension
{
    public static class IQueryablePageListExtensions
    {
        public static async Task<Pagination<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize) where T : class
        {
            var totalRecords = await source.CountAsync().ConfigureAwait(false);
            List<T> items;

            if (pageIndex == 0 || pageSize == 0)
            {
                items = await source.ToListAsync().ConfigureAwait(false);
            }
            else
            {
                items = await source.Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize).ToListAsync().ConfigureAwait(false);
            }
            var pageNumber = totalRecords / pageSize;

            var pagination = new Pagination<T>()
            {
                Items = items,
                TotalRecords = totalRecords,
                CurrentPage = pageIndex,
                PageSize = pageSize,
                TotalPages = totalRecords % pageSize != 0 ? (pageNumber + 1) : pageNumber
            };

            return pagination;
        }
    }
}
