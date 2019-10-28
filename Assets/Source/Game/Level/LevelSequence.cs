using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Laser.Game
{
    [CreateAssetMenu(fileName = "LevelSequence", menuName = "Level/Levels Sequence", order = 1)]
    public class LevelSequence : ScriptableObject
    {
        public string[] Levels;
    }
}
