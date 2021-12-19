using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit
{
    public class LearningActivityDto
    {
        public string Description { get; init; } = string.Empty;
        public string Activity { get; init; } = string.Empty;
        public string Offerings { get; init; } = string.Empty;

        public override string ToString() {
            return Description;
        }
    }
}