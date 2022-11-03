using System.Collections.Generic;

namespace GES.Common.Models
{
    public class JqGridFilterModel
    {
        private const string And = "and";
        private const string AndOperator = "&&";
        private const string OrOperator = "||";

        public string groupOp { get; set; }

        public List<JqGridRuleModel> rules { get; set; }

        public string GroupOperator => groupOp.ToLower() == And ? $" {AndOperator} " : $" {OrOperator} ";

        //public IList<FilterModel> groups { get; set; }
    }
}