using Microsoft.Xna.Framework.Content;
using Monogame.Extended.MonogameTilesetImporter;

namespace RTS1.MonoGame.Extended
{
    class TilesetReader : ContentTypeReader<TilesetData>
    {
        protected override TilesetData Read(ContentReader input, TilesetData existingInstance)
        {
            return new TilesetData("",input.ReadString(), '\n');
        }
    }
}
