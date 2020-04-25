namespace Models.WorldGen
{
    public static class Constants
    {
        public const int WORLD_HEIGHT = 128;
        public const int WORLD_WIDTH = 128;
        public const int WORLD_TILES_COUNT = WORLD_HEIGHT * WORLD_WIDTH;

        public const int REGION_HEIGHT = 256;
        public const int REGION_WIDTH = 256;
        public const int REGION_DEPTH = 128;
        public const int REGION_TILES_COUNT = REGION_DEPTH * REGION_HEIGHT * REGION_WIDTH;
    }

    public enum TileFlag
    {
		CAN_GO_UP = 1,
		CAN_GO_DOWN = 2,
		CAN_GO_NORTH = 4,
		CAN_GO_EAST = 8,
		CAN_GO_SOUTH = 16,
		CAN_GO_WEST = 32,
		CAN_STAND_HERE = 64,
		CONSTRUCTION = 128,
		REVEALED = 256,
		VISIBLE = 512,
		SOLID = 1024,
		ABOVE_GROUND = 2048,
		BLOODSTAIN = 4096,
		CAN_GO_NORTH_WEST = 8192,
		CAN_GO_NORTH_EAST = 16384,
		CAN_GO_SOUTH_WEST = 32768,
		CAN_GO_SOUTH_EAST = 65536,
		OPAQUE_TILE = 131072,
    }

	public enum TileType
	{
		SEMI_MOLTEN_ROCK = 0,
		SOLID = 1,
		OPEN_SPACE = 2,
		WALL = 3,
		RAMP = 4,
		STAIRS_UP = 5,
		STAIRS_DOWN = 6,
		STAIRS_UPDOWN = 7,
		FLOOR = 8,
		TREE_TRUNK = 9,
		TREE_LEAF = 10,
		WINDOW = 11,
		CLOSED_DOOR = 12
	}
}
