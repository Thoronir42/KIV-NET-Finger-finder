﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FingerprintAnalyzer.Model;

namespace FingerprintAnalyzer
{
    /* Minutia */
    partial class FingerprintAnalyzer
    {
        public List<Minutia> detectMinituae()
        {
            var types = (MinutiaType[])Enum.GetValues(typeof(MinutiaType));
            List<Minutia> minutiae = new List<Minutia>();
            Random randVal = new Random(413);

            for (int i = 0; i < types.Length; i++)
            {
                PointF point = new PointF((float)randVal.NextDouble(), (float)randVal.NextDouble());
                Minutia minutia = new Minutia { Type = types[i], Position = point };
                minutiae[i] = minutia;
            }
            return minutiae;
        }

    }
}
