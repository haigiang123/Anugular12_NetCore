using System;
using System.Collections.Generic;
using System.Text;

namespace WebViewModel.Common
{
    public class PageResultBase
    {
        public int Size { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int PageCount
        {
            get
            {
                if (Size == 0)
                {
                    return Size;
                }
                return (int)Math.Ceiling((double)(Total / Size));
            }
        }
    }
}
