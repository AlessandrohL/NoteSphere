using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Pagination
{
    public abstract class PaginationParameters
    {
        const int maxPageSize = 20;
        private int _minPage = 1;
        public int Page {
            get
            {
                return _minPage;
            } 
            set
            {
                _minPage = (value < _minPage)
                    ? _minPage
                    : value;
            }
        }

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
