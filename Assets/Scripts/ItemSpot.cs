using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpot : MonoBehaviour, Interactable {
    [SerializeField] protected List<Transform> itemPositions = new List<Transform>();

    [SerializeField] private TextMeshProUGUI _itemCountText;
    [SerializeField] private Image _itemImage;
    [SerializeField] private ParticleSystem _particleSystem;
    
    private AudioSource _audioSource;

    protected ItemType _itemType;
    protected readonly List<Item> _items = new List<Item>();
    
    protected int _maxItems = 3;
    protected int _itemCount = 3;
    
    private void Start() {
        _audioSource = GetComponentInChildren<AudioSource>();

        foreach (Transform t in itemPositions) {
            Item item = ItemManager.Instance.GetItem(_itemType, t.position);
            Transform itemTransform = item.transform;
            
            itemTransform.SetParent(t);
            itemTransform.localPosition = Vector3.zero;
            _items.Add(item);
        }
        
        Restock();
    }

    private void OnDestroy() {
        if (ItemManager.Instance == null) return;
        
        foreach (Item item in _items.Where(item => item != null)) 
            ItemManager.Instance.ReturnItem(item);
    }
    
    public void SetItemType(ItemType itemType) {
        _itemType = itemType;

        foreach (Item item in _items)
            item.SetType(_itemType);
        
        _particleSystem.textureSheetAnimation.SetSprite(0, _itemType.icon);

        Restock();
    }

    protected void UpdateDisplay() {
        _itemCountText.text = _itemCount.ToString();
        _itemImage.sprite = _itemType.icon;
    }
    
    protected void Play() {
        _particleSystem.Play();
        _audioSource.Play();
    }

    public virtual void Interact(PlayerManager player) {}
    
    public void Restock() {
        _itemCount = _maxItems;
        
        UpdateDisplay();
    }
}