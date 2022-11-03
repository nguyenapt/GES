using GES.Clients.Web.Infrastructure.Rendering;
using GES.Common.Enumeration;
using GES.Common.Factories;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GES.Clients.Web.Models;
using GES.Common.Configurations;
using Enumerable = System.Linq.Enumerable;

namespace GES.Clients.Web.Extensions
{
    public static class CaseReportExtensions
    {
        public static MvcHtmlString RenderCaseReport(this HtmlHelper helper, GesCaseReportType reportType)
        {
            var builderFactory = DependencyResolver.Current.GetService<IServiceFactory<CaseProfileBuilder>>();

            if (builderFactory != null)
            {
                switch (reportType)
                {

                    case GesCaseReportType.StConfirmed:
                    case GesCaseReportType.StIndicationOfViolation:
                    case GesCaseReportType.StAlert:
                    case GesCaseReportType.StResolved:
                    case GesCaseReportType.StArchived:
                        var builderSt = builderFactory.Get(reportType.ToString());
                        if (builderSt != null)
                        {
                            // Construct the case profile view
                            builderSt.Initialize(helper)
                                .BuildLeftView()
                                .BuilRightView();

                            return builderSt.GetView();
                        }
                        break;
                    default:
                        var builder = new FullAttributeBuilder();
                        if (builder != null)
                        {
                            // Construct the case profile view
                            builder.Initialize(helper)
                                .BuildLeftView()
                                .BuilRightView();

                            return builder.GetView();
                        }
                        break;
                }
            }

            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString RenderBlockContentExport<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, string>> propertyExpression, DateTime? dateTime = null,
            string popupModalTitle = null, string blockContentViewPath = null)
        {
            return RenderBlockContent(helper, propertyExpression, dateTime, popupModalTitle,
                blockContentViewPath ?? "~/Views/Company/CaseProfiles/Shared/_BlockContentExport.cshtml");
        }

        public static MvcHtmlString RenderBlockContent<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, string>> propertyExpression, DateTime? dateTime = null, string popupModalTitle = null, string blockContentViewPath = null, string blockTitle = null)
        {
            var model = (TModel)helper.ViewContext.ViewData.Model;
            var content = propertyExpression.Compile().Invoke(model);
            var isValidContent = !string.IsNullOrWhiteSpace(content) && content.ToLower().Trim() != Configurations.NullContent;

            return isValidContent
                ? GetBlockContent(helper, blockTitle ?? helper.DisplayNameFor(propertyExpression).ToHtmlString(),
                    dateTime, popupModalTitle, content, blockContentViewPath: blockContentViewPath)
                : MvcHtmlString.Empty;
        }

        public static MvcHtmlString RenderBlockContentWithCustomContent<TModel>(this HtmlHelper<TModel> helper, string title, string customContent, DateTime? dateTime = null, string popupModalTitle = null, string blockContentViewPath = null)
        {
            var isValidContent = !string.IsNullOrWhiteSpace(customContent) && customContent.ToLower().Trim() != Configurations.NullContent;

            return isValidContent ? GetBlockContent(helper, title, dateTime, popupModalTitle, null, customContent, blockContentViewPath) : MvcHtmlString.Empty;
        }

        public static MvcHtmlString RenderBlockContentWithListValues<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IEnumerable<string>>> propertyExpression)
        {
            var model = (TModel)helper.ViewContext.ViewData.Model;
            var value = propertyExpression.Compile().Invoke(model);
            var contents = value as string[] ?? Enumerable.ToArray(value);
            var customContent = string.Empty;

            if (contents.Any())
            {
                customContent = Enumerable.Aggregate(contents, "<ul class='convention-list'>", (current, content) => current + $"<li>{content}</li>");
                customContent += "</ul>";
            }

            return contents.Any() ? GetBlockContent(helper, helper.DisplayNameFor(propertyExpression).ToHtmlString(), null, null, customContent) : MvcHtmlString.Empty;
        }

        private static MvcHtmlString GetBlockContent<TModel>(HtmlHelper<TModel> helper, string title, DateTime? dateTime, string popupModalTitle, string content, string customContent = null, string blockContentViewPath = null)
        {
            return helper.Partial(blockContentViewPath ?? "~/Views/Company/CaseProfiles/Shared/_BlockContent.cshtml",
                new BlockContentModel
                {
                    Title = title,
                    DateTime = dateTime ?? new DateTime(),
                    Content = content,
                    PopupModalTitle = popupModalTitle,
                    CustomContent = customContent
                });
        }

        public static MvcHtmlString RenderProperty<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, string>> propertyExpression, string cssClass = "")
        {
            var model = (TModel)helper.ViewContext.ViewData.Model;

            var property = propertyExpression.Compile().Invoke(model);

            if (!string.IsNullOrEmpty(property))
            {
                var tagBuilder = new StringBuilder();
                tagBuilder.AppendLine($@"<dt>{helper.DisplayNameFor(propertyExpression).ToHtmlString()}</dt>");
                tagBuilder.AppendLine($"<dd class='{cssClass}'>{property}</dd>");

                return MvcHtmlString.Create(tagBuilder.ToString());
            }

            return MvcHtmlString.Empty;
        }
    }
}