namespace GameOne.Source.SaveAndLoad
{
    using System;

    /// <summary>
    /// Container for your save game data.
    /// Put the variables you need here, as long as it's serializable.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public int testInt;
        public bool testBool;
        public string testString;
    }
}