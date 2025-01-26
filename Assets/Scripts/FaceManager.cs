using Cysharp.Threading.Tasks;
using GumFly.Extensions;
using GumFly.UI;
using GumFly.UI.ChewChew;
using GumFly.Utils;
using LitMotion;
using LitMotion.Extensions;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GumFly
{
    [RequireComponent(typeof(RectTransform))]
    [DefaultExecutionOrder(-800)]
    public class FaceManager : MonoSingleton<FaceManager>
    {
        private RectTransform _rectTransform;

        [SerializeField]
        private Transform _cursorPos;

        [SerializeField]
        private Transform _cheek;

        [SerializeField]
        private Transform _chew0;

        [SerializeField]
        private Transform _chew1;

        [SerializeField]
        private Transform _chew2;

        private Vector3 _initialPosition;

        private bool _isInAimingPosition = false;

        protected override void Awake()
        {
            base.Awake();

            _rectTransform = GetComponent<RectTransform>();
            _initialPosition = transform.position;
        }


        private void Start()
        {
            GameManager.Instance.StateChanged.AddListener(OnStateChanged);
            RhythmManager.Instance.Chewed.AddListener(OnChewed);
        }

        private void Update()
        {
            if (!_isInAimingPosition) return;
            
            float scale =  EventSystem.current.IsPointerOverGameObject() ? 0.5f : 1f;

            float targetHeight = Camera.main.ScreenToWorldPoint(
                Input.mousePosition.WithY(Mathf.Clamp(Input.mousePosition.y,
                    400.0f,
                    GameManager.TARGET_RESOLUTION.y - 200.0f)
                )
            ).y;
            
            transform.position = Vector3.Lerp(
                transform.position,
                transform.position.WithHeight(targetHeight),
                Time.deltaTime * 5.0f * scale
            );
        }

        private void OnChewed(ChewEvent e)
        {
            SoundManager.Instance.PlayChew();

            if (e.Key == KeyType.L)
            {
                _chew0.gameObject.SetActive(false);
                _chew1.gameObject.SetActive(true);
                _chew2.gameObject.SetActive(false);
            }
            else
            {
                _chew0.gameObject.SetActive(false);
                _chew1.gameObject.SetActive(false);
                _chew2.gameObject.SetActive(true);
            }
        }

        public async UniTask MoveToGums()
        {
            _isInAimingPosition = false;
            var min = Camera.main.ScreenToWorldPoint(Vector3.zero);
            var max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

            var gumManager = GumManager.Instance.RectTransform;

            // Hide
            SoundManager.Instance.PlaySwoosh(0);
            await LMotion.Create(transform.position,
                    transform.position.WithX(min.x - _rectTransform.rect.width), 1f)
                .WithEase(Ease.InOutCirc)
                .BindToPosition(transform);

            await UniTask.Delay(250);

            // Re-emerge
            SoundManager.Instance.PlaySwoosh(1);
            transform.localScale = Vector3.one * 1.5f;

            var centerBottom = gumManager.TransformPoint(new Vector2(gumManager.rect.center.x, gumManager.rect.min.y));
            var centerTop =
                gumManager.TransformPoint(new Vector2(gumManager.rect.center.x, gumManager.rect.max.y + 100));
            await LMotion.Create(
                    centerBottom,
                    centerTop,
                    0.5f)
                .BindToPosition(transform);
        }

        public async UniTask MoveToChewPosition()
        {
            _isInAimingPosition = false;

            var translation = RhythmManager.Instance.Cursor.position - _cursorPos.position;
            var rhythmManager = RhythmManager.Instance.Cursor;

            SoundManager.Instance.PlaySwoosh(3);
            await LMotion.Create(transform.position, transform.position + translation, 0.5f)
                .WithEase(Ease.InOutCubic)
                .BindToPosition(transform);
        }

        public async UniTask MoveToAimingPosition()
        {
            var min = Camera.main.ScreenToWorldPoint(Vector3.zero);
            var max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

            SoundManager.Instance.PlaySwoosh(4);
            await LMotion.Create(transform.position,
                    transform.position.WithY(min.y) + _rectTransform.rect.height * Vector3.down, 0.5f)
                .BindToPosition(transform);

            transform.localScale = Vector3.one;
            SoundManager.Instance.PlaySwoosh(5);
            await LMotion.Create(_initialPosition.WithX(min.x - _rectTransform.rect.width), _initialPosition, 0.5f)
                .BindToPosition(transform);

            _isInAimingPosition = true;
        }

        private void OnStateChanged(StateChangeEvent e)
        {
            if (e.OldState == GameState.Aiming)
            {
                EnableChewing(false);
            }

            switch (e.NewState)
            {
                case GameState.Initializing:
                    break;
                case GameState.PickingGum:

                    break;
                case GameState.Chewing:
                    EnableChewing(true);


                    break;
                case GameState.Aiming:
                    break;
                case GameState.Finished:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EnableChewing(bool enabled)
        {
            _cheek.gameObject.SetActive(!enabled);
            _chew0.gameObject.SetActive(enabled);
            _chew1.gameObject.SetActive(false);
            _chew2.gameObject.SetActive(false);
        }
    }
}