using System.Runtime.InteropServices;

namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Database;

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Based on https://github.com/dotnet/efcore/blob/main/src/EFCore/ValueGeneration/SequentialGuidValueGenerator.cs
public static class SequentialGuid
{
    private static long _counter = DateTime.UtcNow.Ticks;

    public static Guid NewSequentialGuid()
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Guid.NewGuid().TryWriteBytes(guidBytes);

        var incrementedCounter = Interlocked.Increment(ref _counter);

        Span<byte> counterBytes = stackalloc byte[sizeof(long)];
        MemoryMarshal.Write(counterBytes, in incrementedCounter);

        if (!BitConverter.IsLittleEndian)
        {
            counterBytes.Reverse();
        }

        guidBytes[08] = counterBytes[1];
        guidBytes[09] = counterBytes[0];
        guidBytes[10] = counterBytes[7];
        guidBytes[11] = counterBytes[6];
        guidBytes[12] = counterBytes[5];
        guidBytes[13] = counterBytes[4];
        guidBytes[14] = counterBytes[3];
        guidBytes[15] = counterBytes[2];

        return new Guid(guidBytes);
    }
}
