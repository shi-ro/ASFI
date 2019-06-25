using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.MonogameMapImporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1.MonoGame.Extended
{
    class MapReader : ContentTypeReader<TilemapData>
    {
        protected override TilemapData Read(ContentReader input, TilemapData existingInstance)
        {
            return new TilemapData("",input.ReadString(), '\n');
        }
    }
}
