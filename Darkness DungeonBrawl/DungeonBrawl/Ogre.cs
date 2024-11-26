//Ogre is a derived class of Character

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn_Based_Game
{
    public class Ogre : Character
    {
      public Ogre()
      : base((int)CHAR_TYPES.OGRE, //type number
           CHAR_NAMES[(int)CHAR_TYPES.OGRE], //type name
           5, //strength
           1, //inteligence
           90, //totalHP
           5, //totalSp
           6, //speed
           true, //isAlive
           false, //isHerp
           "UNKNOWN", //char imaage file name
           6, // physical def
           2, //magical def
           (int)DMG_TYPES.PHYSICAL) // damge type
      {
      }
   }
}