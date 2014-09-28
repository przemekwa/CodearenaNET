using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ai
{
    public class DiamondAlgorithm
    {
        private List<HexField> map { get; set; }

        private Sees diamond { get; set; }

        public DiamondAlgorithm(List<HexField> hexMap, Sees diamondSees)
        {
            this.map = hexMap;
            this.diamond = diamondSees;
        }

        public List<Coordinate> CaluculatPath()
        {
            return new List<Coordinate>();
        }
    }
}
