using GumFly.Domain;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace GumFly.UI.ChewChew
{
    [RequireComponent(typeof(CanvasGroup))]
    public class KeyBehaviour : MonoBehaviour, IInitializable<RhythmEvent>
    {
        private RhythmEvent _event;
        private CanvasGroup _group;

        [SerializeField]
        private Graphic _graphic;

        public RhythmEvent Event => _event;

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }

        public void Initialize(RhythmEvent instance)
        {
            _event = instance;
        }

        public void Consume(RhythmJudgement result)
        {
            if (result == RhythmJudgement.Wrong)
            {
                LMotion.Create(1.0f, 0.0f, 2f)
                    .WithEase(Ease.Linear)
                    .BindToAlpha(_group);

                LMotion.Create(_graphic.color, Color.gray, 0.1f)
                    .BindToColor(_graphic);
            }
            else
            {
                
                LMotion.Create(1.0f, 0.0f, 1f)
                    .WithEase(Ease.InCubic)
                    .BindToAlpha(_group);
                
                if (result == RhythmJudgement.Bad)
                {
                    var x = transform.localPosition.x;
                    LMotion.Create(x - 100f, x + 100f, 0.5f)
                        .WithLoops(3, LoopType.Yoyo)
                        .WithEase(Ease.InOutCubic)
                        .BindToLocalPositionX(transform);
                }


                LMotion.Create(transform.localScale, transform.localScale * 2.0f, 1f)
                    .WithEase(Ease.OutCubic)
                    .BindToLocalScale(transform);


                LMotion.Create(0.0f, 400.0f, 2f)
                    .WithOnComplete(() => Destroy(gameObject))
                    .BindToLocalPositionY(transform);
            }
        }
    }
}