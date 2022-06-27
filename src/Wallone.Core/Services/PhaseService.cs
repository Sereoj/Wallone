using System;
using Wallone.Core.Models;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Services
{
    public class PhaseRepository
    {
        private static Phase phase;
        public class PhaseService
        {
            public static void SetCurrentPhase(Times times)
            {
                if (phase != null)
                {
                    phase.currentPhase = times;
                    LoggerService.Log(typeof(PhaseService), $"times {times}");
                }
            }
        }

        public static void Load(Phase phaseModel)
        {
            phase = phaseModel;
        }
        public static Phase Get()
        {
            return phase;
        }
    }
}