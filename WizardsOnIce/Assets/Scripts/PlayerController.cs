using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private enum SkillID {Meteor, IceWall};

    public string PlayerNumber;
    [SerializeField] private SkillID Skill;

    public GameObject mainCamera;
    public GameObject missilePrefab;
    public GameObject grabBox;

    public GameObject IceBrake;

    public Text cdtext;

    public Material color;
    public Material indicatorColor;

    public Transform playerCenter;
    public float gravity;
    public float jumpSpeed;

    public Transform missileSpawnLocation;
    public Transform iceWallSpawn;
    public Transform VIPHoldLocation;

    public float throwForce;

    public float speed;
	public float rollSpeed;
    public float airSpeedModifier;
    public float missileSpeed;

    public float maxSpeed;
    public float maxSpeedHitModifier;
    public float maxSpeedDecay;
    public float brakeSpeed;
    public float currentMaxSpeed;
    public float dashCooldown;
    public float turnRate;

    public float FireTime;
    public float DashTime;
    public float GrabTime;
    public float DeathStunTime;
    public float MaxHoldTime;
    public float iTime;

    float angle;
    public float FireTimer;
    public float AbilityTimer;
	public float DashTimer;
    public float GrabTimer;
    public float StunTimer;
    public float HoldTimer;
    public float iTimer;

    public bool dead;

    public Vector3 moveDirection = Vector3.zero;

    Rigidbody rb;
    public Pickupable heldObject;

    public PlayerAbility playerSkill;

    public enum State { NoMovement, GroundedMovement, Jumping, Dash, Braking } 
    
    bool holding;
    public bool beingHeld;
    public State movementState;

    bool willFire;

    public PlayerController holder;

    public float grabTime;

    public float environmentDamage;
    void Awake()
    {

    }


    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody>();

        ChangeMovementState(State.GroundedMovement);
        
        holding = false;

        // Change this to be added by menu system!!
        // SHOULDNT NEED AN ENUM, DUMB CODE
        if (Skill == SkillID.Meteor)
        {
            playerSkill = gameObject.AddComponent<MeteorAbility>();
        }
        else if (Skill == SkillID.IceWall)
        {
            playerSkill = gameObject.AddComponent<IceWallAbility>();

            // short term hack, pls dont keep this way thanks
            transform.Find("PlayerCenter/TargetReticle").position = iceWallSpawn.position;
        }
        // Change this to be added by menu system!!

        playerSkill.Initialize(color, indicatorColor, PlayerNumber, gameObject);


        willFire = false;

        float grabTime = 0.0f;

        GameManager.Inst.AddPlayer(this.GetComponent<PlayerController>());

        dead = false;

        cdtext.enabled = false;

        currentMaxSpeed = maxSpeed;

        IceBrake.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude > currentMaxSpeed)
        {
            Vector3 v = GetComponent<Rigidbody>().velocity.normalized* currentMaxSpeed;

            GetComponent<Rigidbody>().velocity = new Vector3(v.x, GetComponent<Rigidbody>().velocity.y, v.z);
        }

        rb.angularVelocity = Vector3.zero;

        ControlUpdate();
        PowerUpdate();

       


        FireTimer -= Time.deltaTime;
        GrabTimer -= Time.deltaTime;
        HoldTimer -= Time.deltaTime;
        StunTimer -= Time.deltaTime;
        iTimer -= Time.deltaTime;
        AbilityTimer -= Time.deltaTime;
	    DashTimer -= Time.deltaTime;

        if (currentMaxSpeed > maxSpeed)
        {
            IceBrake.GetComponent<Renderer>().enabled = false;
            currentMaxSpeed -= maxSpeedDecay * Time.deltaTime;
            if(currentMaxSpeed < maxSpeed)
            {
                currentMaxSpeed = maxSpeed;
            }
        }
        else
        {
            

            if (movementState != State.Braking)
            {
                currentMaxSpeed = maxSpeed;
                IceBrake.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                IceBrake.GetComponent<Renderer>().enabled = true;
                Color c = IceBrake.GetComponent<Renderer>().material.color;
                IceBrake.GetComponent<Renderer>().material.SetColor("_Color", new Color(c.r, c.g, c.b, 1.0f - currentMaxSpeed / maxSpeed));

                currentMaxSpeed -= brakeSpeed * Time.deltaTime;
                if (currentMaxSpeed < 0)
                {
                    currentMaxSpeed = 0;
                }
            }
        }
    

        cdtext.text = AbilityTimer.ToString();

        if(AbilityTimer <= 0)
        {
            cdtext.enabled = false;
        }

        
        
        
        

        if (GrabTimer <= 0)
        {
            if(grabBox.GetComponent<GrabBox>().isactive)
                grabBox.GetComponent<GrabBox>().isactive = false;
        }

        if(movementState == State.Dash && DashTimer <= dashCooldown)
        {
            ChangeMovementState(State.GroundedMovement);
            currentMaxSpeed = maxSpeed;
        }

        //if (movementState == State.NoMovement && StunTimer <= 0)
        //{

        //    ChangeMovementState(State.GroundedMovement);
        //}
        if (beingHeld && HoldTimer <= 0)
        {
            holder.Chuck();
        }

        
    }


    //Controls and Movement Functions

    public void ChangeMovementState(State state)
    {
        //if(movementState == State.NoMovement && state != State.NoMovement)
        //{
        //    Destroy(gameObject.GetComponent<Pickupable>());
        //    transform.parent = null;
        //}
        if(movementState == State.Dash && state != State.Dash)
        {
            rb.useGravity = true;
        }

        

        movementState = state;

        if(movementState == State.Dash)
        {
            rb.useGravity = false;
        }

        
        

    }

    void ControlUpdate()
    {
        // Normal Movement (Grounded state)
        //if(!rigidbody.isGrounded)
        //{
        //    ChangeMovementState(State.Jumping);
        //}


        if (movementState == State.NoMovement || movementState == State.Dash)
        {
            
        }
        else if (movementState == State.GroundedMovement || movementState == State.Braking)
        {
            
            HorizontalMoveControl();
            AimControl();

            rb.AddForce(moveDirection * Time.deltaTime);

            //playerCenter.transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, angle, 0), turnRate * Time.deltaTime);


        }
        //else if (movementState == State.Jumping)
        //{
        //    HorizontalMoveControl();
        //    AimControl();
        //    rb.AddForce(moveDirection * Time.deltaTime * airSpeedModifier);
        //    moveDirection.y -= (gravity) * Time.deltaTime;
        //    playerCenter.transform.rotation = Quaternion.Euler(0, angle, 0);

        //    //if(controller.isGrounded && moveDirection.y < 0)
        //    //{
        //    //    ChangeMovementState(State.GroundedMovement);
        //    //}
        //}
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
        if (movementState != State.NoMovement && movementState != State.Dash)
        {
            if (!holding)
            {
                if (Input.GetButtonDown("Brake" + PlayerNumber))
                {
                    ChangeMovementState(State.Braking);
                }
                if (Input.GetButtonDown("Fire" + PlayerNumber))
                {
                    grabBox.GetComponent<GrabBox>().SetActive(true);
                    GrabTimer = GrabTime;

                }
                if(Input.GetButtonDown("AbilityTrigger" + PlayerNumber))
                {
                    Ability();
                }

                if (Input.GetAxis("Trigger" + PlayerNumber) > 0.5f)
                {
                    Fireball();
                }

                if (Input.GetButtonDown("RollDash" + PlayerNumber))
                {
                    RollDash();

                }
                if(Input.GetButtonUp("Brake" + PlayerNumber))
                {
                    ChangeMovementState(State.GroundedMovement);
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
        if(FireTimer <= 0)
        {
            GameObject go = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


            go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            go.GetComponent<Bullet>().shooter = PlayerNumber;
            go.transform.GetChild(0).GetComponent<Renderer>().material = color;
            FireTimer = FireTime;
        }
        
    }

    void Ability()
    {
        if (AbilityTimer <= 0)
        {
            playerSkill.TriggerAbility();

            AbilityTimer = playerSkill.GetAbilityTime();

            cdtext.enabled = true;
            cdtext.text = AbilityTimer.ToString();
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
        //if(iTimer <= 0)
        //    currentHealth -= dmg;
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

	public void RollDash ()
	{
		if (DashTimer <= 0) 
		{
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
            moveDirection = new Vector3 (Input.GetAxis ("Horizontal" + PlayerNumber), moveDirection.y, Input.GetAxis ("Vertical" + PlayerNumber));
			moveDirection.x *= rollSpeed;
			moveDirection.z *= rollSpeed;
            rb.AddForce(moveDirection, ForceMode.Impulse);

            currentMaxSpeed = 9999999.0f;
            DashTimer = DashTime + dashCooldown;

            ChangeMovementState(State.Dash);
		}
	}

    // ......Beeeep, your current wait time is **8 MINUTES** ...Beeeeeeep
    public void OnHold()
    {
        beingHeld = true;
        Stun(MaxHoldTime + .2f, true);
        HoldTimer = MaxHoldTime;
    }

    public void OnHit()
    {
        if(currentMaxSpeed < maxSpeed)
        {
            currentMaxSpeed = maxSpeed;
        }
        currentMaxSpeed += maxSpeedHitModifier;
    }

    public void Kill()
    {
        if (!dead)
        {
            dead = true;
            GameManager.Inst.SubPlayer(this.GetComponent<PlayerController>());
            Destroy(gameObject);
        }
    }
   
}

