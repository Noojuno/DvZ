using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runed.Voxel
{
    public class BlockTypeTest : BlockType
    {
        public override string Identifier => "blocktest";
        public override string DisplayName => "Test Block";
    }
}
