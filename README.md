Developer Coding Test – Minefield Game
======================================

- _This branch includes after-hours completion. See tag TwoHourMark for the 2 hour mark code._ 

### Test and Run

```
echo '
      Game as finished:
      '
      
git checkout main
dotnet test
dotnet run --project ./MineSweeperCli
#
echo '
      Progress up to the two-hour mark :
      '
git checkout TwoHourMark
dotnet test
dotnet run --project ./MineSweeperCli
#
```

### Known Specs. 

[X] = Completed by the two hour mark
[+] = Completed after two hour mark

- [X] player has lives
- [X] There is a (by default 8x8) board
- [X] Output:
- [X] player current position eg C2 in chess notation
- [X] players lives left
- [X] players moves so far
- [X] There must be some mines
- [X] Input can move the player
- [X] You can't move off the board
- [+] A player loses a life by hitting a minefield
- [+] Move off top of board = win
- [+] Final score= number of moves to reach other side

### Two Hour Mark

The 2 hours coding mark is tag TwoHourMark

### History

* ed2b6f7 - (HEAD -> main, tag: TwoHourMark, origin/main) wire up console input and output (2 minutes ago) <Chris F Carroll>
* 009a0b4 - test 6 WhenInittingMineSweeperCli.ThereShouldBeMinesPlaced (12 minutes ago) <Chris F Carroll>
* a92cb6c - test 5 - (35 minutes ago) <Chris F Carroll>
* 8e4eb91 - test 4 - output shows lives left (66 minutes ago) <Chris F Carroll>
* d2ebb80 - test 3 - output format is chessboard notation (69 minutes ago) <Chris F Carroll>
* 9681522 - test 2 - validation for settings (77 minutes ago) <Chris F Carroll>
* 7a5f732 - first functional test - returns a status line current position and lives left (2 hours ago) <Chris F Carroll>
* 26bea9a - README.md. Specs 1 and 2: a player has lives and there is a board default size 8 (2 hours ago) <Chris F Carroll>
* 9b77ab1 - add README.md (2 hours ago) <Chris F Carroll>
* b507179 - Use https://www.nuget.org/packages/Consoleable/ template (2 hours ago) <Chris F Carroll>
* df95b56 - Start at 21:08 (2 hours ago) <Chris F Carroll>


    
# Guessed Specs?

- Output at each step should tell player how many mines they are adjacent to


The Test
----------

**Set aside 2 hours to create some code that shows how you would code a minefield/minesweeper style game running on the command line (no UI), in order to demonstrate how you would code & test a real-world application using established best practices**.

In the game a player navigates from one side of a chessboard grid to the other whilst trying to avoid hidden mines. The player has a number of lives, losing one each time a mine is hit, and the final score is the number of moves taken in order to reach the other side of the board. The command line / console interface should be simple, allowing the player to input move direction (up, down, left, right) and the game to show the resulting position (e.g. C2 in chess board terminology) along with number of lives left and number of moves taken.



**Above all else please follow these guidelines**

**1. Quality is more important than quantity**

**2. We will assess your ability to write clean-code that has good structure & is covered by meaningful tests**

**3. Don’t code a UI**

When complete, upload your code to a public GitHub repository and forward the URL to us.

Be prepared to talk through you code and explain key design features and coding principles and why you have used them.

Good luck!
