public static class GameState

// Script by OlafRT
// checks for if we have the magazine open or not, since we can't set timescale to 0 (Freeze time) 
// while having this open because of its animations.

{
    public static bool IsMagazineOpen { get; private set; }

    public static void OpenMagazine() => IsMagazineOpen = true;
    public static void CloseMagazine() => IsMagazineOpen = false;
}

