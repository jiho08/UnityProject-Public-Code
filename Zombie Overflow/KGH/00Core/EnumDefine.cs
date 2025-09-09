namespace Core.Define
{
    public static partial class EnumDefine
    {
        public enum StateType
        {
            Top,
            Bottom,
        }
        
        public enum PlayerType
        {
            Starter = 0,
            Revolver = 1,
            Shotgun = 2,
            Sniper = 3,
        }
        public enum PlayerSoundType
        {
            Shoot,
            Reload,
            Empty,
            Dead,
        }

        public enum MapType
        {
            None = -1,
            City,
            Park,
            Restaurant,
        }
    }
}