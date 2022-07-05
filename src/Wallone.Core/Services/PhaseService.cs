using System;
using System.Collections.Generic;
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
                    LoggerService.Log(typeof(PhaseService), $"{times}");
                }
            }

            public static void SetNextPhaseSpan(TimeSpan timeSpan)
            {
                if (phase != null)
                {
                    phase.nextPhaseSpan = timeSpan;
                    LoggerService.Log(typeof(PhaseService), $"SetNextPhaseSpan {timeSpan}");
                }
            }

            public static Times CurrentPhase()
            {
                return phase.currentPhase;
            }

            public static void NextPhase()
            {
                switch (CurrentPhase())
                {
                    case Times.Dawn:
                        SetCurrentPhase(Times.Sunrise);
                        break;
                    case Times.Sunrise:
                        SetCurrentPhase(Times.Day);
                        break;
                    case Times.Day:
                        SetCurrentPhase(Times.GoldenHour);
                        break;
                    case Times.GoldenHour:
                        SetCurrentPhase(Times.Sunset);
                        break;
                    case Times.Sunset:
                        SetCurrentPhase(Times.Night);
                        break;
                    case Times.Night:
                        SetCurrentPhase(Times.Dawn);
                        break;
                    case Times.NotFound:
                        break;
                    default:
                        break;
                }
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
        }

        public class Math
        {
            public static TimeSpan CalcSunTimes(DateTime nowDateTime, DateTime nextDateTime, int imageCount)
            {
                if (imageCount == 0)
                {
                    LoggerService.Log(typeof(Math), $"Изображений не должно быть 0!");
                    imageCount = 1;
                }

               var timeSpan = (nextDateTime - nowDateTime) / imageCount;

                LoggerService.Log(typeof(Math), $"SunSpanImages {timeSpan}");
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