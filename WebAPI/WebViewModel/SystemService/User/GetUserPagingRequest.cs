using System;
using System.Collections.Generic;
using System.Text;
using WebViewModel.Common;

namespace WebViewModel.SystemService.User
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
