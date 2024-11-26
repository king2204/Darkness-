//Bandit is a derived class of Character

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn_Based_Game
{
    public class Bandit : Character
    {
      public Bandit()
      : base((int)CHAR_TYPES.BANDIT, //type number
           CHAR_NAMES[(int)CHAR_TYPES.BANDIT], //type name
           3, //strength
           1, //inteligence
           50, //totalHP
           4, //totalSp
           9, //speed
           true, //isAlive
           false, //isHerp
           "UNKNOWN", //char imaage file name
           2, // physical def
           2, //magical def
           (int)DMG_TYPES.PHYSICAL) // damge type
      {
      }
   }
}