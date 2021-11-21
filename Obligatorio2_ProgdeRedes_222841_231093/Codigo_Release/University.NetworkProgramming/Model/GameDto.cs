using Bussiness.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class GameDto
    {
        public string Title { get; set; }
        public string Gender { get; set; }
        public string Sinopsis { get; set; }
        public int AverageQualification { get; set; }

        public GameDto(Game game)
        {
            this.Title = game.Title;
            this.Gender = game.Gender;
            this.Sinopsis = game.Sinopsis;
            this.AverageQualification = game.AverageQualification;
        }
    }
}
