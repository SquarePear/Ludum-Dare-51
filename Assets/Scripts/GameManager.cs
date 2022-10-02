using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
        public static GameManager Instance;
        
        [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
        [SerializeField] private List<Wave> _waves = new List<Wave>();
        
        private readonly List<ItemSpot> _itemSpots = new List<ItemSpot>();

        private const int _timerLength = 10; // Every 10 seconds
        private float _timer = 10f;
        
        private bool _running = false;
        private int _score = 0;

        private void Awake() { 
                if (Instance == null)
                        Instance = this;
        }
        
        private void OnDestroy() {
                if (Instance == this)
                        Instance = null;
        }
        
        private void Start() {
                _running = true;
                _timer = _timerLength;
                
                Spawn();
        }

        private void Update() {
                if (!_running)
                        return;
                
                _timer -= Time.deltaTime;
                
                GameUIManager.Instance.UpdateTimer(_timer);
                
                if (_timer > 0)
                        return;
                
                _timer = _timerLength;

                foreach (ItemSpot itemSpot in _itemSpots)
                        itemSpot.Restock();

                Spawn();
                AddScore(1);
        }

        private void Spawn() {
                if (_waves.Count == 0)
                        return;

                Wave wave = _waves[0];
                _waves.RemoveAt(0);
                
                
                
                if (_spawnPoints.Count < wave.waveItems.Count + wave.waveItemCrafts.Count)
                        return;

                foreach (WaveItemSpot waveItem in wave.waveItems)
                        SpawnItemSpot(waveItem.spotPrefab, waveItem.itemType);
                
                foreach (WaveItemCraft waveItem in wave.waveItemCrafts)
                        SpawnItemCraft(waveItem.craftPrefab, waveItem.recipe);
        }

        private void SpawnItemSpot(GameObject prefab, ItemType itemType) {
                Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
                GameObject itemSpotGameObject = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                ItemSpot itemSpot = itemSpotGameObject.GetComponent<ItemSpot>();
                
                itemSpot.SetItemType(itemType);
                
                _itemSpots.Add(itemSpot);
                _spawnPoints.Remove(spawnPoint);
        }

        private void SpawnItemCraft(GameObject prefab, CraftRecipe recipe) {
                Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
                GameObject itemSpotGameObject = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                ItemCraft itemCraft = itemSpotGameObject.GetComponent<ItemCraft>();
                
                itemCraft.SetRecipe(recipe);
                
                _spawnPoints.Remove(spawnPoint);
        }

        public void AddScore(int score) {
                _score += score;
                
                GameUIManager.Instance.UpdateScore(_score);
        }

        public int GetScore() {
                return _score;
        }
}