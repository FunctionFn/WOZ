using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public string PlayerNumber;
    public int maxHealth;
    public int currentHealth;
    

    public GameObject mainCamera;
    public GameObject missilePrefab;
    public GameObject meteorPrefab;
    public GameObject meteorIndicator;
    public GameObject grabBox;

    public Text cdtext;

    public Material color;
    public Material indicatorColor;

    public Transform playerCenter;
    public float gravity;
    public float jumpSpeed;

    public Transform missileSpawnLocation;
    public Transform meteorSpawnLocation;
    public Transform meteorTarget;
    public Transform VIPHoldLocation;

    public float throwForce;

    public float speed;
    public float airSpeedModifier;
    public float missileSpeed;
    public float meteorSpeed;

    public float maxSpeed;

    public float FireTime;
    public float MeteorTime;
    public float GrabTime;
    public float DeathStunTime;
    public float MaxHoldTime;
    public float iTime;

    float angle;
    public float FireTimer;
    public float MeteorTimer;
    public float GrabTimer;
    public float StunTimer;
    public float HoldTimer;
    public float iTimer;

    public bool dead;

    public Vector3 moveDirection = Vector3.zero;

    Rigidbody rb;
    public Pickupable heldObject;

    public enum State { NoMovement, GroundedMovement, Jumping }

    bool holding;
    public bool beingHeld;
    public State movementState;

    bool willFire;

    public PlayerController holder;
    //public CostumeEnum equippedCostume;

    // Singleton Pattern

    public float grabTime;

    public float environmentDamage;
    void Awake()
    {

    }


    // Use this for initialization
    void Start()
    {
        //controller = GetComponent<CharacterController>();

        rb = GetComponent<Rigidbody>();

        ChangeMovementState(State.GroundedMovement);

        currentHealth = maxHealth;
        holding = false;

        //Physics.IgnoreLayerCollision(8, gameObject.layer);

        willFire = false;

        float grabTime = 0.0f;

        mainCamera.GetComponent<GameManager>().AddPlayer();

        dead = false;

        cdtext.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude > maxSpeed)
        {
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized* maxSpeed;
        }


        ControlUpdate();
        PowerUpdate();

        

        if (currentHealth <= 0)
        {
            Stun(DeathStunTime);
            currentHealth = maxHealth;
        }


        FireTimer -= Time.deltaTime;
        GrabTimer -= Time.deltaTime;
        HoldTimer -= Time.deltaTime;
        StunTimer -= Time.deltaTime;
        iTimer -= Time.deltaTime;
        MeteorTimer -= Time.deltaTime;

        cdtext.text = MeteorTimer.ToString();

        if(MeteorTimer <= 0)
        {
            cdtext.enabled = false;
        }

        if (GrabTimer <= 0)
        {
            if(grabBox.GetComponent<GrabBox>().isactive)
                grabBox.GetComponent<GrabBox>().isactive = false;
        }

        

        if (movementState == State.NoMovement && StunTimer <= 0)
        {
            

            if(GetComponent<CharacterController>().enabled == false)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Destroy(GetComponent<Rigidbody>());
                GetComponent<CharacterController>().enabled = true;
                
            }

            ChangeMovementState(State.GroundedMovement);
        }
        if (beingHeld && HoldTimer <= 0)
        {
            holder.Chuck();
        }

        
    }


    //Controls and Movement Functions

    public void ChangeMovementState(State state)
    {
        if(movementState == State.NoMovement && state != State.NoMovement)
        {
            Destroy(gameObject.GetComponent<Pickupable>());
            transform.parent = null;
        }

        movementState = state;

        if(movementState == State.NoMovement && !gameObject.GetComponent<Pickupable>())
        {
            gameObject.AddComponent<Pickupable>();
            //transform.parent = mainCamera.transform;
        }

    }

    void ControlUpdate()
    {
        // Normal Movement (Grounded state)
        //if(!rigidbody.isGrounded)
        //{
        //    ChangeMovementState(State.Jumping);
        //}


        if (movementState == State.NoMovement)
        {

        }
        else if (movementState == State.GroundedMovement)
        {
            
            HorizontalMoveControl();
            AimControl();
            //moveDirection.y = 0;
            if (Input.GetButtonDown("Jump" + PlayerNumber))
            {
                Debug.Log("Jump" + PlayerNumber);
                moveDirection.y = jumpSpeed;
                ChangeMovementState(State.Jumping);
            }

            rb.AddForce(moveDirection * Time.deltaTime);

            playerCenter.transform.rotation = Quaternion.Euler(0, angle, 0);

            

        }
        else if (movementState == State.Jumping)
        {
            HorizontalMoveControl();
            AimControl();
            rb.AddForce(moveDirection * Time.deltaTime * airSpeedModifier);
            moveDirection.y -= (gravity) * Time.deltaTime;
            playerCenter.transform.rotation = Quaternion.Euler(0, angle, 0);

            //if(controller.isGrounded && moveDirection.y < 0)
            //{
            //    ChangeMovementState(State.GroundedMovement);
            //}
        }
        //if (willFire)
        //{
        //    Fireball();
        //    willFire = false;
        //}

    }

    void HorizontalMoveControl()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal" + PlayerNumber), moveDirection.y, Input.GetAxis("Vertical" + PlayerNumber));
        //moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.x *= speed;
        moveDirection.z *= speed;
    }

    void AimControl()
    {
        if (Mathf.Abs(Input.GetAxis("RHorizontal" + PlayerNumber)) > 0.01f || Mathf.Abs(Input.GetAxis("RVertical" + PlayerNumber)) > 0.01f)
        {
            angle = Mathf.Atan2(Input.GetAxis("RHorizontal" + PlayerNumber), Input.GetAxis("RVertical" + PlayerNumber)) * Mathf.Rad2Deg;
            //willFire = true;
        }



    }

    void PowerUpdate()
    {
        if (movementState != State.NoMovement)
        {
            if (!holding)
            {
                if(Input.GetButtonDown("Fire" + PlayerNumber))
                {
                    grabBox.GetComponent<GrabBox>().SetActive(true);
                    GrabTimer = GrabTime;

                }
                if(Input.GetButtonDown("Meteor" + PlayerNumber))
                {
                    Meteor();
                }

                if (Input.GetAxis("Trigger" + PlayerNumber) > 0.5f)
                {
                    Fireball();
                }

            }
            else if (Input.GetButtonDown("Fire" + PlayerNumber))
            {
                Chuck();
            }
        }


    }

    void Fireball()
    {
        if(FireTimer <= 0 && !holding && movementState != State.NoMovement)
        {
            GameObject go = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


            go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            go.GetComponent<Bullet>().shooter = PlayerNumber;
            go.transform.GetChild(0).GetComponent<Renderer>().material = color;
            FireTimer = FireTime;
        }
        
    }

    void Meteor()
    {
        if (MeteorTimer <= 0)
        {
            GameObject go = (GameObject)Instantiate(meteorPrefab, meteorSpawnLocation.position, meteorSpawnLocation.rotation);

            go.GetComponent<Rigidbody>().transform.LookAt(meteorTarget);

            go.GetComponent<Rigidbody>().velocity = (go.GetComponent<Rigidbody>().transform.forward) * meteorSpeed;
            go.GetComponent<Meteor>().shooter = PlayerNumber;

            go.transform.GetChild(0).GetComponent<Renderer>().material = color;

            GameObject go2 = (GameObject)Instantiate(meteorIndicator, meteorTarget.position, playerCenter.rotation);

            go2.transform.GetChild(0).GetComponent<Renderer>().material = indicatorColor;

            MeteorTimer = MeteorTime;

            cdtext.enabled = true;
            cdtext.text = MeteorTimer.ToString();
        }
    }

    public void Grab(Pickupable p)
    {
        if (!p.held)
        {
            if (p.GetComponent<Rigidbody>())
            {
                p.GetComponent<Rigidbody>().transform.position = VIPHoldLocation.position;
                p.gameObject.transform.SetParent(this.gameObject.transform);

                p.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.x, 0);

                p.GetComponent<Rigidbody>().isKinematic = true;
                p.held = true;

                grabTime = Time.time;

            }
            else if(p.GetComponent<CharacterController>())
            {
                p.GetComponent<CharacterController>().transform.position = VIPHoldLocation.position;
                p.gameObject.transform.SetParent(this.gameObject.transform);
                p.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.x, 0);

                p.GetComponent<PlayerController>().OnHold();
                p.GetComponent<PlayerController>().holder = this.gameObject.GetComponent<PlayerController>();

            }
            heldObject = p;
            holding = true;
        }

    }

    public void Chuck()
    {
        Vector3 throwmove = new Vector3(moveDirection.x, 0, moveDirection.z) * .5f;
        if(heldObject)
            heldObject.gameObject.transform.SetParent(null);
        else
        {
            holding = false;

        }

        if (heldObject.GetComponent<Rigidbody>())
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.GetComponent<Rigidbody>().AddForce(playerCenter.forward * throwForce + throwmove);
            heldObject.held = false;
            holding = false;

            if (heldObject.gameObject.GetComponent<VIP>())
            {
                heldObject.gameObject.GetComponent<VIP>().GetUp();
            }

        }
        else if(heldObject.GetComponent<CharacterController>())
        {
            //Vector3 vel = playerCenter.forward * throwForce + throwmove;
            heldObject.GetComponent<CharacterController>().enabled = false;
            heldObject.gameObject.AddComponent<Rigidbody>();
            heldObject.GetComponent<Rigidbody>().AddForce(playerCenter.forward * throwForce + throwmove);
            heldObject.held = false;
            heldObject.GetComponent<PlayerController>().beingHeld = false;
            holding = false;
            heldObject.GetComponent<PlayerController>().StunTimer = 1.0f;
            heldObject.GetComponent<PlayerController>().holder = null;
            
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.GetComponent<EnemyBase>())
        //{
        //    Damage(other.GetComponent<EnemyBase>().damage);
        //    Destroy(other.gameObject);

        //    iTween.PunchPosition(mainCamera, new Vector3(0.0f, cameraPunchStrength, 0.0f), cameraPunchTime);
        //}
        //else if (other.GetComponent<Costume>())
        //{
        //    ChangeCostume();
        //    Destroy(other.gameObject);
        //}
        //Destroy(other.gameObject);
        if (other.GetComponent<Killbox>())
        {
            Kill();
        }
    }

    void OnTriggerStay(Collider other)
    {
        //if (other.GetComponent<AmmoStation>() && other.GetComponent<AmmoStation>().active)
        //{
        //    other.GetComponent<AmmoStation>().Deactivate();
        //    ammo += other.GetComponent<AmmoStation>().ammoYield;
        //}
        //Destroy(other.gameObject);
    }

    public void Damage(int dmg)
    {
        if(iTimer <= 0)
            currentHealth -= dmg;
    }

    public void Stun(float time, bool force = false)
    {
        iTimer = iTime;
        ChangeMovementState(State.NoMovement);
        if ((time <= StunTimer || StunTimer <= 0) || force)
        {
            StunTimer = time;
            
        }
        if(holding)
            Chuck();
        
    }

    // ......Beeeep, your current wait time is **8 MINUTES** ...Beeeeeeep
    public void OnHold()
    {
        beingHeld = true;
        Stun(MaxHoldTime + .2f, true);
        HoldTimer = MaxHoldTime;
    }

    public void Kill()
    {
        if (!dead)
        {
            dead = true;
            mainCamera.GetComponent<GameManager>().SubPlayer();
            Destroy(gameObject);
        }
    }
   
}

