using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{ 
    private PlayerInput _playerInput;
    private UnityEngine.InputSystem.PlayerInput _currentInput;
    private Vector2 _move;
    private CharacterController _cc;
 
    [Range(2, 20)]
    [SerializeField] private float playerSpeed;
 
    private float _verticalSpeed;
    
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform checkPos;
 
    private Vector3 _movement;
 
    private const float Gravity = -9.8f;
    [SerializeField]private float jumpSpeed;
 
    private float _camRotation;
    [SerializeField] private Vector2 mouseSensitivity;
    [SerializeField] private Vector2 controllerSensitivity;
    [SerializeField] private Transform camTransform;
    private Vector2 _mouseInput;
    
 
    [SerializeField] private Transform parentTransform;
 
 
    [SerializeField] private float slideDelay;
    [SerializeField] private float slideBoostTime;
 
    private  const float SlideBoostDefault = 1;
    [SerializeField] private float slideBoostAffected;
    private float _slideBoost;
    private bool _canSlide;
 
    private const float CrouchAffected = 0.85f;
    private const float CrouchDefault = 1;
    
     private void Awake()
   {
      _playerInput = new PlayerInput();
      TryGetComponent(out _cc);
      TryGetComponent(out _currentInput);
      _slideBoost = SlideBoostDefault;
      _canSlide = true;
      _playerInput.Player.Move.performed += tgb => _move = tgb.ReadValue<Vector2>();
      _playerInput.Player.Move.canceled += _ => _move = Vector2.zero;
      _playerInput.Player.Jump.performed += _ => JumpAction();
      _playerInput.Player.Look.performed += tgb => _mouseInput = tgb.ReadValue<Vector2>();
      _playerInput.Player.Look.canceled += _ => _mouseInput = Vector2.zero;
      _playerInput.Player.Crouch.performed += _ => Crouch(CrouchAffected);
      _playerInput.Player.Crouch.canceled += _ => Crouch(CrouchDefault);

      _playerInput.Player.Slide.performed += _ => Slide();
   }

   private void OnEnable()
   {
      _playerInput.Enable();
   }

   private void OnDisable()
   {
      _playerInput.Disable();
   }

   private void Update()
   {
      _movement = Vector3.zero;
      Move();
      Jump();
      MouseLook();
      _cc.Move(_movement);
   }

   private void Move()
   {
      var ySpeed = _move.y * playerSpeed * _slideBoost* Time.deltaTime;
      _movement += ySpeed * transform.forward;
      var xSpeed = _move.x * playerSpeed * _slideBoost * Time.deltaTime;
      _movement += xSpeed * transform.right;

   }

   private void Jump()
   {
      _verticalSpeed += Gravity * Time.deltaTime;
      _movement += transform.up * (_verticalSpeed * Time.deltaTime);
      
   }
   
   private void JumpAction()
   {
      if(!CheckJumpAction()) return;
      _verticalSpeed = jumpSpeed;

   }

   private bool CheckJumpAction()
   {
      if (!Physics.CheckSphere(checkPos.position, 0.5f, groundMask) || !(_verticalSpeed <= 0)) return false;
      _verticalSpeed = 0;
      return true;
   }

   private void MouseLook()
   {
      var mouseX = _mouseInput.x * CheckSensInput().x * Time.deltaTime;
      transform.Rotate(new Vector3(transform.rotation.x, mouseX, transform.rotation.z));
      var mouseY = -_mouseInput.y * CheckSensInput().y * Time.deltaTime;
      _camRotation += mouseY;
      _camRotation = Mathf.Clamp(_camRotation, -80f, 90f);
      camTransform.localRotation = Quaternion.Euler(new Vector3(_camRotation, transform.rotation.y, transform.rotation.z));
   }

   private Vector2 CheckSensInput()
   {
      return _currentInput.currentControlScheme switch
      {
         "Gamepad" => controllerSensitivity * 10,
         "Keyboard&Mouse" => mouseSensitivity,
         _ => Vector2.zero
      };
   }
   
   private void Crouch(float crouchVar)
   {
      parentTransform.localScale = new Vector3(1, crouchVar, 1);
   }

   private void Slide()
   {
      if(!_canSlide || !CheckJumpAction()) return;
      StartCoroutine(SlideDelay());
   }

   private IEnumerator SlideDelay()
   {
      _canSlide = false;
      _slideBoost = slideBoostAffected;
      Crouch(CrouchAffected);
      yield return new WaitForSeconds(slideBoostTime);
      _slideBoost = SlideBoostDefault;
      Crouch(CrouchDefault);

      yield return new WaitForSeconds(slideDelay);
      _canSlide = true;
   }
}
