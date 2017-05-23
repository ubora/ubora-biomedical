using JetBrains.Annotations;

// https://youtrack.jetbrains.com/issue/RSRP-461882

// {0} - Action Name
// {1} - Controller Name
// {2} - Area Name
// {3} - Feature Name

[assembly: AspMvcViewLocationFormat(@"~\_Features\_Shared\{0}.cshtml")]
[assembly: AspMvcPartialViewLocationFormat(@"~\_Features\_Shared\")]

[assembly: AspMvcViewLocationFormat(@"~\_Features\Users\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\Users\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\Projects\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\ProjectCreation\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\ProjectList\{1}\{0}.cshtml")]
