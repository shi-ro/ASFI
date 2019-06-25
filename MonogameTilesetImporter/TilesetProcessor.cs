using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame.Extended.MonogameTilesetImporter
{
    [ContentProcessor(DisplayName = "Tilesetting Processor - Monogame.Extended")]
    class TilesetProcessor : ContentProcessor<string, TilesetData>
    {
        public override TilesetData Process(string input, ContentProcessorContext context)
        {
            return new TilesetData(input, '\n');
        }
    }
}
