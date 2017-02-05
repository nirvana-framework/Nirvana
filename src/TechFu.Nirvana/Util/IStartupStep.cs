using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TechFu.Nirvana.Util
{
    public interface IStartupStep
    {
        void Start();
    }


    public class Startup
    {
        private readonly IStartupStep[] _steps;

        public Startup(IStartupStep[] steps)
        {
            _steps = steps;
        }

        public void Start()
        {
            var stopwatch = new Stopwatch();
            var diagnostics = new StringBuilder();
            foreach (var step in _steps.OrderBy(a => a.GetType().Name))
            {
                var name = step.GetType().Name;
                try
                {
                    stopwatch.Restart();
                    step.Start();
                    diagnostics.AppendFormat("Startup Timing:\t{0}\t\t{1}\r\n", name, stopwatch.Elapsed.TotalSeconds);
                }
                catch
                {
                }
            }
        }
    }

}