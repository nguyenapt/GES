using System.Text;
using System.Web.Script.Serialization;
using GES.Common.Helpers;
using GES.Common.Models;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Helpers
{
    public class JqGridHelper
    {
        public static string GetFilterRules<T>(JqGridViewModel jqGridParams)
        {
            var filterRuleBuilder = new StringBuilder();
            var serializer = new JavaScriptSerializer();
            var filters = serializer.Deserialize<JqGridFilterModel>(jqGridParams.filters);
            var total = filters.rules.Count;
            for (var i = 0; i < total; i++)
            {
                var condition = LinqDynamicConditionHelper.GetCondition<T>(filters.rules[i]);
                if (condition.Length > 0)
                {
                    filterRuleBuilder.Append(condition);
                    if (i != total - 1)
                        filterRuleBuilder.Append(filters.GroupOperator);
                }
            }
            return filterRuleBuilder.ToString();
        }
    }
}
