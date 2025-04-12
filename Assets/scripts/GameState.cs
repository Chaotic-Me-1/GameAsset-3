public static class GameState
{
    public static bool IsMagazineOpen { get; private set; }

    public static void OpenMagazine() => IsMagazineOpen = true;
    public static void CloseMagazine() => IsMagazineOpen = false;
}

