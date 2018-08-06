using System;
using Ubora.Domain.Infrastructure.Commands;

// ReSharper disable once InconsistentNaming
// ReSharper disable once CheckNamespace
public static class ICommandResultExtensions
{
    public static ICommandResult OnSuccess(this ICommandResult @this, Action<ICommandResult> handler)
    {
        if (@this.IsSuccess)
        {
            handler(@this);
        }
        return @this;
    }

    public static ICommandResult OnFailure(this ICommandResult @this, Action<ICommandResult> handler)
    {
        if (@this.IsFailure)
        {
            handler(@this);
        }
        return @this;
    }
}