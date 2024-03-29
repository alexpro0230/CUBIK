using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour, IDamage
{
    #region variables

    [Header("locks")]
    public bool lockMovement;
    public bool lockShooting;
    public bool lockMusicPlay;

    [Header("\n\nmovement:\n")]
    public float movementSpeed;

    [Header("\n\ntime manitpulation:\n")]
    public float slowMoValue; //Slow mo scale
    [HideInInspector]public float currentTimeScale; //Used for other scripts 
    [HideInInspector]public float slowMoTimeLeft; //if in slowm mo, we know how much time more we can use it
    public int slowMoTime; //the maximum of time we can be in slowmo mode
    public float slowMoRecoverDelay;//Delay between slowmo finshes and the slow mo timer starts to slowly recover
    private float slowMoRecoveredDelay; //Counter to check how much of the delay has already past
    private bool canRecoverSlowMoDelay; //Boolean to check if we can start recovering the slowmo delay
    private bool isInSlowMo; //Self explanatory

    [Header("\n\nJump system\n")]
    public float jumpForce; //Make a nice jumping system
    public float hangTime = .2f;
    public float JumpBufferLength = .1f;    
    [HideInInspector]public float JumpBufferCount;
    [HideInInspector]public float hangCounter;

    [Header("\n\nground detection:\n")]
    public Transform checkObj; //The check sphere transform
    public LayerMask groundCheckMask; //What is ground and whats not
    public float checkSphereRadius; //Self exp
    private bool WasGouned; //This is a varible to check if player was grounded the previous frame, and in this way know the exact moment when landed
    public bool grounded; //Variable to know if player is currently landed

    
    [Header("\n\nOther components")]
    public Rigidbody2D rb; //players rigidbody
    public Volume volume; //the current volume
    public AudioSource musicdef; //no fucking idea
    public Slider healthBar; //to modify it's value according to health

    [Header("\n\nshooting system\n")]
    public int bullets; //(amount)
    public bool canShoot; //Self exp
    public lavaScript lavaScript; //Another sctipt for collison in lava
    public gameMenuScript gameMenuScript; //Reference to menu management script
    
    [Header("\n\nhealth system:\n")]
    public float maxHealth; //the health you spawn with
    public float Health { get; set; }

    [Header("\n\nother things:\n")]
    public Texture2D cursor; //The cursors texture
    public GameObject lavaCollEffect; //Effect for collision with lava
    public GameObject storeGo; //self explanatory
    public GameObject pressFtext; //the go with press f to interact
    public GameObject weaponManagerGo; //honestly idk why I have a separate reference to it but im lazy to change
    [HideInInspector] public bool buttonHover; //Varibble to stop you from shooting when hovering over pause button

    [Header("\n\nGrappling Gun system\n")]
    public DistanceJoint2D joint; //The joint that does the grappling hook simulation
    [HideInInspector] public bool isGrappling; //Self exp

    [Header("\n\nJetpack system\n")]
    public bool hasJetpack; //Self exp
    public bool jetPacking; //Currently flying in jetpack
    public int jetpackMultiplier; //variable to control the force that is applied when jetpacking
    private GameObject jetpackGO; //Variable for the jetpack gameobject
    public int jetPackTime; //maximum time that you can fly using jetpack
    [HideInInspector]public float jetPackTimeLeft; //countdow for how much time there's left for jetpack

    #endregion

    #region startThings

    private void Start()
    {
        startSettings();
        initializeJetpack();
    }

    void startSettings()
    {
        joint = GetComponent<DistanceJoint2D>();

        healthBar.maxValue = maxHealth;

        //I'm to lazy to reference a new variable, so i'm gonna use another bars parent to find the slow mo one
        healthBar.gameObject.transform.parent.Find("slowmo counter bar").GetComponent<Slider>().maxValue = slowMoTime;
        healthBar.gameObject.transform.parent.Find("jetpack counter bar").GetComponent<Slider>().maxValue = jetPackTime;
        healthBar.gameObject.transform.parent.Find("jetpack counter bar").gameObject.SetActive(false);

        slowMoTimeLeft = slowMoTime;
               
        weaponManagerGo = transform.Find("weapon manager").gameObject;
        
        
        ChromaticAberration cb;
        volume.profile.TryGet<ChromaticAberration>(out cb);
        cb.intensity.value = 0;
        
        musicdef.Play();

        Health = maxHealth;
        canShoot = true;

        grounded = true;

        //Time settings
        Time.fixedDeltaTime = 0.0007f; //some hardcoded value that made it work so I didnt care about it anymore
        Time.timeScale = 1; //Idk you're already supposed to start with this 

        //Set how cursor looks
        setCursor();

        //if i want to change and get a jetpack by giving it to myself thorught the game then it would start full
        jetPackTimeLeft = jetPackTime;
    }


    void setCursor()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    #endregion

    #region updateThings
    
    void Update()
    {
        //Check if landed
        groundCheck();

        //some other scripts that use this variable instead of getting it themselves
        currentTimeScale = Time.timeScale;
        
        //Take all kind of input
        if(!lockMovement)
            takeInput();

        //Die if health under 0
        checkHealth();
        
        //Other important calculations
        doUpdateThings();

        WasGouned = grounded;
        
        //Calulations for grappling gun
        //grapplingGunCalculation();
    }

    private void FixedUpdate()
    {
        //The if is really important, because without it no movement for the player when grappling would be done
        if (!isGrappling && !lockMovement)
        {
            float x = Input.GetAxisRaw("Horizontal") * movementSpeed;
            rb.velocity = new Vector2(x, rb.velocity.y);
        }
    }

    void doUpdateThings()
    {
        //slow mo calculations: 
        slowMoTimeLeft = Mathf.Clamp(slowMoTimeLeft, 0, slowMoTime);

        //If allowed to recover the delay, do so
        if (canRecoverSlowMoDelay)
        {
            slowMoRecoveredDelay -= Time.unscaledDeltaTime;
        }
        
        //If we are in slow mo, substract from the slow mo counter
        if(isInSlowMo)
        {
            slowMoTimeLeft -= Time.unscaledDeltaTime;
        }

        //If the delay we already recovered the need delay to recover
        if(slowMoRecoveredDelay < 0)
        {
            slowMoTimeLeft += Time.unscaledDeltaTime * 2;
        }
        
        //If we are in slow mo but slow mo time runs out, stop slow mo
        if(Time.timeScale != 1 && slowMoTimeLeft <= 0)
        {
            slowMo();
        }
        
        //when all menus closed, it originated after some bugs, and it was the first hard coded way that came to my mind
        if(gameMenuScript.menu.activeSelf == false && gameMenuScript.deathMenu.activeSelf == false && gameMenuScript.settingsMenu.activeSelf == false && gameMenuScript.winMenu.activeSelf == false && !buttonHover && !lockShooting)
        {
            weaponManagerGo.GetComponent<weapon_manager>().canSwitch = true;
            shootBullet shootBullet;
            
            //Get current weapon's shoot bullet script
            weaponManagerGo.transform.GetChild(weaponManagerGo.GetComponent<weapon_manager>().selectedWeapon).TryGetComponent<shootBullet>(out shootBullet);
            
            //Some weapons dont have shoot bullet script, like graenade, that's why we gotta check if it's not null
            if(shootBullet != null) 
                shootBullet.canShoot = true;
        }
        else if(lockShooting)
        {
            weaponManagerGo.GetComponent<weapon_manager>().canSwitch = false;
            shootBullet shootBullet;

            //Get current weapon's shoot bullet script
            weaponManagerGo.transform.GetChild(weaponManagerGo.GetComponent<weapon_manager>().selectedWeapon).TryGetComponent<shootBullet>(out shootBullet);

            //Some weapons dont have shoot bullet script, like graenade, that's why we gotta check if it's not null
            if (shootBullet != null)
                shootBullet.canShoot = false;

        }

        //If player laned this frame
        if (!WasGouned && grounded)
        {
            //Instantiate landing effect
            GameObject LandPartEffect = GameObjHodler._i.landParticeEffect;
            GameObject instanciatied = Instantiate(LandPartEffect, checkObj.position - new Vector3(0.3f, 0.3f, 0.3f), Quaternion.Euler(90, 0 , 0));
            Destroy(instanciatied, 1f);
        }
        
        //We chane the counters to the needed values
        healthBar.gameObject.transform.parent.Find("slowmo counter bar").GetComponent<Slider>().value = slowMoTimeLeft;
        healthBar.gameObject.transform.parent.Find("jetpack counter bar").GetComponent<Slider>().value = jetPackTimeLeft;
        healthBar.value = Health;
        
        //if we are jetpacking then do the needed calculations and apply the needed forces
        if (jetPacking)
            Jetpack();
        
        //if not then stop the jetack trail
        if (!jetPacking) 
            jetpackGO.GetComponent<ParticleSystem>().Stop();
    }

    void checkHealth()
    {
        if(Health <= 0)
        {
            gameMenuScript.death();
        }
    }
    
    void takeInput()
    {
        if(!lockMovement)
            takeJumpInput();
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            slowMo();
        }
    }

    void takeJumpInput()
    {
        if (!hasJetpack)
        {
            if (grounded)
            {
                hangCounter = hangTime;
            }
            else
            {
                hangCounter -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpBufferCount = JumpBufferLength;
            }
            else
            {
                JumpBufferCount -= Time.deltaTime;
            }

            if (JumpBufferCount >= 0 && hangCounter > 0)
            {
                jump();
                JumpBufferCount = 0;
            }

            if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .30f);
            }
        }
        else
        {
            healthBar.gameObject.transform.parent.Find("jetpack counter bar").gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space) && jetPackTimeLeft > 0)
            {
                jetPacking = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                jetPacking = false;
            }
        }
    }
    
    void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void groundCheck()
    {
        grounded = Physics2D.OverlapCircle(checkObj.position, checkSphereRadius, groundCheckMask);
    }

    #endregion

    #region other things

    public void speedPowerUp(Collider2D collision)
    {
        StartCoroutine(increaseSpeed(5));
        Destroy(collision.gameObject);
    }

    public void jumpPowerUp(GameObject powerUp)
    {
        StartCoroutine(increaseJumpForce(5));
        Destroy(powerUp);
    }

    IEnumerator increaseJumpForce(float time)
    {
        jumpForce *= 2;
        yield return new WaitForSeconds(time);
        jumpForce /= 2;
        StopCoroutine(increaseJumpForce(time));
    }

    IEnumerator increaseSpeed(float time)
    {
        movementSpeed += 7;
        yield return new WaitForSeconds(time);
        movementSpeed -= 7;
        StopCoroutine(increaseSpeed(time));
    }

    public void slowMo()
    {
        //global volume components for slow mo effects
        ChromaticAberration cb;
        LensDistortion ld;
        
        if (Time.timeScale == 1)
        {
            //post processing effects
            volume.profile.TryGet<ChromaticAberration>(out cb);
            volume.profile.TryGet<LensDistortion>(out ld);
            cb.intensity.value = 1;
            ld.intensity.value = .15f;

            //Time effects
            Time.timeScale = slowMoValue;
            
            //slow mo counting
            slowMoRecoveredDelay = slowMoRecoverDelay;
            canRecoverSlowMoDelay = false;
            isInSlowMo = true;

            //Music effects
            musicdef.pitch = .75f;
        }
        else
        {
            //post proccesing
            volume.profile.TryGet<LensDistortion>(out ld);
            volume.profile.TryGet<ChromaticAberration>(out cb);
            ld.intensity.value = 0f;
            cb.intensity.value = 0;

            //time effects
            Time.timeScale = 1;
           
            //music effects
            musicdef.pitch = 1;

            //slow mo counting
            canRecoverSlowMoDelay = true;
            isInSlowMo = false;
        }
    }

    public IEnumerator deleteAfterTime(float time, GameObject gameObject)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        StopCoroutine(deleteAfterTime(time, gameObject));
    }

    #endregion

    #region jetpack system
    
    private void Jetpack()
    {
        if(!jetpackGO.GetComponent<ParticleSystem>().isPlaying)
            jetpackGO.GetComponent<ParticleSystem>().Play();

        jetPackTimeLeft -= Time.deltaTime;

        rb.AddForce((Vector2.up * jetpackMultiplier) * Time.deltaTime, ForceMode2D.Impulse);
    }

    //Creates jetpack at the begging of the game
    private void initializeJetpack()
    {
        Debug.Log(GameObjHodler._i.jetpackVFX);
        jetpackGO = Instantiate(GameObjHodler._i.jetpackVFX, transform.position + new Vector3(0, -0.25f, 0), Quaternion.Euler(90, 0, 0), transform);
        jetpackGO.GetComponent<ParticleSystem>().Stop();
    }

    #endregion
}
