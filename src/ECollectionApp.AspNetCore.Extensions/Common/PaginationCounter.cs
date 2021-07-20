using System;

namespace ECollectionApp.AspNetCore.Microservice
{
    public class PaginationCounter
    {
        public int TotalItems { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public int StartPage { get; }
        public int EndPage { get; }
        public int StartIndex { get; }
        public int EndIndex { get; }

        public PaginationCounter(int totalItems, int currentPage, int pageSize) : this(totalItems, currentPage, pageSize, 100) { }

        public PaginationCounter(int totalItems, int currentPage) : this(totalItems, currentPage, 10, 100) { }

        public PaginationCounter(int totalItems) : this(totalItems, 1, 10, 100) { }

        public PaginationCounter(int totalItems, int currentPage, int pageSize, int maxPages)
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize);

            // clamp currentPage from 1 to totalPages
            currentPage = currentPage < 1 ? 1
                                          : currentPage > totalPages ? totalPages
                                                                     : currentPage;

            int startPage;
            int endPage;
            if (totalPages <= maxPages)
            {
                // total pages less than max so show all pages
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                // total pages more than max so calculate start and end pages
                int maxPagesBeforeCurrentPage = (int)Math.Floor((double)maxPages / (double)2);
                int maxPagesAfterCurrentPage = (int)Math.Ceiling((double)maxPages / (double)2) - 1;
                if (currentPage <= maxPagesBeforeCurrentPage)
                {
                    // current page near the start
                    startPage = 1;
                    endPage = maxPages;
                }
                else if (currentPage + maxPagesAfterCurrentPage >= totalPages)
                {
                    // current page near the end
                    startPage = totalPages - maxPages + 1;
                    endPage = totalPages;
                }
                else
                {
                    // current page somewhere in the middle
                    startPage = currentPage - maxPagesBeforeCurrentPage;
                    endPage = currentPage + maxPagesAfterCurrentPage;
                }
            }
            int startIndex = (currentPage - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize - 1, totalItems - 1);

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }
}
