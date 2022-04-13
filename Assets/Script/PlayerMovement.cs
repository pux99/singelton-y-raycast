using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Varibles
    public Vector3 spawnPoint;
    public float speed, jump, wallJumpX, wallJumpY, wallJumpTimer, dashForce, dashduration, dashCD, dashTimer, ultimaDireccion,life,invultimer;
    public float proyectileCD=1, axiX;
    public Rigidbody rb;
    Quaternion faceRight,faceLeft;
    int wallSide;
    public GameObject smock, test, projectiel;
    public CharacterManager characterManager;
    public bool touchinWall, isGrounded,walljumping,dashing;
    #endregion

    private void Awake()
    {
            
    }
    void Start()
    {
        Debug.Log("hola");
        characterManager = CharacterManager.instans;
        life = characterManager.health;
        this.gameObject.GetComponentInChildren<TextMesh>().text = "Health " + life + "/" + 100;
        dashTimer = dashCD;
        faceLeft = Quaternion.Euler(0, 90, 0);
        faceRight = Quaternion.Euler(0, 270, 0);
        rb = this.GetComponent<Rigidbody>();
        
    }
    private void Update()
    {
        if (invultimer>=0)
        {
            invultimer-=Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            ThrowProjectile();
        }
        if(life<=0)
        {
            Dead();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCD <= dashTimer)
        {
            //test = Instantiate(smock, transform.position, Quaternion.identity, this.transform);
            rb.useGravity = false;
            rb.velocity = new Vector3(-axiX * dashForce, 0, 0);
            dashing = true;
            Invoke(nameof(EndOfDash), dashduration);
            dashTimer = 0;
        }

        if (dashTimer < dashCD)
            dashTimer += Time.deltaTime;

        if ( (gameObject.transform.eulerAngles.y!=90 && gameObject.transform.eulerAngles.y != 270) || axiX!=0)
        RotatePJ(ultimaDireccion);

        axiX = Input.GetAxis("Horizontal");

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jump);
            this.GetComponent<Animator>().SetBool("jumping", true);
        }

        if (touchinWall && Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            rb.velocity = new Vector3(wallSide * wallJumpX, wallJumpY, 0);
            walljumping = true;
            Invoke(nameof(WallJumpEnd), wallJumpTimer);
        }
        
        
        if (Physics.Raycast(transform.position, gameObject.transform.forward, out RaycastHit hit, 1000))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                
                if (hit.transform.gameObject.GetComponent<ChangeColor>().colores >= 8)
                    hit.transform.gameObject.GetComponent<ChangeColor>().colores = 0;
                if (Input.GetKeyDown(KeyCode.C))
                    hit.transform.gameObject.GetComponent<ChangeColor>().colores++;
            }
            
        }
        


    }
    void FixedUpdate()
    {

        #region Movment    

        if (!walljumping && !dashing)
        {
            if (axiX != 0)
            {
                this.GetComponent<Animator>().SetBool("moving", true);
                rb.velocity = (new Vector3(-axiX * speed, rb.velocity.y, 0));
                
                if (axiX == 1 && gameObject.transform.eulerAngles.y!=270)
                {
                    ultimaDireccion = 1;
                    this.GetComponent<Animator>().SetBool("turningrigth", true);
                }
                if (axiX == -1 && gameObject.transform.eulerAngles.y != 90)
                {
                    ultimaDireccion = -1;
                    this.GetComponent<Animator>().SetBool("turningleft", true);
                }


            }
            if (axiX == 0)
            {
                this.GetComponent<Animator>().SetBool("moving", false);
            }
        }
        #endregion        
    }
    private void OnCollisionEnter(Collision col)
    {
        if (CheckSide(Vector3.up, col)&&col.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            this.GetComponent<Animator>().SetBool("jumping", false);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        switch (col.gameObject.tag)
        {
            case "Wall":
                wallSide = CheckWallSide(col);
                break;
            case "Enemy":
                if (invultimer <= 0)
                {
                    characterManager.Hit();
                    life = characterManager.Changehealth(-10);
                    invultimer = 1;
                }
                break;
            default:
                break;
        }
        
    }
    private void OnCollisionExit(Collision col)
    {
        switch (col.gameObject.tag)
        {
            case "Wall":
                touchinWall = false;
                this.GetComponent<Animator>().SetBool("onWall", false);
                break;
            case "Ground":
                isGrounded = false;
                this.GetComponent<Animator>().SetBool("jumping", true);
                break;
            default:
                break;
        }
    }
    private void OnCollisionStay(Collision col)
    {
        switch (col.gameObject.tag)
        {
            case "Wall":
                touchinWall = true;
                this.GetComponent<Animator>().SetBool("onWall", true);
                break;
            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Respawn":
                spawnPoint = other.gameObject.transform.position;
                break;
            case "Enemy":
                if (invultimer <= 0)
                {
                    characterManager.Hit();
                    life=characterManager.Changehealth(-10);
                    this.gameObject.GetComponentInChildren<TextMesh>().text = "Health " + life + "/" + 100;
                    invultimer = .5f;
                }
                break;
            default:
                break;
        }
    }
    bool CheckSide(Vector3 standarSide,Collision col)
    {
        foreach (ContactPoint hitPoint in col.contacts)
        {
            if (hitPoint.normal == standarSide)
            {
                return true;
            }
        }      
        return false;
    }
    int  CheckWallSide(Collision col)
    {
        foreach (ContactPoint hitPoint in col.contacts)
        {
            if (hitPoint.normal == Vector3.right)
            {
                Debug.Log("rigth");
               return 1;
            }
            else if (hitPoint.normal == -Vector3.right)
                return -1 ;
        }
        return 0;
    }
    void WallJumpEnd()
    {
        walljumping = false;
    }
    void EndOfDash()
    {
        Destroy(test,0.05f);
        rb.useGravity = true;
        dashing = false;
    }
    void RotatePJ(float direction)
    {   
        float rotacion = gameObject.transform.eulerAngles.y;
        if((rotacion<90&&rotacion>-1)||(rotacion<360&&rotacion>270)||(rotacion==90&&direction>=1)|| (rotacion == 270 && direction <= -1))
        gameObject.transform.Rotate(0, 360 * -direction*Time.deltaTime, 0);
        if (rotacion > 90 && rotacion <= 120)
        {
            gameObject.transform.rotation = faceLeft;
            this.GetComponent<Animator>().SetBool("turningleft", false);
        }
        if (rotacion < 270 && rotacion >= 250)
        {
            gameObject.transform.rotation = faceRight;
            this.GetComponent<Animator>().SetBool("turningrigth", false);
            
        }
    }
    void Dead()
    {
        transform.position = spawnPoint;

        life = characterManager.Changehealth(100);
        this.gameObject.GetComponentInChildren<TextMesh>().text = "Health " + life + "/" + 100;
    }
    void ThrowProjectile()
    {
        proyectileCD += Time.deltaTime;
        if (proyectileCD >= 0.5f)
        {
            Instantiate(projectiel, transform.position, Quaternion.identity, this.transform);
            proyectileCD = 0;
        }
    }
}
