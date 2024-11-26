//Warrior is a derived class of Character

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn_Based_Game
{
    public class Warrior : Character
    {
      public Warrior()
         : base((int)CHAR_TYPES.WARRIOR, //type number
              CHAR_NAMES[(int)CHAR_TYPES.WARRIOR], //type name
              8, //strength
              2, //inteligence
              100, //totalHP
              7, //totalSp
              5, //speed
              true, //isAlive
              true, //isHerp
              "UNKNOWN", //char imaage file name
              9, // physical def
              3, //magical def
              (int)DMG_TYPES.PHYSICAL) // damge type
      {
      }
    }
}