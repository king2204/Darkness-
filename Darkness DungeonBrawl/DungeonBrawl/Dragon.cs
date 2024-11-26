//Dragon is a derived class of Character

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn_Based_Game
{
    public class Dragon : Character
    {
      public Dragon()
      : base((int)CHAR_TYPES.DRAGON, //type number
           CHAR_NAMES[(int)CHAR_TYPES.DRAGON], //type name
           6, //strength
           6, //inteligence
           140, //totalHP
           2, //totalSp
           2, //speed
           true, //isAlive
           false, //isHerp
           "UNKNOWN", //char imaage file name
           7, // physical def
           7, //magical def
           (int)DMG_TYPES.MAGIC) // damge type
      {
      }
   }
}