using GES.Inside.Data.Models;
using System.Web.Mvc;
using System;

namespace GES.Clients.Web.Infrastructure.Rendering
{

    /// <summary>
    /// Define the builder class that will be used to build the Case Profile view.
    /// </summary>
    public abstract class CaseProfileBuilder : ViewBuilder
    {
        protected HtmlHelper Helper;

        /// <summary>
        /// Build left part of view
        /// </summary>
        public abstract CaseProfileBuilder BuildLeftView();
        
        /// <summary>
        /// Build right part of view
        /// </summary>
        public abstract CaseProfileBuilder BuilRightView();

        /// <summary>
        /// Method to initialize the view
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public virtual CaseProfileBuilder Initialize(HtmlHelper helper)
        {
            this.viewBuilder.Clear();

            if (helper == null)
                throw new ArgumentNullException("Helper not allow to null");

            this.Helper = helper;

            return this;
        }

        protected override string GetViewPath(string viewName)
        {
            if (!string.IsNullOrEmpty(viewName) && viewName.EndsWith(".cshtml", StringComparison.Ordinal)) // Absolute path
                return viewName;

            // Relative path
            return $"~/Views/Company/CaseProfiles/Shared/{viewName}.cshtml";
        }
    }

    /// <summary>
    /// Define the generic type that used to build the CaseProfile view
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class CaseProfileBuilder<TViewModel> : CaseProfileBuilder
        where TViewModel : CaseProfileCoreViewModel
    {
        protected TViewModel ViewModel;
        
        /// <summary>
        /// Initialize & clear the view 
        /// </summary>
        public override CaseProfileBuilder Initialize(HtmlHelper helper)
        {
            base.Initialize(helper);
            
            if (helper.ViewContext?.ViewData?.Model == null)
                throw new ArgumentNullException("Model not allow to null");

            ViewModel = helper.ViewContext.ViewData.Model as TViewModel;

            if (ViewModel == null)
                throw new ArgumentException(
                    $"The required model is {typeof(TViewModel).Name} but the input model is {helper.ViewContext.ViewData.Model.GetType().Name}");

            return this;
        }
    }
}