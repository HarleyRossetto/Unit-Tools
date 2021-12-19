namespace Macquarie.Handbook.Helpers.Extensions
{
    public static class IntExtensions
    {
        public static uint Clamp(this uint value, uint minimumValue, uint maximumValue) {
            if (value < minimumValue)   return minimumValue;
            if (value > maximumValue)   return maximumValue;
            return value;
        }

        public static int Clamp(this int value, int minimumValue, int maximumValue) {
            if (value < minimumValue)   return minimumValue;
            if (value > maximumValue)   return maximumValue;
            return value;
        }
    }
}