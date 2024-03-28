﻿using MCPE.AlphaServer.Game.material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPE.AlphaServer.Game.BlockBase
{
    public class SolidBlock : Block
    {
        public SolidBlock(int id, Material m) : base(id, m)
        {
        }
        public SolidBlock setBlockName(string name)
        {
            this.name = name;
            return this;
        }
    }
}