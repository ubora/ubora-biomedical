using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Ubora.Web.Infrastructure
{
    public class ViewRender
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly HttpContext _httpContext;
        private readonly ActionContext _actionContext;

        protected ViewRender()
        {  
        }

        public ViewRender(IRazorViewEngine viewEngine, 
            ITempDataProvider tempDataProvider, 
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor
            )
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _httpContext = httpContextAccessor.HttpContext;
            _actionContext = actionContextAccessor.ActionContext;
        }

        public virtual string Render<TModel>(string path, string fileName, TModel model)
        {
            var viewEngineResult = _viewEngine.GetView(path, fileName, false);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"Couldn't find path '{path}' or view '{fileName}'");
            }

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    _actionContext,
                    view,
                    new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        _actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions())
                {
                    RouteData = _httpContext.GetRouteData()
                    
                };

                view.RenderAsync(viewContext).GetAwaiter().GetResult();

                return output.ToString();
            }
        }
    }
}

