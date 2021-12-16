using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit
{
    public class LearningActivityDTO
    {
        public string Description { get; init; }
        public string Activity { get; init; }
        public string Offerings { get; init; }

        public override string ToString() {
            return Description;
        }
    }
}