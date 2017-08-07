using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Ubora.Web.Services
{
    public interface IViewRender
    {
        string Render<TModel>(string path, string fileName, TModel model);
    }

    public class ViewRender : IViewRender
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly HttpContext _context;

        public ViewRender(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IHttpContextAccessor accessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _context = accessor.HttpContext;
        }

        public string Render<TModel>(string path, string fileName, TModel model)
        {
            var viewEngineResult = _viewEngine.GetView(path, fileName, false);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"Couldn't find path '{path}' or view '{fileName}'");
            }

            var actionContext = new ActionContext(_context, new RouteData(), new ActionDescriptor());
            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions())
                {
                    RouteData = _context.GetRouteData()
                    
                };

                view.RenderAsync(viewContext).GetAwaiter().GetResult();

                return output.ToString();
            }
        }
    }
}

