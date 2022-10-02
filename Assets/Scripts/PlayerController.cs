using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform raycastOrigin;
    
    private PlayerManager _playerManager;
    private CharacterController _controller;
    private AudioSource _audioSource;
    
    private Vector2 _direction = Vector2.zero;
    private bool _interact = false;
    
    private void Awake() {
        _playerManager = GetComponent<PlayerManager>();
        _controller = GetComponent<CharacterController>();
        _audioSource = transform.parent.GetComponentInChildren<AudioSource>();
    }

    private void FixedUpdate() {
        // Move
        Vector3 movement = new Vector3(_direction.x, 0, _direction.y) * speed;
        _controller.Move(movement);
        _controller.transform.LookAt(_controller.transform.position + movement);

        // Interact
        if (!_interact)
            return;
        
        _interact = false;
        
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, 1f, LayerMask.GetMask("Interactable")))
            return;

        Interactable interactable = hit.collider.GetComponentInParent<Interactable>();

        interactable?.Interact(_playerManager);
    }

    private void OnMove(InputValue value) {
        _direction = value.Get<Vector2>() * 0.02f;
    }
    
    private void OnInteract(InputValue value) {
        _interact = value.isPressed;
    }
}