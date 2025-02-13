﻿using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Util.Buffers.Packets;

public static class PacketWriterExtensions
{
    public static IPacketWriter Write(
        this IPacketWriter writer,
        IPacketWritable writable
    )
    {
        writable.WriteTo(writer);
        return writer;
    }

    public static IPacketWriter WritePoint2D(
        this IPacketWriter writer,
        IPoint2D point
    )
    {
        writer.WriteShort((short)point.X);
        writer.WriteShort((short)point.Y);
        return writer;
    }

    public static IPacketWriter WriteDateTime(
        this IPacketWriter writer,
        DateTime date
    )
    {
        writer.WriteLong(date.ToFileTimeUtc());
        return writer;
    }
}
