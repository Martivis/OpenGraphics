using System.Diagnostics;

namespace OpenGraphics;

public class FPSCounter
{
    private readonly Stopwatch _stopwatch;

    private uint _framesCount;
    private double _lastCutoff;

    public FPSCounter()
    {
        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }

    public void CountFps()
    {
        _framesCount++;
        if (_stopwatch.Elapsed.TotalSeconds >= _lastCutoff + 1)
        {
            _lastCutoff = _stopwatch.Elapsed.TotalSeconds;
            Console.Clear();
            Console.WriteLine($"Fps: {_framesCount}");
            _framesCount = 0;
        }
    }
}
