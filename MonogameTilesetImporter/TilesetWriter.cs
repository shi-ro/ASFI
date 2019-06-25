using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame.Extended.MonogameTilesetImporter
{
    [ContentTypeWriter]
    class TilesetWriter : ContentTypeWriter<TilesetData>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "RTS1.MonoGame.Extended.TilesetReader, RTS1";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(TilesetData).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, TilesetData value)
        {
            output.Write(value.Data);
        }
    }
}
