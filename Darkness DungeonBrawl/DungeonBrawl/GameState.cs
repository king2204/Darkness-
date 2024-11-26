//GameState is a class that save all information about Characters (fighters), level number, and in progress game states

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn_Based_Game
{
    public class GameState
    {
      public enum MOVES { ATT, DEF, SPEC, NO_SELECT };
      public const int NO_TARGET = -1; //Used to dtermine no target is selected
      public const int HEROES_DEFEATED = 1; //All heroes defeated
      public const int ENEMIES_DEFEATED = 2; //All enemines defeated
      public const int GAME_CONTINUE = 0; //Heroes and enemies still alive

      //State vars
      private int levelNum;
      private bool gameOver;
      private int selectedChar; //current character selected to att, def, special
      private int selectedMove; //ATT, DEF, SPEC
      private int selectedTarget; //int from Character.CHAR_TYPES;
      private int[] battleOrder; // Order of character battle order, contains Character constants 
                                 //from Character.CHAR_TYPE
      private int orderIndex;    //Current index of battle order
      private bool allHeroesDead; //Flag for all heroes defeated
      private bool allEnemiesDead; //Flag for all enemies defeated

      //Character vars
      private Character character; // must be called to assign CHAR_NAME dictionary values
      Character[] characters;

      public GameState()
      {
         ResetState();
      }

      public void ResetState()
      {
         levelNum = 1;
         gameOver = false;
         character = new Character();
         Characters = new Character[] 
         { 
            //do not alter this order, maps to Character.CHAR_TYPES constants
            new Warrior(), new Mage(), 
            new Cleric(), new Bandit(), 
            new Ogre(), new Dragon() 
         };
         GetBattleOrder();
         OrderIndex = 0;
         selectedTarget = NO_TARGET;
         SelectedChar = BattleOrder[0];
         AllHeroesDead = false;
         AllEnemiesDead = false;
      }

      //Revive and restore stats for enemies on a new level
      public void SetupNewLevel()
      {
         AllEnemiesDead = false;

         Characters[(int)Character.CHAR_TYPES.BANDIT].Hp 
            = Characters[(int)Character.CHAR_TYPES.BANDIT].TotalHp;
         Characters[(int)Character.CHAR_TYPES.BANDIT].Sp
           = Characters[(int)Character.CHAR_TYPES.BANDIT].TotalSp;
         Characters[(int)Character.CHAR_TYPES.BANDIT].IsAlive = true;
         Characters[(int)Character.CHAR_TYPES.BANDIT].IsDefending = false;

         Characters[(int)Character.CHAR_TYPES.OGRE].Hp
           = Characters[(int)Character.CHAR_TYPES.OGRE].TotalHp;
         Characters[(int)Character.CHAR_TYPES.OGRE].Sp
           = Characters[(int)Character.CHAR_TYPES.OGRE].TotalSp;
         Characters[(int)Character.CHAR_TYPES.OGRE].IsAlive = true;
         Characters[(int)Character.CHAR_TYPES.OGRE].IsDefending = false;

         Characters[(int)Character.CHAR_TYPES.DRAGON].Hp
           = Characters[(int)Character.CHAR_TYPES.DRAGON].TotalHp;
         Characters[(int)Character.CHAR_TYPES.DRAGON].Sp
           = Characters[(int)Character.CHAR_TYPES.DRAGON].TotalSp;
         Characters[(int)Character.CHAR_TYPES.DRAGON].IsAlive = true;
         Characters[(int)Character.CHAR_TYPES.DRAGON].IsDefending = false;
      }

      public int CheckEndGame()
      {
         if (!Characters[(int)Character.CHAR_TYPES.WARRIOR].IsAlive
            && !Characters[(int)Character.CHAR_TYPES.MAGE].IsAlive
            && !Characters[(int)Character.CHAR_TYPES.CLERIC].IsAlive)
            return HEROES_DEFEATED;
         if (!Characters[(int)Character.CHAR_TYPES.BANDIT].IsAlive
           && !Characters[(int)Character.CHAR_TYPES.OGRE].IsAlive
           && !Characters[(int)Character.CHAR_TYPES.DRAGON].IsAlive)
            return ENEMIES_DEFEATED;
         return GAME_CONTINUE;
      }

      public void UpdateSelectedChar()
      {
         //Go to next alive character in battle sequence
         do
         {
            OrderIndex = (OrderIndex + 1) % 6;
            SelectedChar = BattleOrder[OrderIndex];
         } while (!Characters[SelectedChar].IsAlive);
      }

      private void GetBattleOrder()
      {
         BattleOrder = new int[]
         {
            (int)Character.CHAR_TYPES.WARRIOR,
            (int)Character.CHAR_TYPES.MAGE,
            (int)Character.CHAR_TYPES.CLERIC,
            (int)Character.CHAR_TYPES.BANDIT,
            (int)Character.CHAR_TYPES.OGRE,
            (int)Character.CHAR_TYPES.DRAGON
         };

         //Sink sort battle order by character speed, Higher speed goes first
         for(int i = 0; i < 6-1; i++)
            for(int j = 0; j < 6-1-i; j++)
            {
               if (Characters[BattleOrder[j]].Speed < Characters[BattleOrder[j + 1]].Speed)
                  SwapBattleOrder(j, BattleOrder[j], j + 1, BattleOrder[j + 1]);
            }
      }

      private void SwapBattleOrder(int indexone, int one, int indextwo, int two)
      {
         int temp = one;
         BattleOrder[indexone] = two;
         BattleOrder[indextwo] = temp;
      }

      public int LevelNum { get => levelNum; set => levelNum = value; }
      public bool GameOver { get => gameOver; set => gameOver = value; }

      public int SelectedChar { get => selectedChar; set => selectedChar = value; }
      public int SelectedMove { get => selectedMove; set => selectedMove = value; }
      public int SelectedTarget { get => selectedTarget; set => selectedTarget = value; }
      public Character[] Characters { get => characters; set => characters = value; }
      public int[] BattleOrder { get => battleOrder; set => battleOrder = value; }
      public int OrderIndex { get => orderIndex; set => orderIndex = value; }
      public bool AllHeroesDead { get => allHeroesDead; set => allHeroesDead = value; }
      public bool AllEnemiesDead { get => allEnemiesDead; set => allEnemiesDead = value; }
   }
}