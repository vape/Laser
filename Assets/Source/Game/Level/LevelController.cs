using Laser.Game.Main;
using Laser.Game.Main.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Laser.Game.Level
{
    [Serializable]
    public class AbsorberTypePair
    {
        [SerializeField]
        public AbsorberType Type;
        [SerializeField]
        public AbsorberController Prefab;
    }

    [Serializable]
    public class EmitterTypePair
    {
        [SerializeField]
        public EmitterType Type;
        [SerializeField]
        public EmitterController Prefab;
    }

    [Serializable]
    public class ReflectorTypePair
    {
        [SerializeField]
        public ReflectorType Type;
        [SerializeField]
        public ReflectorController Prefab;
    }

    public partial class LevelController : MonoBehaviour
    {
        public GridController Grid;

        public AbsorberTypePair[] Absorbers;
        public EmitterTypePair[] Emitters;
        public ReflectorTypePair[] Reflectors;

        private void Awake()
        { }
    }
}
