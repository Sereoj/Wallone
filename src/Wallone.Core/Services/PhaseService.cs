using System;
using System.Collections.Generic;
using Wallone.Core.Models;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Services
{
    public class PhaseFactory
    {
        public static Phase Create()
        {
            return new Phase();
        }
    }
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
                }
            }

            public static void SetNextPhaseSpan(TimeSpan timeSpan)
            {
                if (phase != null)
                {
                    phase.nextPhaseSpan = timeSpan;
                }
            }

            public static Times CurrentPhase()
            {
                return phase.currentPhase;
            }

            public static Times NextPhase()
            {
                switch (CurrentPhase())
                {
                    case Times.Dawn:
                        SetCurrentPhase(Times.Sunrise);
                        return GetCurrentPhase();
                    case Times.Sunrise:
                        SetCurrentPhase(Times.Day);
                        return GetCurrentPhase();
                    case Times.Day:
                        SetCurrentPhase(Times.GoldenHour);
                        return GetCurrentPhase();
                    case Times.GoldenHour:
                        SetCurrentPhase(Times.Sunset);
                        return GetCurrentPhase();
                    case Times.Sunset:
                        SetCurrentPhase(Times.Night);
                        return GetCurrentPhase();
                    case Times.Night:
                        SetCurrentPhase(Times.Dawn);
                        return GetCurrentPhase();
                    case Times.NotFound:
                        return GetCurrentPhase();
                    default:
                        return GetCurrentPhase();
                }
            }

            private static Times GetCurrentPhase()
            {
                return CurrentPhase();
            }

            public static Times GetNextPhase()
            {
                switch (CurrentPhase())
                {
                    case Times.Dawn:
                        return Times.Sunrise;
                    case Times.Sunrise:
                        return Times.Day;
                    case Times.Day:
                        return Times.GoldenHour;
                    case Times.GoldenHour:
                        return Times.Sunset;
                    case Times.Sunset:
                        return Times.Night;
                    case Times.Night:
                        return Times.Dawn;
                    case Times.NotFound:
                        return Times.NotFound;
                    default:
                        return Times.NotFound;
                }
            }

            public static Times GetPreviousPhase()
            {
                switch (CurrentPhase())
                {
                    case Times.Dawn:
                        return Times.Night;
                    case Times.Sunrise:
                        return Times.Dawn;
                    case Times.Day:
                        return Times.Sunrise;
                    case Times.GoldenHour:
                        return Times.Day;
                    case Times.Sunset:
                        return Times.GoldenHour;
                    case Times.Night:
                        return Times.Sunset;
                    case Times.NotFound:
                        return Times.NotFound;
                    default:
                        return Times.NotFound;
                }
            }
        }

        public class Math
        {
            public static TimeSpan CalcSunTimes(DateTime nowDateTime, DateTime nextDateTime, int imageCount)
            {
                if (imageCount == 0)
                {
                    _ = LoggerService.LogAsync(typeof(Math), $"Изображений не должно быть 0!");
                    imageCount = 1;
                }
                
                var timeSpan = (nextDateTime - nowDateTime) / imageCount;
                return timeSpan;
            }
        }

        public class Time
        {
            public static void SetNextTime(TimeSpan nextImageTimeSpan)
            {
                phase.nextPhaseSpan = nextImageTimeSpan;
            }

            public static TimeSpan GetNextTime()
            {
                return phase.nextPhaseSpan;
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

        public static Phase Set(Phase phase)
        {
            Load(phase);
            return Get();
        }
    }
}