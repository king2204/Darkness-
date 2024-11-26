//This is the main game form where the game battle interactions take place. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turn_Based_Game
{
   public partial class Turn_Based_GameForm : Form
   {
      GameState gameState;
      public Turn_Based_GameForm()
      {
         InitializeComponent();
         InitGame();
      }

      //Setup game state
      private void InitGame()
      {
         gameState = new GameState();
         InitCharImages();
         UpdateSelectedHeroLabel();
         UpdateCharInfo();
         battleLogBox.TextChanged += new EventHandler(ScrollTextChange);
      }

      //Auto scroll to bottom of text box
      public static void ScrollTextChange(object sender, EventArgs e)
      {
         ((RichTextBox)sender).SelectionStart = ((RichTextBox)sender).Text.Length;
         // scroll it automatically
         ((RichTextBox)sender).ScrollToCaret();
      }

      //Updtae the name of the current hero for the selected hero label
      private void UpdateSelectedHeroLabel()
      {
         //Find the hero name that maps to the selectedChar field in gameState and update the selected hero label
         string heroName = "UNKNOWN";
         foreach (KeyValuePair<int, string> mapping in Character.CHAR_NAMES)
            if (gameState.SelectedChar == mapping.Key)
               heroName = mapping.Value;
         selectedHeroLabel.Text = $"Hero: {heroName}";
      }

      //display the character images in the game form
      private void InitCharImages()
      {
         var bmp = new Bitmap(Properties.Resources.warrior);
         hero1ImgPan.BackgroundImage = bmp;
         bmp = new Bitmap(Properties.Resources.MAGE);
         hero2ImgPan.BackgroundImage = bmp;
         bmp = new Bitmap(Properties.Resources.CLERIC);
         hero3ImgPan.BackgroundImage = bmp;
         bmp = new Bitmap(Properties.Resources.BANDIT);
         enemy1ImgPan.BackgroundImage = bmp;
         bmp = new Bitmap(Properties.Resources.OGRE);
         enemy2ImgPan.BackgroundImage = bmp;
         bmp = new Bitmap(Properties.Resources.DRAGON);
         enemy3ImgPan.BackgroundImage = bmp;
      }

      //Display the tareget buttons
      private void ShowTargetButs()
      {
         chooseTargetLab.Visible = true;
         target1Butt.Visible = true;
         target2But.Visible = true;
         target3But.Visible = true;
      }

      //Make the target buttons is the game form invisibleffg
      private void HideTargetButs()
      {
         chooseTargetLab.Visible = false;
         target1Butt.Visible = false;
         target2But.Visible = false;
         target3But.Visible = false;
      }

      //Change selectedMove in gameState, display the target buttons and display the ENEMY names in the target buttons
      private void attackButton_Click(object sender, EventArgs e)
      {
         gameState.SelectedMove = (int)GameState.MOVES.ATT;
         UpdateTargetButText();
         ShowTargetButs();
         //Update target buttons to enemy names
         target1Butt.Text = Character.CHAR_NAMES[(int)Character.CHAR_TYPES.BANDIT];
         target2But.Text = Character.CHAR_NAMES[(int)Character.CHAR_TYPES.OGRE];
         target3But.Text = Character.CHAR_NAMES[(int)Character.CHAR_TYPES.DRAGON];
      }

      //Update sekectedMove in gameState and hide target buttons. They are not utilized for a defense move
      private void defendButton_Click(object sender, EventArgs e)
      {
         gameState.SelectedMove = (int)GameState.MOVES.DEF;
         HideTargetButs();
      }

      //Update the character names that are displayed on the target buttons
      private void UpdateTargetButText()
      {
         if (gameState.SelectedChar == (int)Character.CHAR_TYPES.CLERIC &&
            gameState.SelectedMove == (int)GameState.MOVES.SPEC)
         {
            //Update target buttons to hero names
            target1Butt.Text = Character.CHAR_NAMES[(int)Character.CHAR_TYPES.WARRIOR];
            target2But.Text = Character.CHAR_NAMES[(int)Character.CHAR_TYPES.MAGE];
            target3But.Text = Character.CHAR_NAMES[(int)Character.CHAR_TYPES.CLERIC];
         }
         else
         {
            //Update target buttons to enemy names
            target1Butt.Text = Character.CHAR_NAMES[(int)Character.CHAR_TYPES.BANDIT];
            target2But.Text = Character.CHAR_NAMES[(int)Character.CHAR_TYPES.OGRE];
            target3But.Text = Character.CHAR_NAMES[(int)Character.CHAR_TYPES.DRAGON];
         }
      }

      //When SPECIAL button is clicked update selectedMove in game state, and show apropriate target buttons
      private void specialButton_Click(object sender, EventArgs e)
      {
         gameState.SelectedMove = (int)GameState.MOVES.SPEC;
         UpdateTargetButText();
         ShowTargetButs();
      }

      private void ExecuteEnemyMove()
      {
         //if selectedChar is not alive do not alllow an enemy to attack
         if (!gameState.Characters[gameState.SelectedChar].IsAlive)
         {
            //Get ready for next character in battle
            NextCharInBattle();
            return;
         }

         //Like the okBut logic but for enemies
         bool isValidMove = false;
         //Use random to determine enemy attacks
         Random rand = new Random();

         //Choose a random selection until a valid option is found
         while (!isValidMove)
         {
            //Choose random move and random target
            int enemyMove = rand.Next(0, 3);
            int charToAttack = rand.Next(0, 3);
            gameState.SelectedTarget = charToAttack;
            string selectedName = gameState.Characters[gameState.SelectedChar].Name;
            string targetName = gameState.Characters[gameState.SelectedTarget].Name;
            switch (enemyMove)
            {
               case (int)GameState.MOVES.ATT:
                  if (gameState.Characters[charToAttack].IsAlive)
                  {
                     //Use attack on target
                     int damageTaken = gameState.Characters[gameState.SelectedChar]
                        .Attack(gameState.Characters[charToAttack]);
                     isValidMove = true;
                     //Add attack info to battle log
                     battleLogBox.AppendText(
                        $"{selectedName} attacked {targetName} for {damageTaken} damage\n");
                  }
                  break;
               case (int)GameState.MOVES.DEF:
                  gameState.Characters[gameState.SelectedChar].Defend();
                  isValidMove = true;
                  //Add defend info to battle log
                  battleLogBox.AppendText(
                     $"{selectedName} defended\n");
                  break;
               case (int)GameState.MOVES.SPEC:
                  //If the char to attack is alive, and the selected character has SP
                  if (gameState.Characters[charToAttack].IsAlive
                     && gameState.Characters[gameState.SelectedChar].Sp > 0)
                  {
                     //Use special attack on target
                     int damageTaken = gameState.Characters[gameState.SelectedChar]
                        .SpecialMove(gameState.Characters[charToAttack],
                        gameState.Characters[gameState.SelectedChar].CharType, gameState);
                     isValidMove = true;
                     if (gameState.Characters[gameState.SelectedChar].CharType == (int)Character.CHAR_TYPES.DRAGON)
                        battleLogBox.AppendText(
                           $"DRAGON attacked all heroes for a total loss of {damageTaken}\n");
                     else
                        battleLogBox.AppendText(
                            $"{selectedName} used a special on {targetName} for {damageTaken} damage\n");
                  }
                  break;
            }
         }
         //Check for end game
         CheckFailOrNewLevel();

         if (gameState.GameOver)
            return;

         //Get ready for next character in battle
         NextCharInBattle();

      }

      //When OK button is clicked, execute the selected characters move on the selected target
      // Update next character in the battle sequence, and check if the next character is a
      // NPC that requires a random move generation
      private void okBut_Click(object sender, EventArgs e)
      {
         //Prompt user that no target selected, exit function if no target selected
         if (gameState.SelectedTarget == GameState.NO_TARGET
            && gameState.SelectedMove != (int)GameState.MOVES.DEF)
         {
            battleLogBox.AppendText("No target selected. Try again.\n");
            return;
         }

         string selectedName = gameState.Characters[gameState.SelectedChar].Name;
         string targetName;
         if (gameState.SelectedTarget != GameState.NO_TARGET)
            targetName = gameState.Characters[gameState.SelectedTarget].Name;
         else
            targetName = "UNKNOWN";

         switch (gameState.SelectedMove)
         {
            case (int)GameState.MOVES.ATT:
               if (gameState.Characters[gameState.SelectedTarget].IsAlive)
               {
                  //Use the selected character to perform an attack on the target
                  int damageTaken = gameState.Characters[gameState.SelectedChar]
                  .Attack(gameState.Characters[gameState.SelectedTarget]);
                  //Add attack info to battle log
                  battleLogBox.AppendText(
                     $"{selectedName} attacked {targetName} for {damageTaken} damage\n");
               }
               else
                  battleLogBox.AppendText("Attack missed, target is not alive.\n");
               break;

            case (int)GameState.MOVES.DEF:
               //Use the selected character to perform a special move on the target
               gameState.Characters[gameState.SelectedChar]
                  .Defend();
               //Add defend info to battle log
               battleLogBox.AppendText(
                  $"{selectedName} defended\n");
               break;

            case (int)GameState.MOVES.SPEC:
               if (gameState.Characters[gameState.SelectedTarget].IsAlive
                     && gameState.Characters[gameState.SelectedChar].Sp > 0)
               {
                  //Use the selected character to perform a special move on the target
                  int hpPoints = gameState.Characters[gameState.SelectedChar]
                  .SpecialMove(gameState.Characters[gameState.SelectedTarget], gameState.SelectedChar, gameState);
                  //Add special move info to battle log
                  if (gameState.SelectedChar == (int)Character.CHAR_TYPES.CLERIC)
                     battleLogBox.AppendText(
                        $"{selectedName} healed {hpPoints} HP to {targetName}\n");
                  else
                     battleLogBox.AppendText(
                        $"{selectedName} used a special on {targetName} for {hpPoints} damage\n");
               }
               else
               {
                  battleLogBox.AppendText("Special move missed, target is not alive.\n");
               }
               break;
            case (int)GameState.MOVES.NO_SELECT:
               break;
         }
         //Update game for next chars move in battle sequence
         NextCharInBattle();
         //Check for end game
         CheckFailOrNewLevel();
      }

      //Update game for next battle turn
      private void NextCharInBattle()
      {
         //Unselect a target at the end of a turn
         gameState.SelectedMove = (int)GameState.MOVES.NO_SELECT;
         //Update on screen character info
         UpdateCharInfo();
         //Go to the next character in the battle sequence
         gameState.UpdateSelectedChar();

         UpdateSelectedHeroLabel();
         //If the next character is an enemy execute a random battle move automatically
         if (!gameState.Characters[gameState.SelectedChar].IsHero)
            ExecuteEnemyMove();
      }

      //Update the selectedTarget in gameState when a target button is clicked
      private void target1Butt_Click(object sender, EventArgs e)
      {
         UpdateSelectedTarget(sender, e);
      }

      private void target2But_Click(object sender, EventArgs e)
      {
         UpdateSelectedTarget(sender, e);
      }

      private void target3But_Click(object sender, EventArgs e)
      {
         UpdateSelectedTarget(sender, e);
      }

      //Check for a game over or new level state
      private void CheckFailOrNewLevel()
      {
         int gameStatus = gameState.CheckEndGame();
         if (gameStatus == GameState.HEROES_DEFEATED)
         {
            gameOverLabel.Visible = true;
            gameState.GameOver = true;
            UpdateCharInfo();
         }
         if (gameStatus == GameState.ENEMIES_DEFEATED)
         {
            gameState.LevelNum++;
            levelLabel.Text = $"LEVEL {gameState.LevelNum}";
            gameState.SetupNewLevel();
            UpdateCharInfo();
         }
      }

      //Update the selected target in game state when a target button is clicked
      private void UpdateSelectedTarget(object sender, EventArgs e)
      {
         foreach (KeyValuePair<int, string> mapping in Character.CHAR_NAMES)
         {
            string buttonText = ((Button)sender).Text;
            if (((Button)sender).Text == mapping.Value)
            {
               gameState.SelectedTarget = mapping.Key;
            }
         }
      }

      //Update the character information displayed in the game form
      private void UpdateCharInfo()
      {
         hero1StatLabel.Text = gameState.Characters[(int)Character.CHAR_TYPES.WARRIOR].GetCharInfoText();
         if (!gameState.Characters[(int)Character.CHAR_TYPES.WARRIOR].IsAlive)
            hero1StatLabel.ForeColor = Color.Red;
         else
            hero1StatLabel.ForeColor = Color.Black;
         
         hero2StatLabel.Text = gameState.Characters[(int)Character.CHAR_TYPES.MAGE].GetCharInfoText();
         if (!gameState.Characters[(int)Character.CHAR_TYPES.MAGE].IsAlive)
            hero2StatLabel.ForeColor = Color.Red;
         else
            hero2StatLabel.ForeColor = Color.Black;

         hero3StatLabel.Text = gameState.Characters[(int)Character.CHAR_TYPES.CLERIC].GetCharInfoText();
         if (!gameState.Characters[(int)Character.CHAR_TYPES.CLERIC].IsAlive)
            hero3StatLabel.ForeColor = Color.Red;
         else
            hero3StatLabel.ForeColor = Color.Black;

         enemy1StatLabel.Text = gameState.Characters[(int)Character.CHAR_TYPES.BANDIT].GetCharInfoText();
         if (!gameState.Characters[(int)Character.CHAR_TYPES.BANDIT].IsAlive)
            enemy1StatLabel.ForeColor = Color.Red;
         else
            enemy1StatLabel.ForeColor = Color.Black;

         enemy2StatLabel.Text = gameState.Characters[(int)Character.CHAR_TYPES.OGRE].GetCharInfoText();
         if (!gameState.Characters[(int)Character.CHAR_TYPES.OGRE].IsAlive)
            enemy2StatLabel.ForeColor = Color.Red;
         else
            enemy2StatLabel.ForeColor = Color.Black;

         enemy3StatLabel.Text = gameState.Characters[(int)Character.CHAR_TYPES.DRAGON].GetCharInfoText();
         if (!gameState.Characters[(int)Character.CHAR_TYPES.DRAGON].IsAlive)
            enemy3StatLabel.ForeColor = Color.Red;
         else
            enemy3StatLabel.ForeColor = Color.Black;
      }

      //Reset games state and update on screen stats when new game is selected from menu strip
      private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
      {
         gameState.ResetState();
         battleLogBox.Text = "";
         levelLabel.Text = "LEVEL 1";
         UpdateCharInfo();
      }

      //Close app when exit is selected from memu strip
      private void exitToolStripMenuItem_Click(object sender, EventArgs e)
      {
         Application.Exit();
      }

      private void howToPlayToolStripMenuItem_Click(object sender, EventArgs e)
      {
         Form howTo = new Turn_Based_GameForm();
         howTo.Show();
      }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void DungeonBrawlForm_Load(object sender, EventArgs e)
        {

        }

        private void battleImagePanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
