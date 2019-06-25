using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameMapImporter
{
    [ContentImporter(".tilemap", DefaultProcessor = "MapProcessor", DisplayName = "Tilemap Importer - Monogame.Extended")]
    class MapImporter : ContentImporter<string>
    {
        public override string Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage($"Importing Tilemap file: {filename}");
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
