using UnityEngine;

[CreateAssetMenu(fileName = "ItemType", menuName = "ScriptableObjects/ItemType", order = 1)]
public class ItemType : ScriptableObject {
        public string itemName;
        public Sprite icon;
        public Mesh mesh;
        public Material material;
        public int weight = 1;
        public int value = 1;
        
}