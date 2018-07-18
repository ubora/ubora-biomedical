using JetBrains.Annotations;

// https://youtrack.jetbrains.com/issue/RSRP-461882

// {0} - Action Name
// {1} - Controller Name
// {2} - Area Name

[assembly: AspMvcAreaViewLocationFormat(@"~\_Areas\{2}\{1}\{0}.cshtml")]
[assembly: AspMvcAreaPartialViewLocationFormat(@"~\_Areas\{2}\_Shared\{0}.cshtml")]
[assembly: AspMvcAreaPartialViewLocationFormat(@"~\_Features\_Shared\{0}.cshtml")]

[assembly: AspMvcViewLocationFormat(@"~\_Features\_Shared\{0}.cshtml")]
[assembly: AspMvcPartialViewLocationFormat(@"~\_Features\_Shared\")]

[assembly: AspMvcViewLocationFormat(@"~\_Features\{1}\{0}.cshtml")]

[assembly: AspMvcViewLocationFormat(@"~\_Features\Users\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\Admin\{1}\{0}.cshtml")]

[assembly: AspMvcViewLocationFormat(@"~\_Features\Projects\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\Projects\Workpackages\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\Projects\Workpackages\Reviews\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\Projects\Workpackages\Steps\{0}.cshtml")]