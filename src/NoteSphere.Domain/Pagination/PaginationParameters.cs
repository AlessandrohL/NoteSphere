using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Pagination
{
    public abstract class PaginationParameters
    {
        const int maxPageSize = 40;
        public int Page { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize)
                    ? maxPageSize
                    : value;
            }

        }


    }


}
