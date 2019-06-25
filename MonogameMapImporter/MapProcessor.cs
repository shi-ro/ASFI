using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Extended.MonogameMapImporter
{
    [ContentProcessor(DisplayName = "Tilemap Processor - Monogame.Extended")]
    class MapProcessor : ContentProcessor<string, TilemapData>
    {
        public override TilemapData Process(string input, ContentProcessorContext context)
        {
            return new TilemapData(input, '\n');
        }
    }
}
