using System;
using UnityEngine.Timeline;

namespace GumFly.Domain
{
    [Serializable]
    public class Rhythm
    {
        public TimelineAsset[] Timelines;
        public float Duration = 5.0f;
    }
}