using Macquarie.Handbook.Helpers.Extensions;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public record CreditPoint
    {
        private uint _creditPointValue;
        public uint Value {
            get => _creditPointValue;
            init => _creditPointValue = value.Clamp(0, int.MaxValue); //TODO Revise maximum clamp value.
        }

        public CreditPoint(uint credits) => Value = credits;

        public override string ToString() {
            return Value.ToString();
        }
    }
}