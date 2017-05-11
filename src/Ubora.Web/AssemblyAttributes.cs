using JetBrains.Annotations;

// https://youtrack.jetbrains.com/issue/RSRP-461882
[assembly: AspMvcViewLocationFormat(@"~\_Features\Projects\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\Users\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\_Features\_Shared\{0}.cshtml")]
