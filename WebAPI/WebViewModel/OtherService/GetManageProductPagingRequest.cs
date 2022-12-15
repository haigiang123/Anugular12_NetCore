using System;
using System.Collections.Generic;
using System.Text;
using WebViewModel.Common;

namespace WebViewModel.OtherService
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        public string LanguageId { get; set; }

        public int? CategoryId { get; set; }
    }
}
