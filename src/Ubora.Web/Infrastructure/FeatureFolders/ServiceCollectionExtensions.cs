using System;
using OdeToCode.AddFeatureFolders;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Use feature folders with custom options
        /// </summary>
        public static IMvcBuilder AddUboraFeatureFolders(this IMvcBuilder services, FeatureFolderOptions options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            var expander = new FeatureViewLocationExpander(options);

            services.AddMvcOptions(o =>
                {
                    o.Conventions.Add(new FeatureControllerModelConvention(options));
                })
                .AddRazorOptions(o =>
                {
                    o.ViewLocationFormats.Clear();

                    o.ViewLocationFormats.Add(options.FeatureNamePlaceholder + @"\{0}.cshtml");
                    o.ViewLocationFormats.Add(options.FeatureFolderName + @"\_Shared\{0}.cshtml");
                    o.ViewLocationFormats.Add(@"_Components\{0}.cshtml");
                    o.ViewLocationExpanders.Add(expander);

                    o.AreaViewLocationFormats.Add(@"_Areas\{2}\{1}\{0}.cshtml");
                    o.AreaViewLocationFormats.Add(@"_Areas\{2}\_Shared\{0}.cshtml");
                    o.AreaViewLocationFormats.Add(@"_Features\_Shared\{0}.cshtml");
                });

            return services;
        }

        /// <summary>
        /// Use feature folders with the default options. Controllers and view will be located
        /// under a folder named Features. Shared views are located in Features\Shared.
        /// </summary>
        public static IMvcBuilder AddUboraFeatureFolders(this IMvcBuilder services)
        {
            return AddUboraFeatureFolders(services, new FeatureFolderOptions());
        }
    }
}