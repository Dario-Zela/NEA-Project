using Newtonsoft.Json;
using Pixel_Engine;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Models.Sprites
{
    public sealed class TileSet
    {
        private Dictionary<string, TileSetData> GameTileSet;

        private static TileSet instance = null;
        public static TileSet Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TileSet();
                }
                return instance;
            }
        }

        public Sprite[] GetSprites(string name)
        {
            return GameTileSet[name].value;
        }
        private TileSet()
        {
            GameTileSet = JsonConvert.DeserializeObject<Dictionary<string, TileSetData>>(File.ReadAllText(@"..\..\..\Models\Sprites\TileSetSpec.json"));
            foreach (var TileSetD in GameTileSet.Values)
            {
                TileSetD.init();
            }
        }

        internal class TileSetData
        {
            public string src = "";
            public int tileWidth = 0;

            public Sprite[] value;
            public void init()
            {
                try
                {
                    Bitmap bitmap = new Bitmap(new FileStream(@"..\..\..\Models\Sprites\" + src, FileMode.Open));
                    value = new Sprite[(bitmap.Width / tileWidth) * (bitmap.Height / tileWidth)];
                    int width = bitmap.Width / tileWidth;
                    int w = 0;
                    int h = 0;
                    for (int i = 0; i < value.Length; i++)
                    {
                        value[i] = new Sprite(bitmap.Clone(new Rectangle(w, h, tileWidth, tileWidth), System.Drawing.Imaging.PixelFormat.DontCare));
                        w = w + tileWidth == bitmap.Width ? 0 : w + tileWidth;
                        h = w == 0 ? h + tileWidth : h;
                    }
                }
                catch { }
            }
        }
    }
}
