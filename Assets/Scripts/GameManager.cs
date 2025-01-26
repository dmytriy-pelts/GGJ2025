using Cysharp.Threading.Tasks;
using GumFly.Domain;
using GumFly.ScriptableObjects;
using GumFly.UI;
using GumFly.Utils;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GumFly
{
    public enum GameState
    {
        Initializing,
        PickingGum,
        Chewing,
        Aiming,
        Finished
    }

    public struct StateChangeEvent
    {
        public GameState NewState;
        public GameState OldState;
    }

    [DefaultExecutionOrder(-1000)]
    public class GameManager : MonoSingleton<GameManager>
    {
        public static Vector2 TARGET_RESOLUTION = new Vector2(1920, 1080);

        public GameState State { get; private set; } = GameState.Initializing;

        /// <summary>
        /// Gets the current gum-gas mixture. Be aware that the gum might not be selected yet.
        /// </summary>
        public GumGasMixture CurrentMixture { get; private set; } = new GumGasMixture();

        [field: SerializeField]
        public UnityEvent<StateChangeEvent> StateChanged { get; private set; }

        [SerializeField]
        private Inventory _inventory;

        private Inventory _inventoryInstance;
        private bool _gameOver;

        private void Start()
        {
            if (!_inventory)
            {
                Debug.LogError("Please set the inventory on the GameManager!", this);
                return;
            }

            _inventoryInstance = Instantiate(_inventory);

            GumManager.Instance.Initialize(_inventoryInstance);
            GasManager.Instance.Initialize(_inventoryInstance);


            GameLoop().Forget();
        }

        private void ChangeState(GameState newState)
        {
            Debug.Log($"New State: {newState}");

            GameState oldState = State;
            State = newState;

            StateChanged.Invoke(new StateChangeEvent { NewState = newState, OldState = oldState });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                _gameOver = true;
            }
        }

        private async UniTask GameLoop()
        {
            await UniTask.Delay(2000);
            float startTime = Time.time;

            FlyManager.Instance.AllFliesDead.AddListener(() =>
            {
                EndIt(startTime).Forget();
            });
            var cancellation = FlyManager.Instance.AllFliesDead
                .OnInvokeAsync(gameObject.GetCancellationTokenOnDestroy())
                .ToCancellationToken();


            while (_inventoryInstance.HasAnyGumsLeft
                   && _inventoryInstance.HasAnyGasLeft
                   && FlyManager.Instance.RemainingFlyCount > 0 && !_gameOver)
            {
                // 1st step -- pick a gum
                ChangeState(GameState.PickingGum);
                var gum = await GumManager.Instance.PickGumAsync(cancellation);
                CurrentMixture.Gum = gum;

                // 2nd step -- do the rhythm
                ChangeState(GameState.Chewing);
                float capacity = await RhythmManager.Instance.ChewAsync(gum, cancellation);

                // 3rd step -- aim and load
                ChangeState(GameState.Aiming);
                await AimManager.Instance.AimAsync(CurrentMixture, cancellation);

                CurrentMixture = new GumGasMixture();
            }

            EndIt(startTime).Forget();
        }

        private async UniTask EndIt(float startTime)
        {
            float endTime = Time.time;
            ChangeState(GameState.Finished);

            await UniTask.Delay(1000);
            await AimManager.Instance.WaitUntilBubblesAreGone();

            int remainingGums = _inventoryInstance.Gums.Sum(it => it.Count);
            float remainingGas = _inventoryInstance.Gases.Average(it => it.Fill);

            ScoreManager.Instance.Show(new Stats()
            {
                RemainingFlies = FlyManager.Instance.RemainingFlyCount,
                TotalFlies = FlyManager.Instance.DeadFlyCount + FlyManager.Instance.RemainingFlyCount,
                RemainingGas = remainingGas,
                RemainingGums = remainingGums,
                Elapsed = TimeSpan.FromSeconds(endTime - startTime)
            });
        }
    }
}