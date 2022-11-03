using System;
using System.Collections.Generic;
using GES.Common.Models;

namespace GES.Common.Helpers
{
    public static class LinqDynamicConditionHelper
    {
        private static readonly Dictionary<string, string> WhereOperation =
                new Dictionary<string, string>
                {
                    { "eq", "{1} =  {2}({0})" },
                    { "ne", "{1} != {2}({0})" },
                    { "lt", "{1} <  {2}({0})" },
                    { "le", "{1} <= {2}({0})" },
                    { "gt", "{1} >  {2}({0})" },
                    { "ge", "{1} >= {2}({0})" },
                    { "bw", "{1}.StartsWith({2}({0}))" },
                    { "bn", "!{1}.StartsWith({2}({0}))" },
                    { "in", "" },
                    { "ni", "" },
                    { "ew", "{1}.EndsWith({2}({0}))" },
                    { "en", "!{1}.EndsWith({2}({0}))" },
                    { "cn", "{1}.ToLower().Contains({2}({0}).ToLower())" },
                    { "nc", "!{1}.ToLower().Contains({2}({0}).ToLower())" },
                    { "nu", "{1} == null" },
                    { "nn", "{1} != null" }
                };

        private static readonly Dictionary<string, string> ValidOperators =
                new Dictionary<string, string>
                {
                    { "Object"   ,  "nu:" },
                    { "Boolean"  ,  "eq:ne:nu:" },
                    { "Char"     ,  "" },
                    { "String"   ,  "eq:ne:lt:le:gt:ge:bw:bn:cn:nc:ew:en:nu:nn:" },
                    { "SByte"    ,  "" },
                    { "Byte"     ,  "eq:ne:lt:le:gt:ge:" },
                    { "Int16"    ,  "eq:ne:lt:le:gt:ge:nu:nn:" },
                    { "UInt16"   ,  "" },
                    { "Int32"    ,  "eq:ne:lt:le:gt:ge:nu:nn:" },
                    { "UInt32"   ,  "" },
                    { "Int64"    ,  "eq:ne:lt:le:gt:ge:nu:nn:" },
                    { "UInt64"   ,  "" },
                    { "Decimal"  ,  "eq:ne:lt:le:gt:ge:nu:nn:" },
                    { "Single"   ,  "eq:ne:lt:le:gt:ge:nu:nn:" },
                    { "Double"   ,  "eq:ne:lt:le:gt:ge:nu:nn:" },
                    { "DateTime" ,  "eq:ne:lt:le:gt:ge:nu:nn:" },
                    { "TimeSpan" ,  "" },
                    { "Guid"     ,  "" }
                };

        public static string GetCondition<T>(JqGridRuleModel rule)
        {
            if (rule.data == "%")
            {
                // returns an empty string when the data is ‘%’  
                return "";
            }
            else
            {
                // initializing variables  
                Type myType = null;
                var myTypeName = string.Empty;
                var myPropInfo = typeof(T).GetProperty(rule.field.Split('.')[0]);
                var index = 0;
                // navigating fields hierarchy  
                foreach (var wrkField in rule.field.Split('.'))
                {
                    if (index > 0)
                    {
                        myPropInfo = myPropInfo.PropertyType.GetProperty(wrkField);
                    }
                    index++;
                }
                // property type and its name  
                myType = myPropInfo.PropertyType;
                myTypeName = myPropInfo.PropertyType.Name;
                // handling ‘nullable’ fields  
                var myTypeNameLowerCase = myTypeName.ToLower();
                if (myTypeNameLowerCase.Contains("nullable"))
                {
                    myType = Nullable.GetUnderlyingType(myPropInfo.PropertyType);
                    myTypeName = myType.Name;
                }
                // creating the condition expression  
                if (ValidOperators[myTypeName].Contains(rule.op + ":"))
                {
                    var expression = String.Format(WhereOperation[rule.op],
                                                      GetFormattedData(myType, rule.data),
                                                      rule.field,
                                                      myTypeName);

                    if (myType.Name.ToLower() == "datetime" && rule.op == "eq")
                    {
                        var dt = DateTime.Parse(rule.data);
                        var dt_next = dt.AddDays(1);
                        var value_next = dt_next.Year.ToString() + "," +
                            dt_next.Month.ToString() + "," +
                            dt_next.Day.ToString();
                        expression = String.Format("{0} >= {1}({2}) && {0} < {1}({3})",
                                                      rule.field,
                                                      myTypeName,
                                                      GetFormattedData(myType, rule.data),
                                                      value_next);
                    }

                    return expression;
                }
                else
                {
                    // un-supported operator  
                    return "";
                }
            }
        }
        private static string GetFormattedData(Type type, string value)
        {
            switch (type.Name.ToLower())
            {
                case "string":
                    value = @"""" + value + @"""";
                    break;
                case "datetime":
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var dt = DateTime.ParseExact(value, Configurations.Configurations.DateFormat, null);
                        value = dt.Year.ToString() + "," +
                                dt.Month.ToString() + "," +
                                dt.Day.ToString();
                    }
                    break;
            }
            return value;
        }
    }
}
