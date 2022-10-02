using UnityEngine;

public class Item : MonoBehaviour {
    private ItemType _type;
    
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    
    private void Awake() {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _meshFilter = GetComponentInChildren<MeshFilter>();
    }
    
    public new ItemType GetType() {
        return _type;
    }
    
    public void SetType(ItemType type) {
        _type = type;
        
        _meshFilter.mesh = type.mesh;
        _meshRenderer.material = type.material;
    }
    
    public string GetName() {
        return _type.name;
    }

    public int GetWeight() {
        return _type.weight;
    }
}