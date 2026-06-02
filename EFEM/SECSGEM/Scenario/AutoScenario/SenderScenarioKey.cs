using System;
using FrameOfSystem3.SECSGEM.DefineSecsGem;

namespace FrameOfSystem3.SECSGEM.Scenario
{
    public readonly struct SenderScenarioKey : IEquatable<SenderScenarioKey>
    {
        public SenderScenarioKey(string sender, EN_SCENARIO scenario)
        {
            Sender = sender ?? string.Empty;
            Scenario = scenario;
        }

        public string Sender { get; }
        public EN_SCENARIO Scenario { get; }

        public bool Equals(SenderScenarioKey other)
        {
            return string.Equals(Sender, other.Sender, StringComparison.Ordinal)
                && Scenario == other.Scenario;
        }

        public override bool Equals(object obj)
        {
            return obj is SenderScenarioKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Sender != null ? Sender.GetHashCode() : 0) * 397) ^ (int)Scenario;
            }
        }
    }
}