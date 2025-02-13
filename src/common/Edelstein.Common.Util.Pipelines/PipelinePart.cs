﻿using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Util.Pipelines;

internal class PipelinePart<TMessage> : IComparer<PipelinePart<TMessage>>
{
    public PipelinePart(int priority, IPipelinePlug<TMessage> plug)
    {
        Priority = priority;
        Plug = plug;
    }

    private int Priority { get; }
    internal IPipelinePlug<TMessage> Plug { get; }

    public int Compare(PipelinePart<TMessage>? x, PipelinePart<TMessage>? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return x.Priority.CompareTo(y.Priority);
    }
}
