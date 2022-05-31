using System;
using System.Threading;

namespace SnekButItEatRam {
 internal static class Program {
  private const  int             GridW           = 90;
  private const  int             GridH           = 25;
  private static Cell[,]         memGrid         = new Cell[GridH, GridW];
  private static Cell            memCurrentCell;
  private static int             memDirection;   //0=Up 1=Right 2=Down 3=Left
  private const  int             Speed           = 1;
  private static bool            memPopulated;
  private static bool            memLost;
  private static int             memSnakeLength;
  private static int             memRamCount;
  private static bool            allowExit;
  private static readonly byte[][] mem             = new byte[1000000000][];

  private static void Main() {
   AppDomain.CurrentDomain.ProcessExit += OnExit; 
        
   while (true) {
    Console.Clear();
    try {
     StartGame();
    }
    catch (Exception) {
     // They died
     // Time to eat some RAM
     TakeMoreRam();
     memRamCount++;
    }

    // Reset for new game and hopefully more RAM to eat :)
    memGrid = new Cell[GridH, GridW];
    memPopulated = false;
    memLost = false;
               
    Console.WriteLine("Waiting...");
    Thread.Sleep(1000);
    Console.WriteLine("Done wait");
            
   }
        
  }

  // If they try to exit fill their RAM with more 0s
  private static void OnExit(object sender, EventArgs e) {
   if (allowExit) { return; }
   Console.Title = "How dare you try to leave!";
   Console.Clear();
   Console.WriteLine("I can't let you leave!");
   Console.WriteLine("Prepare to die");
        
   // Fill the mem variables with 0s
   for (int i = 0; i < mem.Length; i++) {
    Console.WriteLine("Filling index: " + i);
   }

  }

  private static void TakeMoreRam() {
   Console.Title = memRamCount.ToString();
   mem[memRamCount] = new byte[1000000000];
   // fill
   for (int j = 0; j < mem[memRamCount].Length; j++) {
    mem[memRamCount][j] = 1;
   }
   Console.WriteLine(GC.GetTotalMemory(true));
  }

  private static void StartGame() {
   if (!memPopulated) {
    memSnakeLength = 5;
    PopulateGrid();
    memCurrentCell = memGrid[(int) Math.Ceiling((double) GridH / 2), (int) Math.Ceiling((double) GridW / 2)];
    UpdatePos();
    AddFood();
    memPopulated = true;
   }

   while (!memLost) {
    Restart();
   }
  }

  private static void Restart() {
   Console.SetCursorPosition(0, 0);
   PrintGrid();
   Console.WriteLine("Length: {0}", memSnakeLength);
   GetInput();
  }

  private static void UpdateScreen() {
   Console.SetCursorPosition(0, 0);
   PrintGrid();
   Console.WriteLine("Length: {0}", memSnakeLength);
  }

  private static void GetInput() {

   Console.Write("Where to move? [WASD] ");
   while (!Console.KeyAvailable) {
    Move();
    UpdateScreen();
   }
   var input = Console.ReadKey();
   DoInput(input.KeyChar);
  }

  private static void CheckCell(Cell cell) {
   if (cell.Val == "%") {
    EatFood();
   }
   if (cell.Visited) {
    Lose();
   }
  }

  private static void Lose() {
   Console.WriteLine("\n You lose!");
   Thread.Sleep(1000);
   // Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
   // Environment.Exit(-1);
   throw new Exception("fail");
  }

  private static void DoInput(char inp) {
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

  private static void AddFood() {
   Random r = new Random();
   while (true) {
    Cell cell = memGrid[r.Next(memGrid.GetLength(0)), r.Next(memGrid.GetLength(1))];
    if (cell.Val == " ")
     cell.Val = "%";
    break;
   }
  }

  private static void EatFood() {
   memSnakeLength += 1;
   if (memSnakeLength == 10) {
    // win
    Console.Clear();
    Console.WriteLine("You win!");
    Thread.Sleep(5000);
    allowExit = true;
    Environment.Exit(0);
   }
   AddFood();
  }

  private static void GoUp() {
   if (memDirection == 2)
    return;
   memDirection = 0;
  }

  private static void GoRight() {
   if (memDirection == 3)
    return;
   memDirection = 1;
  }

  private static void GoDown() {
   if (memDirection == 0)
    return;
   memDirection = 2;
  }

  private static void GoLeft() {
   if (memDirection == 1)
    return;
   memDirection = 3;
  }

  private static void Move() {
   if (memDirection == 0) {
    //up
    if (memGrid[memCurrentCell.Y - 1, memCurrentCell.X].Val == "*") {
     Lose();
     return;
    }
    VisitCell(memGrid[memCurrentCell.Y - 1, memCurrentCell.X]);
   } else if (memDirection == 1) {
    //right
    if (memGrid[memCurrentCell.Y, memCurrentCell.X - 1].Val == "*") {
     Lose();
     return;
    }
    VisitCell(memGrid[memCurrentCell.Y, memCurrentCell.X - 1]);
   } else if (memDirection == 2) {
    //down
    if (memGrid[memCurrentCell.Y + 1, memCurrentCell.X].Val == "*") {
     Lose();
     return;
    }
    VisitCell(memGrid[memCurrentCell.Y + 1, memCurrentCell.X]);
   } else if (memDirection == 3) {
    //left
    if (memGrid[memCurrentCell.Y, memCurrentCell.X + 1].Val == "*") {
     Lose();
     return;
    }
    VisitCell(memGrid[memCurrentCell.Y, memCurrentCell.X + 1]);
   }
   Thread.Sleep(Speed * 100);
  }

  private static void VisitCell(Cell cell) {
   memCurrentCell.Val = "#";
   memCurrentCell.Visited = true;
   memCurrentCell.Decay = memSnakeLength;
   CheckCell(cell);
   memCurrentCell = cell;
   UpdatePos();

   //checkCell(currentCell);
  }

  private static void UpdatePos() {

   memCurrentCell.Set("@");
   memCurrentCell.Val = memDirection switch {
    0 => "^",
    1 => "<",
    2 => "v",
    3 => ">",
    _ => memCurrentCell.Val
   };

   memCurrentCell.Visited = false;
  }

  private static void PopulateGrid() {
   for (int col = 0; col < GridH; col++) {
    for (int row = 0; row < GridW; row++) {
     Cell cell = new Cell {
      X = row,
      Y = col,
      Visited = false
     };
     if (cell.X == 0 || cell.X > GridW - 2 || cell.Y == 0 || cell.Y > GridH - 2)
      cell.Set("*");
     else
      cell.Clear();
     memGrid[col, row] = cell;
    }
   }
  }

  private static void PrintGrid() {
   string toPrint = "";
   for (int col = 0; col < GridH; col++) {
    for (int row = 0; row < GridW; row++) {
     memGrid[col, row].DecaySnake();
     toPrint += memGrid[col, row].Val;

    }
    toPrint += "\n";
   }
   Console.WriteLine(toPrint);
  }

  private class Cell {
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