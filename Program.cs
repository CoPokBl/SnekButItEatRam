using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SnekButItEatRam {
    class Program {
     
     
     static readonly int GridW = 90;
        static readonly int GridH = 25;
        static Cell[, ] _grid = new Cell[GridH, GridW];
        static Cell _currentCell;
        static Cell _food;
        static int _foodCount;
        static int _direction; //0=Up 1=Right 2=Down 3=Left
        static readonly int Speed = 1;
        static bool _populated = false;
        static bool _lost = false;
        static int _snakeLength;
        private static int RamCount;
        private static List<string> RamTakerUpper = new List<string>();
        static MemoryStream inMemoryCopy = new MemoryStream();
        private static byte[] mem = new byte[2000000000];
        private static byte[] mem2 = new byte[2000000000];
        private static byte[] mem3 = new byte[2000000000];
        private static byte[] mem4 = new byte[2000000000];
        private static byte[] mem5 = new byte[2000000000];
        private static byte[] mem6 = new byte[2000000000];
        private static byte[] mem7 = new byte[2000000000];
        private static byte[] mem8 = new byte[2000000000];
        private static byte[] mem9 = new byte[2000000000];
        private static byte[] mem10 = new byte[2000000000];
        private static byte[] mem11 = new byte[2000000000];
        private static byte[] mem12 = new byte[2000000000];
        private static byte[] mem13 = new byte[2000000000];
        private static byte[] mem14 = new byte[2000000000];
        private static byte[] mem15 = new byte[2000000000];
        private static byte[] mem16 = new byte[2000000000];

        static void Main(string[] args) {
         // for (int i = 0; i < mem.Length; i++) {
         //  mem[i] = 0;
         // }
         while (true) {
          Console.Clear();
          try {
           StartGame();
          }
          catch (Exception) {
           // they failed
           TAKEMORERAM();
           RamCount++;
          }

          // reset
          _grid = new Cell[GridH, GridW];
          _foodCount = 0;
          _populated = false;
          _lost = false;
          
          Console.WriteLine("Waiting...");
          Thread.Sleep(1000);
          Console.WriteLine("Done wait");
          
         }
        }

        static void TAKEMORERAM() {
         Console.Title = RamCount.ToString();
         switch (RamCount) {
           case 0:
            for (int i = 0; i < mem2.Length/2; i++) { mem2[i] = 0; }
            break;
           case 1:
            for (int i = 0; i < mem3.Length/2; i++) { mem3[i] = 0; }
            break;
           case 2:
            for (int i = 0; i < mem4.Length/2; i++) { mem4[i] = 0; }
            break;
           case 3:
            for (int i = 0; i < mem5.Length/2; i++) { mem5[i] = 0; }
            break;
           case 4:
            for (int i = 0; i < mem6.Length; i++) { mem6[i] = 0; }
            break;
           case 5:
            for (int i = 0; i < mem7.Length; i++) { mem7[i] = 0; }
            break;
           case 6:
            for (int i = 0; i < mem8.Length; i++) { mem8[i] = 0; }
            break;
           case 7:
            for (int i = 0; i < mem9.Length; i++) { mem9[i] = 0; }
            break;
           case 8:
            for (int i = 0; i < mem10.Length; i++) { mem10[i] = 0; }
            break;
           case 9:
            for (int i = 0; i < mem11.Length; i++) { mem11[i] = 0; }
            break;
           case 10:
            for (int i = 0; i < mem12.Length; i++) { mem12[i] = 0; }
            break;
           case 11:
            for (int i = 0; i < mem13.Length; i++) { mem13[i] = 0; }
            break;
           case 12:
            for (int i = 0; i < mem14.Length; i++) { mem14[i] = 0; }
            break;
           case 13:
            for (int i = 0; i < mem15.Length; i++) { mem15[i] = 0; }
            break;
           case 14:
            for (int i = 0; i < mem16.Length; i++) { mem16[i] = 0; }
            break;
         }
         Console.WriteLine(GC.GetTotalMemory(true));
        }

        static void StartGame() {
         if (!_populated) {
          _foodCount = 0;
          _snakeLength = 5;
          PopulateGrid();
          _currentCell = _grid[(int) Math.Ceiling((double) GridH / 2), (int) Math.Ceiling((double) GridW / 2)];
          UpdatePos();
          AddFood();
          _populated = true;
         }

         while (!_lost) {
          Restart(); 
         }
        }

        static void Restart() {
         Console.SetCursorPosition(0, 0);
         PrintGrid();
         Console.WriteLine("Length: {0}", _snakeLength);
         GetInput();
        }

        static void UpdateScreen() {
         Console.SetCursorPosition(0, 0);
         PrintGrid();
         Console.WriteLine("Length: {0}", _snakeLength);
        }

        static void GetInput() {

         //Console.Write("Where to move? [WASD] ");
         ConsoleKeyInfo input;
         while (!Console.KeyAvailable) {
          Move();
          UpdateScreen();
         }
         input = Console.ReadKey();
         DoInput(input.KeyChar);
        }

        static void CheckCell(Cell cell) {
         if (cell.Val == "%") {
          EatFood();
         }
         if (cell.Visited) {
          Lose();
         }
        }

        static void Lose() {
         Console.WriteLine("\n You lose!");
         Thread.Sleep(1000);
         // Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
         // Environment.Exit(-1);
         throw new Exception("fail");
        }

        static void DoInput(char inp) {
         switch (inp) {
          case 'w':
           GoUp();
           break;
          case 's':
           GoDown();
           break;
          case 'a':
           GoRight();
           break;
          case 'd':
           GoLeft();
           break;
         }
        }

        static void AddFood() {
         Random r = new Random();
         Cell cell;
         while (true) {
          cell = _grid[r.Next(_grid.GetLength(0)), r.Next(_grid.GetLength(1))];
          if (cell.Val == " ")
           cell.Val = "%";
          break;
         }
        }

        static void EatFood() {
         _snakeLength += 1;
         AddFood();
        }

        static void GoUp() {
         if (_direction == 2)
          return;
         _direction = 0;
        }

        static void GoRight() {
         if (_direction == 3)
          return;
         _direction = 1;
        }

        static void GoDown() {
         if (_direction == 0)
          return;
         _direction = 2;
        }

        static void GoLeft() {
         if (_direction == 1)
          return;
         _direction = 3;
        }

        static void Move() {
         if (_direction == 0) {
          //up
          if (_grid[_currentCell.Y - 1, _currentCell.X].Val == "*") {
           Lose();
           return;
          }
          VisitCell(_grid[_currentCell.Y - 1, _currentCell.X]);
         } else if (_direction == 1) {
          //right
          if (_grid[_currentCell.Y, _currentCell.X - 1].Val == "*") {
           Lose();
           return;
          }
          VisitCell(_grid[_currentCell.Y, _currentCell.X - 1]);
         } else if (_direction == 2) {
          //down
          if (_grid[_currentCell.Y + 1, _currentCell.X].Val == "*") {
           Lose();
           return;
          }
          VisitCell(_grid[_currentCell.Y + 1, _currentCell.X]);
         } else if (_direction == 3) {
          //left
          if (_grid[_currentCell.Y, _currentCell.X + 1].Val == "*") {
           Lose();
           return;
          }
          VisitCell(_grid[_currentCell.Y, _currentCell.X + 1]);
         }
         Thread.Sleep(Speed * 100);
        }

        static void VisitCell(Cell cell) {
         _currentCell.Val = "#";
         _currentCell.Visited = true;
         _currentCell.Decay = _snakeLength;
         CheckCell(cell);
         _currentCell = cell;
         UpdatePos();

         //checkCell(currentCell);
        }

        static void UpdatePos() {

         _currentCell.Set("@");
         if (_direction == 0) {
          _currentCell.Val = "^";
         } else if (_direction == 1) {
          _currentCell.Val = "<";
         } else if (_direction == 2) {
          _currentCell.Val = "v";
         } else if (_direction == 3) {
          _currentCell.Val = ">";
         }

         _currentCell.Visited = false;
         return;
        }

        static void PopulateGrid() {
         Random random = new Random();
         for (int col = 0; col < GridH; col++) {
          for (int row = 0; row < GridW; row++) {
           Cell cell = new Cell();
           cell.X = row;
           cell.Y = col;
           cell.Visited = false;
           if (cell.X == 0 || cell.X > GridW - 2 || cell.Y == 0 || cell.Y > GridH - 2)
            cell.Set("*");
           else
            cell.Clear();
           _grid[col, row] = cell;
          }
         }
        }

        static void PrintGrid() {
         string toPrint = "";
         for (int col = 0; col < GridH; col++) {
          for (int row = 0; row < GridW; row++) {
           _grid[col, row].DecaySnake();
           toPrint += _grid[col, row].Val;

          }
          toPrint += "\n";
         }
         Console.WriteLine(toPrint);
        }
        public class Cell {
         public string Val {
          get;
          set;
         }
         public int X {
          get;
          set;
         }
         public int Y {
          get;
          set;
         }
         public bool Visited {
          get;
          set;
         }
         public int Decay {
          get;
          set;
         }

         public void DecaySnake() {
          Decay -= 1;
          if (Decay == 0) {
           Visited = false;
           Val = " ";
          }
         }

         public void Clear() {
          Val = " ";
         }

         public void Set(string newVal) {
          Val = newVal;
         }
        }
        
    }
}