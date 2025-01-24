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

        [SerializeField]
        private Inventory _inventory;

        [field: SerializeField]
        public UnityEvent<StateChange> StateChanged { get; private set; }


        private Inventory _inventoryInstance;
        private GumGasMixture _currentMixture = new GumGasMixture();

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
        }

        private void ChangeState(GameState newState)
        {
            GameState oldState = State;
            StateChanged.Invoke(new StateChange { NewState = newState, OldState = oldState });
        }

        private void GameLoop()
        {
        }
    }
}