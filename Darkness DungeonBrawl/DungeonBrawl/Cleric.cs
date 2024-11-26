//Cleric is a derived class of Character

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn_Based_Game
{
   public class Cleric : Character
   {
      public Cleric()
      : base((int)CHAR_TYPES.CLERIC, //type number
           CHAR_NAMES[(int)CHAR_TYPES.CLERIC], //type name
           2, //strength
           3, //inteligence
           70, //totalHP
           15, //totalSp
           10, //speed
           true, //isAlive
           true, //isHerp
           "UNKNOWN", //char imaage file name
           5, // physical def
           5, //magical def
           (int)DMG_TYPES.PHYSICAL) // damge type
      {
      }
   }
}