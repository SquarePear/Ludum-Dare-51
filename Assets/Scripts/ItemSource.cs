using UnityEngine;

public class ItemSource : ItemSpot {
    public override void Interact(PlayerManager player) {
        if (_itemCount <= 0) return;
        if (!player.CanHoldItem(_itemType)) return;
        
        _itemCount--;
        UpdateDisplay();
        Play();
        player.AddItem(_itemType);
    }
}