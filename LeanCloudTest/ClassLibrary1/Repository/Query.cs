using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeanCloudCommond.Repository
{
    public class Query
    {
        private List<QueryParam> paramList = new List<QueryParam>();
        private string orderBy;
        private string groupBy;
        public string QuerySQL<T>()where T:Model.ModelBase
        {
            Type type = typeof(T);
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from "+type.Name+" ");
            if (this.paramList.Count > 0)
            {
                sb.Append(" where ");
                paramList[0].Connect = string.Empty;
            }
            foreach(var param in this.paramList)
            {
                sb.Append(param.ToQueryStr());
            }
            if (!string.IsNullOrEmpty(orderBy))
                sb.Append(orderBy);
            if (!string.IsNullOrEmpty(groupBy))
                sb.Append(groupBy);
            return sb.ToString();
        }
        public Query OrderBy(string column)
        {
            this.orderBy =" order by "+column;
            return this;
        }
        public Query OrderByDESC(string column)
        {
            this.orderBy = " order by " + column+" desc ";
            return this;
        }
        public Query GroupBy(string column)
        {
            this.groupBy = " group by " + column;
            return this;
        }
        public Query AddParam(QueryParam param)
        {
            this.paramList.Add(param);
            return this;
        }
    }
}
