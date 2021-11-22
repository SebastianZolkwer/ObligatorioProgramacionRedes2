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

        public void Validate()
        {
            if (String.IsNullOrEmpty(this.Title))
            {
                throw new InvalidOperationException("El titulo no puede ser nulo o vacio");
            }
            if (String.IsNullOrEmpty(this.Gender))
            {
                throw new InvalidOperationException("El genero no puede ser nulo o vacio");
            }
            if (String.IsNullOrEmpty(this.Sinopsis))
            {
                throw new InvalidOperationException("La sinopsis ser nula o vacia");
            }
        }
    }

    
}
