using System;
using System.Collections.Generic;

namespace Models.WorldGen
{
    public class Rectangle
    {
        public int x, y, width, height;
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }

    public class Leaf
    {
        private const int MIN_LEAF_SIZE = 20;

        public int x, y, width, height;
        public Leaf leftChild, rightChild;
        public Rectangle room;
        public List<Rectangle> halls;
        public Random rng;

        public Leaf(int X, int Y, int Width, int Height, Random rng)
        {
            x = X;
            y = Y;
            width = Width;
            height = Height;
            this.rng = rng;
        }

        public bool split()
        {
            if (leftChild != null || rightChild != null)
                return false;

            bool splitH = rng.NextDouble() > 0.5;
            if (width > height && width / height >= 1.25)
                splitH = false;
            else if (height > width && height / width >= 1.25)
                splitH = true;

            int max = (splitH ? height : width) - MIN_LEAF_SIZE;
            if (max <= MIN_LEAF_SIZE)
                return false;

            int split = rng.Next(MIN_LEAF_SIZE, max);

            if (splitH)
            {
                leftChild = new Leaf(x, y, width, split, rng);
                rightChild = new Leaf(x, y + split, width, height - split, rng);
            }
            else
            {
                leftChild = new Leaf(x, y, split, height, rng);
                rightChild = new Leaf(x + split, y, width - split, height, rng);
            }
            return true;
        }

        public Rectangle getRoom()
        {
            if (room != null)
                return room;
            else
            {
                Rectangle lRoom = null;
                Rectangle rRoom = null;
                if (leftChild != null)
                {
                    lRoom = leftChild.getRoom();
                }
                if (rightChild != null)
                {
                    rRoom = rightChild.getRoom();
                }
                if (lRoom == null && rRoom == null)
                    return null;
                else if (rRoom == null)
                    return lRoom;
                else if (lRoom == null)
                    return rRoom;
                else if (rng.NextDouble() > 0.5)
                    return lRoom;
                else
                    return rRoom;
            }
        }

        public void createRoom()
        {
            if (leftChild != null || rightChild != null)
            {
                // this leaf has been split, so go into the children leafs
                if (leftChild != null)
                {
                    leftChild.createRoom();
                }
                if (rightChild != null)
                {
                    rightChild.createRoom();
                }

                // if there are both left and right children in this Leaf, create a hallway between them
                if (leftChild != null && rightChild != null)
                {
                    createHall(leftChild.getRoom(), rightChild.getRoom());
                }

            }
            else
            {
                int roomWidth = rng.Next(width / 2, width - 2);
                int roomHeight = rng.Next(height / 2, height - 2);
                int roomX = rng.Next(x + 1, x + width - roomWidth - 1);
                int roomY = rng.Next(y + 1, y + height - roomHeight - 1);
                room = new Rectangle(roomX, roomY, roomWidth, roomHeight);
            }
        }

