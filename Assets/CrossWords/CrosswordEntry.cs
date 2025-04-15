using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Across, Down }

[System.Serializable]
public class CrosswordEntry
{
    public int ClueNumber;     // Number to display in the corner
    public string Word;
    public int Row;
    public int Col;
    public Direction Direction;
    public string Clue;
}

// This class holds the list of entries
public static class CrosswordData
{
    public static List<CrosswordEntry> Entries = new List<CrosswordEntry>
    {
        new CrosswordEntry { ClueNumber = 1, Word = "VALKYRIE", Row = 1, Col = 9, Direction = Direction.Down, Clue = "YOUR FAVOURITE AIRLINE" },
        new CrosswordEntry { ClueNumber = 2, Word = "HEAVEN", Row = 1, Col = 14, Direction = Direction.Down, Clue = "Ultimate place of peace" },
        new CrosswordEntry { ClueNumber = 3, Word = "JUDGEMENT", Row = 2, Col = 4, Direction = Direction.Down, Clue = "Moment of cosmic decision" },
        new CrosswordEntry { ClueNumber = 4, Word = "PASSENGER", Row = 2, Col = 11, Direction = Direction.Down, Clue = "You're one… but this isn’t a normal flight" },
        new CrosswordEntry { ClueNumber = 5, Word = "VALHALLA", Row = 3, Col = 7, Direction = Direction.Across, Clue = "Warrior's paradise in Norse mythology" },
        new CrosswordEntry { ClueNumber = 6, Word = "PURGATORY", Row = 5, Col = 1, Direction = Direction.Across, Clue = "A place of temporary punishment" },
        new CrosswordEntry { ClueNumber = 7, Word = "LIMBO", Row = 7, Col = 2, Direction = Direction.Across, Clue = "Suspended in a space between heaven and hell" },
        new CrosswordEntry { ClueNumber = 8, Word = "KARMA", Row = 8, Col = 14, Direction = Direction.Down, Clue = "Determines your final destination" }, 
        new CrosswordEntry { ClueNumber = 9, Word = "HELL", Row = 8, Col = 16, Direction = Direction.Down, Clue = "A place of eternal punishment" },
        new CrosswordEntry { ClueNumber = 10, Word = "DECEASED", Row = 9, Col = 10, Direction = Direction.Across, Clue = "You are this… even if you don’t know it" }
    };

    public static int GridRows => 14;
    public static int GridCols => 19;
}





