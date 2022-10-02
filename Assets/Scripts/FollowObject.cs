using UnityEngine;

public class FollowObject : MonoBehaviour {
    [SerializeField]
    private Transform trackedObject;

    private Vector3 _offset;

    private void Start() {
        _offset = transform.position - trackedObject.position;
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, trackedObject.position + _offset, Time.deltaTime * 2);
    }
}
