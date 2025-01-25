using Cysharp.Threading.Tasks;
using GumFly.ScriptableObjects;
using GumFly.UI.ChewChew;
using GumFly.Utils;
using LitMotion;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace GumFly.UI
{
    public class RhythmManager : MonoSingleton<RhythmManager>
    {
        [SerializeField]
        private RhythmProcessor _processor;

        private void Awake()
        {
            _processor.JudgementMade.AddListener(OnJudgementMade);
        }

        private void OnJudgementMade(RhythmJudgement j)
        {
            float factor = j switch
            {
                RhythmJudgement.Perfect => 1.0f,
                RhythmJudgement.Good => 0.7f,
                RhythmJudgement.Bad => 0.5f,
                RhythmJudgement.Wrong => 0.0f
            };

            GameManager.Instance.CurrentMixture.Capacity += factor / _processor.EventCount;
        }

        public async UniTask<float> ChewAsync(Gum gum)
        {
            GameManager.Instance.CurrentMixture.Capacity = 0.0f;
            var timelines = gum.Rhythm.Timelines;
            if (timelines.Length == 0)
            {
                GameManager.Instance.CurrentMixture.Capacity = 1.0f;
                return 1.0f;
            }

            var timeline = timelines[Random.Range(0, timelines.Length)];
            _processor.Initialize(timeline);

            await _processor.PlayAsync();

            return GameManager.Instance.CurrentMixture.Capacity;
        }
    }
}