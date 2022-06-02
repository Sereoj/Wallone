using System;
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

        public static void SetModel(Phase phaseModel)
        {
            phase = phaseModel;
        }

        public static void SetCurrentPhase(Times times)
        {
            phase.currentPhase = times;
        }
    }
}