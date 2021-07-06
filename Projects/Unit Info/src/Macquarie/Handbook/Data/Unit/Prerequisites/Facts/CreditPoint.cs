using System;
using Macquarie.Handbook.Helpers.Extensions;

namespace Macquarie.Handbook.Data.Unit.Prerequisites.Facts
{
    public record CreditPoint
    {
        private int _creditPointValue;
        public int Value {
            get => _creditPointValue;
            init => _creditPointValue = value.Clamp(0, int.MaxValue); //TODO Revise maximum clamp value.
        }
    }
}