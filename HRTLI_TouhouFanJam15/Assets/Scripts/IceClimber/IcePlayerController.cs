using UnityEngine;
using UnityEngine.InputSystem;

public class IcePlayerController : MonoBehaviour
{
    public bool isAlive = true;
    public PlayerInput playerInput;
    public Transform spriteAnchor;
    public SpriteRenderer spriteRender;

    Vector2 _movementInput;
    float _xInput;

    float _moveForce = 5f;
    float _jumpForce = 30f;
    float _jumpThreshold = 1.5f;

    float _playerFacing = 1f;

    bool _isJumpKeyPressed = false;
    bool _inAir = false;
    bool _hasKnockedBlock = false;

    int _layermask;

    Rigidbody2D _rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _movementInput = Vector2.zero;
        _rigidbody = GetComponent<Rigidbody2D>();

        _layermask = (1 << 8); //Player
        _layermask = ~_layermask;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateJump();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _xInput = ctx.ReadValue<Vector2>().x;

        if (_xInput > 0)
        {
            _playerFacing = 1;
            //spriteRender.flipX = false;
        }
        if (_xInput < 0)
        {
            _playerFacing = -1;
            //spriteRender.flipX = true;
        }

        //_anim.SetFloat("XVelocity", _xInput);
    }

    void UpdateMovement()
    {
        _rigidbody.linearVelocity = new Vector2(_xInput * _moveForce, _rigidbody.linearVelocity.y);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            _isJumpKeyPressed = true;
    }

    void UpdateJump()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 0.4f, transform.position.y),
            Vector2.down, _jumpThreshold, _layermask);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x - 0.4f, transform.position.y),
            Vector2.down, _jumpThreshold, _layermask);
        Gizmos.color = Color.cyan;

        if (hit || hit2)
        {
            _inAir = false;
            _hasKnockedBlock = false;
            Debug.DrawLine(new Vector2(transform.position.x + 0.4f, transform.position.y), hit.point, Color.cyan);
            Debug.DrawLine(new Vector2(transform.position.x - 0.4f, transform.position.y), hit2.point, Color.cyan);
        }
        else
        {
            _inAir = true;
        }

        if (!_inAir && _isJumpKeyPressed)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _jumpForce);
        }

        _isJumpKeyPressed = false;
    }

    public bool CanDestroyBlock()
    {
        if (!_hasKnockedBlock)
        {
            _hasKnockedBlock = true;
            return true;
        }
        return false;
    }
}
