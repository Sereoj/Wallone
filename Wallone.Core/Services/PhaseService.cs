using Wallone.Core.Models;

namespace Wallone.Core.Services
{
    public class PhaseService
    {
        private static Phase phase = new Phase();

        public static Phase GetPhase()
        {
            return phase;
        }
    }
}