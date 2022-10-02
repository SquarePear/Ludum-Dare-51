using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCraft : MonoBehaviour, Interactable {
    [SerializeField] private List<Transform> itemPositions = new List<Transform>();
    
    [SerializeField] private List<TextMeshProUGUI> _itemCountTexts = new List<TextMeshProUGUI>();
    [SerializeField] private List<Image> _itemImages = new List<Image>();
    [SerializeField] private ParticleSystem _particleSystem;
    
    private AudioSource _audioSource;
    
    private readonly Dictionary<ItemType, int> _inventory = new Dictionary<ItemType, int>();

    private CraftRecipe _recipe;
    private ItemType _itemType;
    private readonly List<Item> _items = new List<Item>();
    
    private void Start() {
        _audioSource = GetComponentInChildren<AudioSource>();
        
        foreach (Transform t in itemPositions) {
            Item item = ItemManager.Instance.GetItem(_itemType, t.position);
            Transform itemTransform = item.transform;
            
            itemTransform.SetParent(t);
            itemTransform.localPosition = Vector3.zero;
            _items.Add(item);
        }
    }

    private void OnDestroy() {
        if (ItemManager.Instance == null) return;

        foreach (Item item in _items.Where(item => item != null)) 
            ItemManager.Instance.ReturnItem(item);
    }

    public void SetRecipe(CraftRecipe recipe) {
        _recipe = recipe;
        _itemType = recipe.resultItem;

        foreach (Item item in _items)
            item.SetType(_itemType);
        
        _particleSystem.textureSheetAnimation.SetSprite(0, _itemType.icon);
        _inventory.Clear();

        foreach (KeyValuePair<ItemType, int> item in _recipe.requiredItems)
            _inventory.Add(item.Key, 0);
        
        UpdateDisplay();
    }
    
    private void Update() {
        foreach (Item item in _items)
            item.transform.Rotate(0, Time.deltaTime * -16, 0);
    }

    public void Interact(PlayerManager player) {
        AddInventory(player);
        CraftItem(player);
    }

    private void AddInventory(PlayerManager player) {
        ItemType leastItem = null;
        int leastAmount = int.MaxValue;

        foreach (KeyValuePair<ItemType, int> item in _inventory) {
            if (!player.HasItem(item.Key)) continue;
            if (item.Value >= leastAmount) continue;

            leastItem = item.Key;
            leastAmount = item.Value;
        }

        if (leastItem == null) return;

        _inventory[leastItem]++;
        
        Play();
        UpdateDisplay();
        ItemManager.Instance.ReturnItem(player.DropItem(leastItem));
    }

    private void UpdateDisplay() {
        for (int i = 0; i < _recipe.requiredItems.Count; i++) {
            KeyValuePair<ItemType, int> item = _recipe.requiredItems.ElementAt(i);
            _itemImages[i].sprite = item.Key.icon;
            _itemCountTexts[i].text = $"{_inventory[item.Key]} / {item.Value}";
        }
    }

    private void Play() {
        _audioSource.Play();
        _particleSystem.Play();
    }
    
    private void CraftItem(PlayerManager player) {
        if (!player.CanHoldItem(_recipe.resultItem)) return;
        
        foreach (KeyValuePair<ItemType, int> item in _inventory)
            if (item.Value < _recipe.requiredItems[item.Key]) return;

        foreach (KeyValuePair<ItemType, int> item in _inventory.ToList())
            _inventory[item.Key] -= _recipe.requiredItems[item.Key];

        Play();
        UpdateDisplay();
        player.AddItem(_recipe.resultItem);
    }
}