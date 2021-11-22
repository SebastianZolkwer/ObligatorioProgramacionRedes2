using Bussiness.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class GameToShowAll
    {
        public string Title { get; set; }
        public string Gender { get; set; }

        public GameToShowAll(Game game)
        {
            this.Title = game.Title;
            this.Gender = game.Gender;
        }
    }
}
