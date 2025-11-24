using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.DA.Pagination
{
    public static class IQueryablePaginateExtension
    {
        public static async Task<PagedResult<T>> ToPaginateAsync<T>(this IQueryable<T> source, int index, int size)
        {

            int count = await source.CountAsync();
            List<T> items = await source.Skip(index * size).Take(size).ToListAsync();

            PagedResult<T> list = new()
            {
                Index = index,
                Size = size,
                Count = count,
                Items = items,
                Pages = (int)Math.Ceiling(count / (double)size)
            };

            return list;
        }
    }
}
