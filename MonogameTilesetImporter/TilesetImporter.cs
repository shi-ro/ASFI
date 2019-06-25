using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame.Extended.MonogameTilesetImporter
{
    [ContentImporter(".tilesetting", DefaultProcessor = "TilesetProcessor", DisplayName = "Tilesetting Importer - Monogame.Extended")]
    class TilesetImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage($"Importing Tilesetting file: {filename}");
            string whole = "";
            using (StreamReader sr = new StreamReader(filename))
            {
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    whole += line;
                    if (sr.Peek() >= 0)
                    {
                        whole += '\n';
                    }
                }
            }
            return whole;
        }
    }
}
