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

        private async UniTask GameLoop()
        {
            await UniTask.Delay(2000);
            
            while (_inventory.HasAnyGumsLeft)
            {
                // 1st step -- pick a gum
                ChangeState(GameState.PickingGum);
                var gum = await GumManager.Instance.PickGumAsync();
                CurrentMixture.Gum = gum;

                // 2nd step -- do the rhythm
                ChangeState(GameState.Chewing);
                float capacity = await RhythmManager.Instance.ChewAsync(gum);
                
                // 3rd step -- aim and load
                ChangeState(GameState.Aiming);
                await AimManager.Instance.AimAsync(CurrentMixture);
                
                CurrentMixture = new GumGasMixture();
            }

            ChangeState(GameState.Finished);
        }
    }
}