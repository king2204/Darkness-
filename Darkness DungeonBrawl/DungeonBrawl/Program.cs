//Name Joshua Doucet
//Project DungeonBrawl
//Date: May 5n 2020
//Coure: CS 3020 C# and .NET Programming

//This is the entry point for the code execution

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turn_Based_Game
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new Turn_Based_GameForm());
      }
   }
}
