using System;
using System.Collections.Generic;
using System.Threading;

public static class IdGenerator
{
    public static IEnumerable<string> Generate()
    {
        long lastId = DateTime.UtcNow.Ticks;

        while (true)
        {
            long currentId = DateTime.UtcNow.Ticks;
            long candidateId = Interlocked.Read(ref lastId);
            if (currentId <= candidateId)
            {
                currentId = candidateId + 1;
            }

            Interlocked.Exchange(ref lastId, currentId);
            yield return currentId.ToString("X");
        }
    }
}