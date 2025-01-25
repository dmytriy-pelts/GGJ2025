using Cysharp.Threading.Tasks;
using GumFly.Domain;
using GumFly.ScriptableObjects;
using GumFly.UI;
using GumFly.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace GumFly
{
    public enum GameState
    {
        Initializing,
        PickingGum,
        Chewing,
        PickingGas,
        Aiming,
        Finished
    }

    public struct StateChange
    {
        public GameState NewState;
        public GameState OldState;
    }

    public class GameManager : MonoSingleton<GameManager>
    {
        public GameState State { get; private set; } = GameState.Initializing;
        
        /// <summary>
        /// Gets the current gum-gas mixture. Be aware that the gum might not be selected yet.
        /// </summary>
        public GumGasMixture CurrentMixture { get; private set; } = new GumGasMixture();
        
        [field: SerializeField]
        public UnityEvent<StateChange> StateChanged { get; private set; }

        [SerializeField]
        private Inventory _inventory;
        private Inventory _inventoryInstance;

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
            StateChanged.Invoke(new StateChange { NewState = newState, OldState = oldState });
        }

        private async UniTask GameLoop()
        {
            while (_inventory.HasAnyGumsLeft)
            {
                // 1st step -- pick a gum
                ChangeState(GameState.PickingGum);
                var gum = await GumManager.Instance.PickGumAsync();
                CurrentMixture.Gum = gum;

                // 2nd step -- do the rhythm
                ChangeState(GameState.Chewing);
                float capacity = await RhyhmManager.Instance.ChewAsync(gum);

                // 3rd step -- gases
                ChangeState(GameState.PickingGas);
                await GasManager.Instance.PickGasesAsync(CurrentMixture, capacity);

                // 4th step -- aim
                ChangeState(GameState.Aiming);
                await AimManager.Instance.ShootAsync(CurrentMixture);
                CurrentMixture = new GumGasMixture();
            }

            ChangeState(GameState.Finished);
        }
    }
}