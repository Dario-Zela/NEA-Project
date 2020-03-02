namespace Models.WorldGen
{
    static class Constants
    {
        public const int WORLD_HEIGHT = 128;
        public const int WORLD_WIDTH = 128;
        public const int WORLD_TILES_COUNT = WORLD_HEIGHT * WORLD_WIDTH;

        public const int REGION_HEIGHT = 256;
        public const int REGION_WIDTH = 256;
        public const int REGION_DEPTH = 128;
        public const int REGION_TILES_COUNT = REGION_DEPTH * REGION_HEIGHT * REGION_WIDTH;
    }
}
