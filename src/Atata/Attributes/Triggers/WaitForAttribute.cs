﻿using System;

namespace Atata
{
    public abstract class WaitForAttribute : TriggerAttribute
    {
        protected WaitForAttribute(WaitUntil until, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
            Until = until;
        }

        protected enum WaitMethod
        {
            Presence,
            Absence
        }

        /// <summary>
        /// Gets the waiting approach.
        /// </summary>
        public WaitUntil Until { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to throw the exception on the presence (exists or visible) failure. The default value is true.
        /// </summary>
        public bool ThrowOnPresenceFailure { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to throw the exception on the absence (missing or hidden) failure. The default value is true.
        /// </summary>
        public bool ThrowOnAbsenceFailure { get; set; } = true;

        /// <summary>
        /// Gets or sets the presence (exists or visible) timeout in seconds. The default value is taken from <c>AtataContext.Current.RetryTimeout.TotalSeconds</c>.
        /// </summary>
        public double PresenceTimeout { get; set; } = AtataContext.Current.RetryTimeout.TotalSeconds;

        /// <summary>
        /// Gets or sets the absence (missing or hidden) timeout in seconds. The default value is taken from <c>AtataContext.Current.RetryTimeout.TotalSeconds</c>.
        /// </summary>
        public double AbsenceTimeout { get; set; } = AtataContext.Current.RetryTimeout.TotalSeconds;

        /// <summary>
        /// Gets or sets the retry interval. The default value is taken from <c>AtataContext.Current.RetryInterval.TotalSeconds</c>.
        /// </summary>
        public double RetryInterval { get; set; } = AtataContext.Current.RetryInterval.TotalSeconds;

        protected WaitUnit[] GetWaitUnits(WaitUntil until)
        {
            switch (until)
            {
                case WaitUntil.Missing:
                    return new[]
                    {
                        CreateAbsenceUnit(Visibility.Any)
                    };
                case WaitUntil.Hidden:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Hidden)
                    };
                case WaitUntil.MissingOrHidden:
                    return new[]
                    {
                        CreateAbsenceUnit(Visibility.Visible)
                    };
                case WaitUntil.Visible:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Visible)
                    };
                case WaitUntil.VisibleOrHidden:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Any)
                    };
                case WaitUntil.VisibleThenHidden:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Visible),
                        CreatePresenceUnit(Visibility.Hidden)
                    };
                case WaitUntil.VisibleThenMissing:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Visible),
                        CreateAbsenceUnit(Visibility.Any)
                    };
                case WaitUntil.VisibleThenMissingOrHidden:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Visible),
                        CreateAbsenceUnit(Visibility.Visible)
                    };
                case WaitUntil.MissingThenVisible:
                    return new[]
                    {
                        CreateAbsenceUnit(Visibility.Any),
                        CreatePresenceUnit(Visibility.Visible)
                    };
                case WaitUntil.HiddenThenVisible:
                    return new[]
                    {
                        CreatePresenceUnit(Visibility.Hidden),
                        CreatePresenceUnit(Visibility.Visible)
                    };
                case WaitUntil.MissingOrHiddenThenVisible:
                    return new[]
                    {
                        CreateAbsenceUnit(Visibility.Visible),
                        CreatePresenceUnit(Visibility.Visible)
                    };
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(until, nameof(until));
            }
        }

        private WaitUnit CreatePresenceUnit(Visibility visibility)
        {
            return new WaitUnit
            {
                Method = WaitMethod.Presence,
                Options = new SearchOptions
                {
                    Timeout = TimeSpan.FromSeconds(PresenceTimeout),
                    RetryInterval = TimeSpan.FromSeconds(RetryInterval),
                    Visibility = visibility,
                    IsSafely = !ThrowOnPresenceFailure
                }
            };
        }

        private WaitUnit CreateAbsenceUnit(Visibility visibility)
        {
            return new WaitUnit
            {
                Method = WaitMethod.Absence,
                Options = new SearchOptions
                {
                    Timeout = TimeSpan.FromSeconds(AbsenceTimeout),
                    RetryInterval = TimeSpan.FromSeconds(RetryInterval),
                    Visibility = visibility,
                    IsSafely = !ThrowOnAbsenceFailure
                }
            };
        }

        protected class WaitUnit
        {
            public WaitMethod Method { get; set; }

            public SearchOptions Options { get; set; }
        }
    }
}
