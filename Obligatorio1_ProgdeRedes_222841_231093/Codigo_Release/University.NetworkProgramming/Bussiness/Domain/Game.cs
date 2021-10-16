using System;
using System.Collections.Generic;

namespace Bussiness.Domain
{
    public class Game
    {
        public string Title { get; set; }
        public string Gender { get; set; }
        public List<Review> Qualifications { get; set; }

        public int AverageQualification { get; set; }

        public int SumOfQualification { get; set; }

        public string Sinopsis { get; set;  }

        public string ImageRoute { get; set; }
    }
}
