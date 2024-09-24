using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XC.OC.Migration.Core.Domain.Model.OrderCloud
{
    public class PagedResults<T> where T : class
    {
        public IList<T> Items { get; set; } = new List<T>();

        public ResultsMeta Meta { get; set; } = new ResultsMeta();

        //[JsonIgnore]
        public static PagedResults<T> Empty
        {
            get
            {
                return new PagedResults<T> { Items = new List<T>() };
            }
        }
    }

    public class ResultsMeta
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public string NextPageKey { get; set; }

        public int[] ItemRange { get; set; }
    }
}
