using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeanCloudCommond.Repository
{
    public class QueryParam
    {
        public string Column;
        public string Value;
        public string Querysign;
        public string Connect;
        public bool IsAddValueSign;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Column">字段名</param>
        /// <param name="Value">字段值</param>
        /// <param name="QuerySign">比较符号</param>
        /// <param name="ConnectSign">连接符号</param>
        /// <param name="isAddValueSign">是否为value添加引号</param>
        public QueryParam(string Column, string Value, string QuerySign, string ConnectSign,bool isAddValueSign)
        {
            this.Column = Column;
            this.Value = Value;
            this.Querysign = QuerySign;
            this.Connect = ConnectSign;
            this.IsAddValueSign = isAddValueSign;
        }
        public string ToQueryStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" "+this.Connect+" ");
            sb.Append(this.Column);
            sb.Append(" "+this.Querysign+" ");
            if (IsAddValueSign)
            {
                if (this.Querysign == QuerySign.LIKE)
                {
                    sb.Append("'%" + Value + "%'");
                }
                else
                {
                    sb.Append("'" + Value + "'");
                }
            }
            else
            {
                sb.Append(Value);
            }
            return sb.ToString();
        }

    }
    public static class QuerySign
    {
        /// <summary>
        /// 等于=
        /// </summary>
        public static readonly string EQ = "=";
        /// <summary>
        /// 模糊like
        /// </summary>
        public static readonly string LIKE = "like";
        /// <summary>
        /// 非模糊not like
        /// </summary>
        public static readonly string NOTLIKE = "not like";
        /// <summary>
        /// 包含in
        /// </summary>
        public static readonly string IN = "in";
        /// <summary>
        /// 不包含not in
        /// </summary>
        public static readonly string NOTIN = "not in";
        /// <summary>
        /// 是 is
        /// </summary>
        public static readonly string IS = "is";
        /// <summary>
        /// 不是  is not
        /// </summary>
        public static readonly string ISNOT = "is not";
        /// <summary>
        /// 不等于！=
        /// </summary>
        public static readonly string UNEQ = "!=";
        /// <summary>
        /// 小于
        /// </summary>
        public static readonly string LESS = "<";
        /// <summary>
        /// 大于
        /// </summary>
        public static readonly string LARGE = ">";
    }
    public static class ConnectSign
    {
        public static readonly string AND = "and";
        public static readonly string OR = "or";
    }
}
