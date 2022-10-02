using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
        public static ItemManager Instance;
        
        [SerializeField] private ItemType defaultItemType;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private int maxItems = 64;
        
        private readonly List<GameObject> _items = new List<GameObject>();
        private readonly List<GameObject> _freeItems = new List<GameObject>();

        private void Awake() { 
                if (Instance == null)
                        Instance = this;
        }
        
        private void OnDestroy() {
                if (Instance == this)
                        Instance = null;
                
                foreach (GameObject item in _items)
                        Destroy(item);
        }
        
        public ItemType GetDefaultItemType() {
                return defaultItemType;
        }
        
        public Item GetItem(ItemType type, Vector3 position) {
                GameObject itemObject = null;

                if (_freeItems.Count == 0) {
                        itemObject = Instantiate(itemPrefab, position, Quaternion.identity);
                        _items.Add(itemObject);
                } else {
                        itemObject = _freeItems[0];
                        _freeItems.RemoveAt(0);
                }

                Item item = itemObject.GetComponent<Item>();

                itemObject.SetActive(true);
                itemObject.transform.SetParent(transform);
                
                item.SetType(type);

                return item;
        }
        
        public void ReturnItem(Item item) {
                ReturnItem(item.gameObject);
        }
        
        public void ReturnItem(GameObject itemObject) {
                if (!_items.Contains(itemObject))
                        return;
                
                if (_items.Count > maxItems) {
                        _items.Remove(itemObject);
                        Destroy(itemObject);
                        
                        return;
                }
                
                itemObject.transform.SetParent(transform);
                itemObject.SetActive(false);
                _freeItems.Add(itemObject);
        }
}