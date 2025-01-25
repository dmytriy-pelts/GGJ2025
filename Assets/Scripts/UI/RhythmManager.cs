using Cysharp.Threading.Tasks;
using GumFly.ScriptableObjects;
using GumFly.UI.ChewChew;
using GumFly.Utils;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GumFly.UI
{
    public struct ChewEvent
    {
        public KeyType Key;
    }
    
    [DefaultExecutionOrder(-999)]
    public class RhythmManager : MonoSingleton<RhythmManager>
    {
        [SerializeField]
        private RhythmProcessor _processor;

        [field:SerializeField]
        public RectTransform Cursor { get; private set; }
        public UnityEvent<ChewEvent> Chewed { get; } = new UnityEvent<ChewEvent>();

        protected override void Awake()
        {
            base.Awake();
            _processor.Chewed.AddListener(OnJudgementMade);
        }

        private void OnJudgementMade(RhythmJudgementEvent e)
        {
            float factor = e.Judgement switch
            {
                RhythmJudgement.Perfect => 1.0f,
                RhythmJudgement.Good => 0.7f,
                RhythmJudgement.Bad => 0.5f,
                RhythmJudgement.Wrong => 0.0f
            };

            GameManager.Instance.CurrentMixture.Capacity += factor / _processor.EventCount;
            
            Chewed.Invoke(new ChewEvent()
            {
                Key = e.Key,
            });
        }

        public async UniTask<float> ChewAsync(Gum gum)
        {
            // Wait for face
            await UniTask.Delay(1000);
            
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