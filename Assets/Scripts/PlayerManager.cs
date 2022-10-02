using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [SerializeField] private int _strength = 5;
    [SerializeField] private Transform hands;
    
    private readonly List<Item> _inventory = new List<Item>();

    private int GetWeight() {
        return _inventory.Sum(item => item.GetWeight());
    }
    
    public bool AddItem(ItemType itemType) {
        if (!CanHoldItem(itemType))
            return false;
        
        Item item = ItemManager.Instance.GetItem(itemType, Vector3.zero);
        
        _inventory.Add(item);
        UpdateItemPositions();
        
        GameUIManager.Instance.UpdateHands(GetWeight(), _strength);
            
        return true;
    }
    
    public bool HasItem(ItemType itemType) {
        return _inventory.Any(item => item.GetType() == itemType);
    }
    
    public bool CanHoldItem(ItemType itemType) {
        return itemType.weight + GetWeight() <= _strength;
    }
    
    private void UpdateItemPositions() {
        for (int i = 0; i < _inventory.Count; i++) {
            _inventory[i].transform.SetParent(hands);
            _inventory[i].transform.localPosition = new Vector3(0, i * 0.5f, 0);
            _inventory[i].transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }
    
    public Item DropItem(ItemType itemType) {
        int index = _inventory.FindIndex(item => item.GetType() == itemType);
        
        if (index == -1)
            return null;
        
        Item item = _inventory[index];
        _inventory.RemoveAt(index);
        
        UpdateItemPositions();
        GameUIManager.Instance.UpdateHands(GetWeight(), _strength);

        return item;
    }
    
    public List<Item> DropAllItems() {
        List<Item> items = new List<Item>();

        foreach (Item item in _inventory) {
            items.Add(item);
            _inventory.Remove(item);
        }
        
        GameUIManager.Instance.UpdateHands(GetWeight(), _strength);
        
        return items;
    }
}