using System;

namespace Wallone.Core.Models
{
    public class Phase
    {
        public DateTime dawnSolarTime;
        public DateTime sunriseSolarTime;
        public DateTime daySolarTime;
        public DateTime goldenSolarTime;
        public DateTime sunsetSolarTime;
        public DateTime duskSolarTime;
        public DateTime nightSolarTime;

        public Times currentPhase;

        public TimeSpan nextPhaseSpan;
    }
}