using UnityEngine;

public class ItemDrain : ItemSpot {
    private void Update() {
        foreach (Item item in _items)
            item.transform.Rotate(0, Time.deltaTime * 16, 0);
    }

    public override void Interact(PlayerManager player) {
        if (_itemCount <= 0) return;
        
        Item item = player.DropItem(_itemType);
        
        if (item == null) return;
        
        _itemCount--;
        
        UpdateDisplay();
        Play();
        GameManager.Instance.AddScore(_itemType.value);
        ItemManager.Instance.ReturnItem(item);
    }
}