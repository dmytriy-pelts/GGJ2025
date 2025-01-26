using Cysharp.Threading.Tasks;
using GumFly.Utils;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GumFly.UI
{
    public class Stats
    {
        public int TotalFlies { get; set; }
        public int RemainingFlies { get; set; }
        public int RemainingGums { get; set; }
        public float RemainingGas { get; set; }
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [SerializeField]
        private TMP_Text _text;

        [SerializeField]
        private TMP_Text _finalScore;

        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private CanvasGroup _board;
        
        protected override void Awake()
        {
            base.Awake();
            _restartButton.onClick.AddListener(() => Restart().Forget());
            gameObject.SetActive(false);
        }

        private async UniTask Restart()
        {
            _restartButton.interactable = false;

            await LMotion.Create(1.0f, 0.0f, 3.0f)
                .BindToAlpha(_board);

            SceneManager.LoadScene("Init");
        }

        public async void Show(Stats stats)
        {
            gameObject.SetActive(true);
            _restartButton.gameObject.SetActive(false);
            transform.localScale = Vector3.zero;

            await LMotion.Create(Vector3.zero, Vector3.one, 1.0f)
                .WithEase(Ease.OutBounce)
                .BindToLocalScale(transform);

            await UniTask.Delay(1000);
            
            int fliesKilled = stats.TotalFlies - stats.RemainingFlies;
            float score = ((int)(fliesKilled * (stats.RemainingGums * stats.RemainingGas + 1.0f)));

            string text1 = $@"
<b>Killed Flies</b>: {stats.TotalFlies - stats.RemainingFlies} / {stats.TotalFlies}
<b>Remaining Gums</b>: {stats.RemainingGums}
<b>Remaining Gas</b>: {stats.RemainingGas * 100.0f:0}%";

            string text2 = $"<b>Final Score</b>: <u>{score:0}</u>";


            await LMotion.String.Create512Bytes("", text1, 5.0f)
                .WithRichText()
                .BindToText(_text);

            await UniTask.Delay(1000);
            await LMotion.String.Create512Bytes("", text2, 1.0f)
                .WithRichText()
                .WithEase(Ease.OutCubic)
                .BindToText(_finalScore);

            await UniTask.Delay(1000);
            _restartButton.gameObject.SetActive(true);
        }
    }
}