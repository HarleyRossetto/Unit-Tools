namespace Macquarie.Handbook.Helpers.Extensions
{
    public static class IntExtensions
    {
        public static int Clamp(this int value, int minimumValue, int maximumValue) {
            if (value < minimumValue)   return minimumValue;
            if (value > maximumValue)   return maximumValue;
            return value;
        }
    }
}