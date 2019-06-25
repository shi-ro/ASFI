using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Extended.MonogameMapImporter
{
    [ContentTypeWriter]
    class MapWriter : ContentTypeWriter<TilemapData>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "RTS1.MonoGame.Extended.MapReader, RTS1";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(TilemapData).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, TilemapData value)
        {
            output.Write(value.Data);
        }
    }
}
