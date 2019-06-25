using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Extended.MonogameMapImporter
{
    class MapReader : ContentTypeReader<TilemapData>
    {
        protected override TilemapData Read(ContentReader input, TilemapData existingInstance)
        {
            return new TilemapData(input.ReadString(),'\n');
        }
    }
}
