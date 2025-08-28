using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;
public class PLController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
 
    private Rigidbody2D _rigidBody;

    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundCheckRadius = 0.1f; 
    [SerializeField] private LayerMask groundLayer;

    private float _dashPower = 8f;
    private bool _isDashing = false;
    private bool _dashInCooldown;
    private bool isGrounded = false;
    private Vector2 _lastMoove = Vector2.zero;
    private bool _lookRight = true;


    // Animator
    private Animator _playerAnimator;
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        _lastMoove = transform.position;
        Movement();
        GroundChecker();
    }
    private void Update()
    {
        AnimationSwitcher(transform.position);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !_isDashing) 
        {
            _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_dashInCooldown)
        {
            Dash();
        }
        _lastMoove = transform.position;

    }
    private void AnimationSwitcher(Vector2 nowPos)
    {
       
        Vector2 roundedNow = new Vector2(
            Mathf.Round(nowPos.x * 100f) / 100f,
            Mathf.Round(nowPos.y * 100f) / 100f
        );
        Vector2 roundedLast = new Vector2(
            Mathf.Round(_lastMoove.x * 100f) / 100f,
            Mathf.Round(_lastMoove.y * 100f) / 100f
        );

        if (roundedNow == roundedLast && !_isDashing)
        {
            SetState(idle: true);
        }
        else if (!_isDashing && roundedNow.y == roundedLast.y)
        {
            SetState(moving: true);
        }
        else if (roundedNow.y > roundedLast.y)
        {
            SetState(jumping: true);
        }
        else if (roundedNow.y < roundedLast.y)
        {
            SetState(falling: true);
        }
        else if (_isDashing)
        {
            SetState(dashing: true);
        }
    }

    private void SetState(bool idle = false, bool moving = false, bool jumping = false, bool falling = false, bool dashing = false)
    {
        _playerAnimator.SetBool("isStaying", idle);
        _playerAnimator.SetBool("isMooving", moving);
        _playerAnimator.SetBool("isJumping", jumping);
        _playerAnimator.SetBool("isFalling", falling);
        _playerAnimator.SetBool("isDashing", dashing);
    }
    private void Movement()
    {
        if (_isDashing)
            return;

        float moveInput = Input.GetAxis("Horizontal");
        _rigidBody.velocity = new Vector2(moveInput * _moveSpeed, _rigidBody.velocity.y);
                
        HorisontalSwitcher(_rigidBody.velocity);
       
    }
    private void HorisontalSwitcher(Vector2 currentVelocity)
    {
        if (!_isDashing)
        {
            if (currentVelocity.x > 0)
            {
                transform.localScale = new Vector2(1, 1);
                _lookRight = true;
            }
            else if (currentVelocity.x < 0)
            {
                transform.localScale = new Vector2(-1, 1);
                _lookRight = false;
            }
        }
    }
    private void Dash()
    {
        Vector2 startPos = gameObject.transform.position;
        
        if (Input.GetAxis("Horizontal") > 0 )
        {
            _lookRight = true ;
            StartCoroutine(Dashing(_lookRight));
        }
        else if ((Input.GetAxis("Horizontal") < 0))
        {
            _lookRight = false;
            StartCoroutine(Dashing(_lookRight));
        }
        else
        {
            StartCoroutine(Dashing(_lookRight));
        }
           
    }
    //private IEnumerator Dashing(bool lookRight)
    //{
    //    _isDashing = true;
    //    _dashInCooldown = true;
    //    float rbGravity = _rigidBody.gravityScale;
    //    _rigidBody.gravityScale = 0;
    //    int direction = lookRight ? 1 : -1;

    //    float timer = 0f;
    //    float dashDuration = 0.45f;
    //    float dashSpeed = _moveSpeed * 5f;

    //    while (timer < dashDuration)
    //    {
    //        _rigidBody.velocity = new Vector2(direction * dashSpeed, 0);
    //        timer += Time.fixedDeltaTime;
    //        yield return new WaitForFixedUpdate();
    //    }

    //    _rigidBody.gravityScale = rbGravity;
    //    _isDashing = false;
    //    yield return new WaitForSeconds(1.5f);
    //    _dashInCooldown = false;
    //}

    private IEnumerator Dashing(bool lookRight)
    {
        _isDashing = true;
        transform.localScale  = Vector2.one;
        _dashInCooldown = true;
        float rbGravity = _rigidBody.gravityScale;
        _rigidBody.gravityScale = 0;
        int direction = lookRight ? 1 : -1;
        _playerAnimator.SetFloat("DashDirection", direction);
        _rigidBody.velocity = new Vector2(direction * _dashPower, 0f);
        Debug.Log(_rigidBody.velocity);
        yield return new WaitForSeconds(0.1f);
        Debug.Log(_rigidBody.velocity);
        yield return new WaitForSeconds(0.4f);
        _rigidBody.gravityScale = rbGravity;
        _isDashing = false;
        _playerAnimator.SetFloat("DashDirection", 0f);
        yield return new WaitForSeconds(1.5f);
        _dashInCooldown = false;
    }

    public void OnTeleportation(Vector2 daggerPos)
    {
        gameObject.transform.position = daggerPos;
    }
    
    private void GroundChecker()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

   
}