        public void createHall(Rectangle l, Rectangle r)
        {
            halls = new List<Rectangle>();

            var point1 = new Position(rng.Next(l.x + 1, l.x + l.width - 2), rng.Next(l.y + 1, l.y + l.height - 2));
            var point2 = new Position(rng.Next(r.x + 1, r.x + r.width - 2), rng.Next(r.y + 1, r.y + r.height - 2));

            var w = point2.x - point1.x;
            var h = point2.y - point1.y;

            if (w < 0)
            {
                if (h < 0)
                {
                    if (rng.NextDouble() < 0.5)
                    {
                        halls.Add(new Rectangle(point2.x, point1.y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point2.x, point2.y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        halls.Add(new Rectangle(point2.x, point2.y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point1.x, point2.y, 1, Math.Abs(h)));
                    }
                }
                else if (h > 0)
                {
                    if (rng.NextDouble() < 0.5)
                    {
                        halls.Add(new Rectangle(point2.x, point1.y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point2.x, point1.y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        halls.Add(new Rectangle(point2.x, point2.y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point1.x, point1.y, 1, Math.Abs(h)));
                    }
                }
                else // if (h == 0)
                {
                    halls.Add(new Rectangle(point2.x, point2.y, Math.Abs(w), 1));
                }
            }
            else if (w > 0)
            {
                if (h < 0)
                {
                    if (rng.NextDouble() < 0.5)
                    {
                        halls.Add(new Rectangle(point1.x, point2.y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point1.x, point2.y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        halls.Add(new Rectangle(point1.x, point1.y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point2.x, point2.y, 1, Math.Abs(h)));
                    }
                }
                else if (h > 0)
                {
                    if (rng.NextDouble() < 0.5)
                    {
                        halls.Add(new Rectangle(point1.x, point1.y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point2.x, point1.y, 1, Math.Abs(h)));
                    }
                    else
                    {
                        halls.Add(new Rectangle(point1.x, point2.y, Math.Abs(w), 1));
                        halls.Add(new Rectangle(point1.x, point1.y, 1, Math.Abs(h)));
                    }
                }
                else // if (h == 0)
                {
                    halls.Add(new Rectangle(point1.x, point1.y, Math.Abs(w), 1));
                }
            }
            else // if (w == 0)
            {
                if (h < 0)
                {
                    halls.Add(new Rectangle(point2.x, point2.y, 1, Math.Abs(h)));
                }
                else if (h > 0)
                {
                    halls.Add(new Rectangle(point1.x, point1.y, 1, Math.Abs(h)));
                }
            }
        }
    }

    public class Region
    {
        int MAX_LEAF_SIZE;
        public List<Leaf> leafs;
        List<Leaf> rooms
        {
            get
            {
                List<Leaf> leaves = new List<Leaf>();
                foreach (var leaf in leafs)
                {
                    if (leaf.leftChild == null && leaf.rightChild == null)
                    {
                        leaves.Add(leaf);
                    }
                }
                return leaves;
            }
        }
        int mapWidth, mapHeight;
        Random rng;

        public Region(int maxSize, int mapWidth, int mapHeight, int seed)
        {
            MAX_LEAF_SIZE = maxSize;
            this.mapHeight = mapHeight;
            this.mapWidth = mapWidth;
            rng = new Random(seed);
            leafs = new List<Leaf>();
            Leaf l = new Leaf(0, 0, mapWidth, mapHeight, rng);
            leafs.Add(l);
        }

        public void RunRooms()
        {
            leafs = new List<Leaf>();
            Leaf l = new Leaf(0, 0, mapWidth, mapHeight, rng);
            leafs.Add(l);
            bool didSplit = true;
            while (didSplit)
            {
                didSplit = false;
                List<Leaf> leafsCopy = new List<Leaf>(leafs);
                foreach (var leaf in leafs)
                {
                    if (leaf.leftChild == null && leaf.rightChild == null)
                    {
                        if (leaf.width > MAX_LEAF_SIZE || leaf.height > MAX_LEAF_SIZE || new Random().NextDouble() > 0.25)
                        {
                            if (leaf.split())
                            {
                                leafsCopy.Add(leaf.leftChild);
                                leafsCopy.Add(leaf.rightChild);
                                didSplit = true;
                            }
                        }
                    }
                }
                leafs = leafsCopy;
            }
            l.createRoom();
        }

        public void RunRooms(int minNumRooms)
        {
            leafs = new List<Leaf>();
            Leaf l = new Leaf(0, 0, mapWidth, mapHeight, rng);
            leafs.Add(l);
            while (rooms.Count < minNumRooms)
            {
                List<Leaf> leafsCopy = new List<Leaf>(leafs);
                foreach (var leaf in leafs)
                {
                    if (leaf.leftChild == null && leaf.rightChild == null)
                    {
                        if (leaf.width > MAX_LEAF_SIZE || leaf.height > MAX_LEAF_SIZE || new Random().NextDouble() > 0.25)
                        {
                            if (leaf.split())
                            {
                                leafsCopy.Add(leaf.leftChild);
                                leafsCopy.Add(leaf.rightChild);
                            }
                        }
                    }
                }
                leafs = leafsCopy;
            }
            l.createRoom();
        }
    }
}