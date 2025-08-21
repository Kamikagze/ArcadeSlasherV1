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

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Movement();
        GroundChecker();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !_isDashing) 
        {
            _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_dashInCooldown)
        {
            Dash();
        }
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
        _dashInCooldown = true;
        float rbGravity = _rigidBody.gravityScale;
        _rigidBody.gravityScale = 0;
        int direction = lookRight ? 1 : -1;

        _rigidBody.velocity = new Vector2(direction * _dashPower, 0f);
        Debug.Log(_rigidBody.velocity);
        yield return new WaitForSeconds(0.1f);
        Debug.Log(_rigidBody.velocity);
        yield return new WaitForSeconds(0.4f);
        _rigidBody.gravityScale = rbGravity;
        _isDashing = false;
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
