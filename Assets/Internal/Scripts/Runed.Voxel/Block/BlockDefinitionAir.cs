using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runed.Voxel
{
    public class BlockDefinitionAir : BlockDefinition
    {
        public override string Identifier => "air";
        public override string DisplayName => "Air";

        public override bool Render => false;
    }
}
