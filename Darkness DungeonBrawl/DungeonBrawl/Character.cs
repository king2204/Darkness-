//THis class describes a Character which is a fighter in a battle in Dungeon Brawl
//A character has several statistics and can attack, defend, or use a special attack

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turn_Based_Game
{
    public class Character
    {
        //Used to identify the type of character
        public enum CHAR_TYPES  { WARRIOR, MAGE, CLERIC, BANDIT, OGRE, DRAGON  };
        public enum DMG_TYPES { PHYSICAL, MAGIC };
        private static bool charNamesInitialized = false;
        public static Dictionary<int, string> CHAR_NAMES = new Dictionary<int, string>();

        private int charType;    //an integer from CHAR_TYPES above
        private string name;     //Correponds to strings in CHAR_NAMES
        private int strength;    //Physical strength
        private int intelligence;//Magical strength
        private int attackPts;   //A function of strength and inteligence
        private int hp;          //Health points
        private int totalHp;     //Total health points
        private int sp;          //Special ability points
        private int totalSp;     //Total special points
        private int speed;       //How fast the character reacts in battle
        private bool isAlive;    //Is alive or dead
        private bool isHero;     //Is a hero or an enemy
        private bool isDefending;//Is defending itself
        private string imageFileName;  //name of image file related to character
        private int defPhys; //Defense against physical attacks
        private int defMag; //Defense against magical attacks
        private int dmgType; //Deals physical or magical attacks DMG_TYPE PHYSICAL or MAGIC
        Random rand;


      //This constructor must be called prior to any derived classes being created if the derived classes
      // utilize the CHAR_NAMES dictionary
      public Character()
      {
         if (!charNamesInitialized)
         {
            InitCharNames();
            charNamesInitialized = true;
         }
      }
      public Character(int charType, 
         string name, int strength, 
         int intelligence,
         int totalHp, 
         int totalSp,
         int speed, bool isAlive,
         bool isHero, string immageFileName,
         int defPhys, int defMag,
         int dmgTpe)
      {
         //Initialize character names
         if (!charNamesInitialized)
         {
            throw new Exception();
         }

         CharType = charType;
         Name = name;
         Intelligence = intelligence;
         Strength = strength;
         AttackPts = GetAttPts();
         Hp = totalHp;
         TotalHp = totalHp;
         Sp = totalSp;
         TotalSp = sp;
         Speed = speed;
         IsAlive = isAlive;
         IsHero = isHero;
         ImageFileName = imageFileName;
         DefPhys = defPhys;
         DefMag = defMag;
         DmgType = dmgType;
         IsDefending = false;
         rand = new Random();
      }

      private static void InitCharNames()
      {
         CHAR_NAMES.Add((int)CHAR_TYPES.WARRIOR, "WARRIOR");
         CHAR_NAMES.Add((int)CHAR_TYPES.MAGE, "MAGE");
         CHAR_NAMES.Add((int)CHAR_TYPES.CLERIC, "CLERIC");
         CHAR_NAMES.Add((int)CHAR_TYPES.BANDIT, "BANDIT");
         CHAR_NAMES.Add((int)CHAR_TYPES.OGRE, "OGRE");
         CHAR_NAMES.Add((int)CHAR_TYPES.DRAGON, "DRAGON");
      }

      public int GetAttPts()
      {
         if(DmgType == (int)DMG_TYPES.PHYSICAL)
         {
            return 2*Strength + Intelligence;
         }
         else
         {
            return Strength + 2*Intelligence;
         }
      }

      public string Name { get => name; set => name = value; }
      public int Strength { get => strength; set => strength = value; }
      public int Intelligence { get => intelligence; set => intelligence = value; }
      public int AttackPts { get => attackPts; set => attackPts = value; }
      public int Hp { get => hp; set => hp = value; }
      public int TotalHp { get => totalHp; set => totalHp = value; }
      public int Sp { get => sp; set => sp = value; }
      public int TotalSp { get => totalSp; set => totalSp = value; }
      public int Speed { get => speed; set => speed = value; }
      public bool IsAlive { get => isAlive; set => isAlive = value; }
      public bool IsHero { get => isHero; set => isHero = value; }
      public string ImageFileName { get => imageFileName; set => imageFileName = value; }
      public int DefPhys { get => defPhys; set => defPhys = value; }
      public int DefMag { get => defMag; set => defMag = value; }
      public int DmgType { get => dmgType; set => dmgType = value; }
      public int CharType { get => charType; set => charType = value; }
      public bool IsDefending { get => isDefending; set => isDefending = value; }

      //Attack another character. Returns the amount of damage inflicted to the charToAttack
      public int Attack(Character charToAttack)
      {
         //Used to reduce damage if a character is defending
         int defense = 1;
         if (charToAttack.isDefending)
         {
            defense = 2;
            //defense lasts for one attack, enemy no longer defending
            charToAttack.IsDefending = false;
         }

         if(charToAttack.Hp - (AttackPts / defense) > 0)
         {
            charToAttack.Hp -= (AttackPts / defense);
            return AttackPts / defense;
         }
         else
         {
            int lostHp = charToAttack.Hp;
            charToAttack.Hp = 0;
            charToAttack.IsAlive = false;
            return lostHp;
         }
      }

      public void Defend()
      {
         IsDefending = true;
      }

      //Use a special move on another character. If selectedCharType is CLERIC, heal the charToEffect and return
      //points healed. Otherwise deal extra damage on the charToEffect and return damage taken
      public int SpecialMove(Character charToEffect, int selectedCharType, GameState gameState)
      {
         //Decrement SP by 1
         if (Sp - 1 >= 0)
            Sp--;
         //Out of SP do not attack
         else
         {
            return 0;
         }

         //Used to reduce damage if a character is defending AND CLERIC is not healing
         int defense = 1;
         if (charToEffect.isDefending && selectedCharType != (int)Character.CHAR_TYPES.CLERIC)
         {
            defense = 2;
            //defense lasts for one attack, enemy no longer defending
            charToEffect.IsDefending = false;
         }

         //If charType is CLERIC, heal the character to effect
         if (selectedCharType == (int)Character.CHAR_TYPES.CLERIC)
         {
            int healMult = rand.Next(1, 7);
            int healAmountHp = 15 + 2 * healMult;
            if (charToEffect.Hp + healAmountHp > charToEffect.TotalHp)
            {
               healAmountHp = charToEffect.TotalHp - charToEffect.Hp;
               charToEffect.Hp = charToEffect.TotalHp;
            }
            else
               charToEffect.Hp += healAmountHp;
            charToEffect.IsAlive = true;
            return healAmountHp;
         }
         //If char type is DRAGON
         else if (selectedCharType == (int)Character.CHAR_TYPES.DRAGON)
         {
            gameState.Characters[(int)Character.CHAR_TYPES.WARRIOR].Hp -= AttackPts;
            if (gameState.Characters[(int)Character.CHAR_TYPES.WARRIOR].Hp < 0)
               gameState.Characters[(int)Character.CHAR_TYPES.WARRIOR].Hp = 0;
            gameState.Characters[(int)Character.CHAR_TYPES.MAGE].Hp -= AttackPts;
            if (gameState.Characters[(int)Character.CHAR_TYPES.MAGE].Hp < 0)
               gameState.Characters[(int)Character.CHAR_TYPES.MAGE].Hp = 0;
            gameState.Characters[(int)Character.CHAR_TYPES.CLERIC].Hp -= AttackPts;
            if (gameState.Characters[(int)Character.CHAR_TYPES.CLERIC].Hp < 0)
               gameState.Characters[(int)Character.CHAR_TYPES.CLERIC].Hp = 0;
            return 3 * AttackPts;
         }
         //Otherwise damage the character to efect
         else
         {
            if (charToEffect.Hp - 3 * AttackPts / defense > 0)
            {
               charToEffect.Hp -= 3 * AttackPts / defense;
               return 3 * AttackPts / defense;
            }
            else
            {
               int lostHp = charToEffect.Hp;
               charToEffect.Hp = 0;
               charToEffect.IsAlive = false;
               return lostHp;
            }
         }
      }

      public string GetCharInfoText()
      {
         return $"{Name}\n:HP: {Hp}/{TotalHp}\nSP: {Sp}/{TotalSp}";
      }
    }
}