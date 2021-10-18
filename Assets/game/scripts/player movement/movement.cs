using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class movement : MonoBehaviour
{
    #region variables
    [Header("-//movement:")]
    public float movementSpeed;

    [Header("-//time manitpulation:")]
    //Slow mo scale
    public float slowMoValue;
    [HideInInspector]public float currentTimeScale;
    [HideInInspector] public float slowMoTimeLeft;
    
    //Amount of maximum time in slowmo mode
    public int slowMoTime;
    
    //Delay between slowmo finshes and the slow mo timer starts to slowly recover
    public float slowMoRecoverDelay;
    
    //Counter to check how much of the delay has already past
    private float slowMoRecoveredDelay;
    
    //Boolean to check if we can start recovering the slowmo delay
    private bool canRecoverSlowMoDelay;
    
    private bool isInSlowMo; //Self explanatory

    [Header("Jump system")]
    //Make a nice jumping system
    [Range(0, 20)]public float jumpForce;
    public float hangTime = .2f;
    public float JumpBufferLength = .1f;
    
    private float JumpBufferCount;
    private float hangCounter;

    [Header("-//ground detection:")]

    //The check sphere transform
    public Transform checkObj;

    //What is ground and whats not
    public LayerMask groundCheckMask; 
    
    public float checkSphereRadius; //Self exp
    
    //This is a varible to check if player was grounded the previous frame, and in this way know the exact moment when landed
    private bool WasGouned;

    //Variable to know if player is currently landed
    private bool grounded;

    [Header("-//components:")]
    public Rigidbody2D rb;
    public Volume volume;
    public AudioSource musicdef;
    public Slider healthBar;


    [Header("shooting system")]
    public int bullets; //Self exp
    public bool canShoot; //Self exp

    [Header("-//scripts:")]
    //Reference to menu management script
    public gameMenuScript gameMenuScript;

    //Another sctipt for collison in lava
    public lavaScript lavaScript;

    [Header("-//health system:")]
    public float health; //SelfExp

    [Header("-//other things:")]
    public Texture2D cursor; //The cursors texture
    public GameObject lavaCollEffect; //Effect for collision with lava
    public GameObject storeGo; //self exp
    public GameObject pressFtext; //"
    public GameObject weaponManagerGo; //"
    
    //Varibble to stop you from shooting when hovering over pause button
    [HideInInspector] public bool buttonHover;
 
    //other private or not serializable variables
    private bool InteractProcces;
    private bool storeInteraction;

    [Header("Grappling Gun system")]
    //The joint that does the grappling hook simulation
    private DistanceJoint2D joint;
    
    [HideInInspector] public bool isGrappling; //Self exp

    [Header("Jetpack system")]
    public bool hasJetpack; //Self exp
    private bool jetPacking; //Currently flying in jetpack
    public int jetpackMultiplier; //variable to control the force that is applied when jetpacking
    #endregion

    #region startThings

    private void Start()
    {
        healthBar.gameObject.transform.parent.Find("slowmo counter bar").GetComponent<Slider>().maxValue = slowMoTime;
        slowMoTimeLeft = slowMoTime;
        pressFtext = GameObject.FindWithTag("F to interact TEXT");
        if(pressFtext != null) pressFtext.SetActive(false);
        foreach(Transform tr in transform)
        {
            if (tr.tag == "weapon manager")
                weaponManagerGo = tr.gameObject;
        }
        storeGo = GameObject.FindWithTag("store");
        if(storeGo != null) storeGo.SetActive(false);
        try
        {
            print("active in heirarchy: " + storeGo.activeInHierarchy);
            print("active self: " + storeGo.activeSelf);
        }
        catch
        {
            print("Store GO probalby does not exist");
        }
        
        health = 100;
        startSettings();
    }

    void startSettings()
    {
        weaponManagerGo = gameObject.transform.Find("weapon manager").gameObject;
        canShoot = true;
        ChromaticAberration cb;
        volume.profile.TryGet<ChromaticAberration>(out cb);
        cb.intensity.value = 0;
        musicdef.Play();
        grounded = true;
        Time.fixedDeltaTime = 0.0007f;
        Time.timeScale = 1;
        InteractProcces = false;
        setCursor();
    }

    void setCursor()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    #endregion

    #region updateThings
    
    void Update()
    {
        refreshHealth();

        groundCheck();

        currentTimeScale = Time.timeScale;

        takeInput();

        checkHealth();

        //if(InteractProcces) captureInteractProcess();

        doUpdateThings();

        WasGouned = grounded;

        grapplingGunCalculation();
    }

    private void FixedUpdate()
    {
        if (!isGrappling)
        {
            float x = Input.GetAxisRaw("Horizontal") * movementSpeed;
            rb.velocity = new Vector2(x, rb.velocity.y);
        }
    }

    void doUpdateThings()
    {
        slowMoTimeLeft = Mathf.Clamp(slowMoTimeLeft, 0, slowMoTime);

        if(canRecoverSlowMoDelay)
        {
            slowMoRecoveredDelay -= Time.unscaledDeltaTime;
        }

        if(isInSlowMo)
        {
            slowMoTimeLeft -= Time.unscaledDeltaTime;
        }

        if(slowMoRecoveredDelay < 0)
        {
            slowMoTimeLeft += Time.unscaledDeltaTime * 2;
        }

        if(Time.timeScale != 1 && slowMoTimeLeft <= 0)
        {
            slowMo();
        }

        if(gameMenuScript.menu.activeSelf == false && gameMenuScript.deathMenu.activeSelf == false && gameMenuScript.settingsMenu.activeSelf == false && gameMenuScript.winMenu.activeSelf == false && !buttonHover)
        {
            weaponManagerGo.GetComponent<weapon_manager>().canSwitch = true;
            shootBullet shootBullet;
            weaponManagerGo.transform.GetChild(weaponManagerGo.GetComponent<weapon_manager>().selectedWeapon).TryGetComponent<shootBullet>(out shootBullet);
            try
            {
                shootBullet.canShoot = true;
            }
            catch
            {
                
            }
        }

        if(!WasGouned && grounded)
        {
            Debug.Log("player touched ground at: " + transform.position);
            GameObject LandPartEffect = GameObjHodler._i.landParticeEffect;
            Instantiate(LandPartEffect, checkObj.position - new Vector3(0.3f, 0.3f, 0.3f), Quaternion.Euler(90, 0 , 0));
        }

        healthBar.gameObject.transform.parent.Find("slowmo counter bar").GetComponent<Slider>().value = slowMoTimeLeft;

        if (jetPacking) Jetpack();
    }

    void captureInteractProcess()
    {
        if (storeGo == null) return;
    
        if(storeInteraction)
        {
            if(storeGo.activeSelf == true) 
            {
                pressFtext.SetActive(true);
                pressFtext.GetComponentInChildren<TextMeshProUGUI>().text = "Press F to close";
            }    
            else if(storeGo.activeSelf == false) 
            { 
                pressFtext.SetActive(true);
                pressFtext.GetComponentInChildren<TextMeshProUGUI>().text = "Press F to open";
            }
        }
        
        
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(storeInteraction)
            {
                if(storeGo.activeSelf == true) 
                {
                    storeGo.SetActive(false);
                    pressFtext.SetActive(true);
                    pressFtext.GetComponentInChildren<TextMeshProUGUI>().text = "Press F to close";
                }    
                else if(storeGo.activeSelf == false) 
                {
                    storeGo.SetActive(true);
                    pressFtext.SetActive(true);
                    pressFtext.GetComponentInChildren<TextMeshProUGUI>().text = "Press F to open";
                }
            }
        }
    }

    void refreshHealth()
    {
        healthBar.value = health;
    }

    void checkHealth()
    {
        if(health <= 0)
        {
            gameMenuScript.death();
        }
    }
    
    void takeInput()
    {
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
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
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

    IEnumerator bulletClamp()
    {
        bullets = Mathf.Clamp(bullets, 0, 15);
        yield return null;
    }

    #endregion

    #region trigger and collision shit
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        findTriggerType(collision);
    }

    void OnTriggerExit2D(Collider2D collision) {
        findTriggerExitType(collision);
    }
    
    void findTriggerExitType(Collider2D collsion)
    {
        if(collsion.transform.tag == "store trigger")
        {
            InteractProcces = false;
            storeInteraction = false;
            storeGo.SetActive(false);
            pressFtext.SetActive(false);
        }
    }
    
    void findTriggerType(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            LavaCollision();
        }
        else if(collision.gameObject.tag == "win trigger")
        {
            gameMenuScript.win();
        }
        else if(collision.gameObject.tag == "speed p-up")
        {
            speedPowerUp(collision);
        }
        else if(collision.gameObject.tag == "deleteOnEnter")
        {
            fadeText(collision);
        }
        else if(collision.gameObject.tag == "TriggerEnemyAttack")
        {
            triggerEnemyAttackSingle triggerEnemyAttackSingle = collision.gameObject.GetComponent<triggerEnemyAttackSingle>();
            triggerEnemyAttackSingle.startEnemyAttack();
        }
        else if(collision.gameObject.tag == "store trigger")
        {
            InteractProcces = true;
            storeInteraction = true;
        }
    }
    
    public void LavaCollision()
    {
        health -= 40;
        rb.AddForce(Vector2.up * 200000);
        GameObject instantiatedObject = Instantiate(lavaCollEffect, new Vector3(transform.position.x, transform.position.y - 0.5f), Quaternion.identity);
        StartCoroutine(deleteAfterTime(1f, instantiatedObject));
    }
    #endregion

    #region other things
    void speedPowerUp(Collider2D collision)
    {
        StartCoroutine(increaseSpeed(5));
        Destroy(collision.gameObject);
    }
    
    void fadeText(Collider2D collision)
    {
        Animator anim = collision.gameObject.GetComponent<Animator>();
        anim.SetBool("change", true);
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
        ChromaticAberration cb;
        LensDistortion ld;
        if (Time.timeScale == 1)
        {
            volume.profile.TryGet<ChromaticAberration>(out cb);
            cb.intensity.value = 1;
            volume.profile.TryGet<LensDistortion>(out ld);
            ld.intensity.value = .15f;
            Time.timeScale = slowMoValue;
            //Time.fixedDeltaTime = Time.timeScale * 0.02f;
            musicdef.pitch = .75f;
            slowMoRecoveredDelay = slowMoRecoverDelay;
            canRecoverSlowMoDelay = false;
            isInSlowMo = true;
        }
        else
        {
            volume.profile.TryGet<LensDistortion>(out ld);
            ld.intensity.value = 0f;
            volume.profile.TryGet<ChromaticAberration>(out cb);
            cb.intensity.value = 0;
            Time.timeScale = 1;
            //Time.fixedDeltaTime = fixedDeltaTimeAtStart / 5;
            musicdef.pitch = 1;
            canRecoverSlowMoDelay = true;
            isInSlowMo = false;
        }
    }

    IEnumerator deleteAfterTime(float time, GameObject gameObject)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        StopCoroutine(deleteAfterTime(time, gameObject));
    }

    #endregion

    #region grapplingGun
    
    void grapplingGunCalculation()
    {

        if(Input.GetKeyDown(KeyCode.Mouse1) && GameObject.Find("grappling hook pointer").GetComponent<SpriteRenderer>().enabled)
        {
            isGrappling = true;
            joint = GetComponent<DistanceJoint2D>();
            //joint.connectedAnchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            joint.connectedAnchor = GameObject.Find("grappling hook pointer").transform.position;
            GetComponent<LineRenderer>().positionCount = 2;
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, GetComponent<DistanceJoint2D>().connectedAnchor);
            GetComponent<LineRenderer>().enabled = true;
            GetComponent<DistanceJoint2D>().enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isGrappling = false;
            GetComponent<LineRenderer>().enabled = false;
            GetComponent<DistanceJoint2D>().enabled = false;
        }

        if(GetComponent<LineRenderer>().enabled)
        {
            if (GetComponent<DistanceJoint2D>().connectedAnchor.y - transform.position.y <= 0.5f)
            {
                rb.AddForce(Vector2.down * 500, ForceMode2D.Force);
                GetComponent<LineRenderer>().enabled = false;
                GetComponent<DistanceJoint2D>().enabled = false;
            }
            
            GetComponent<LineRenderer>().positionCount = 2;
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, GetComponent<DistanceJoint2D>().connectedAnchor);
        }
    }

    #endregion

    #region jetpack system
    
    private void Jetpack()
    {
        rb.AddForce((Vector2.up * jetpackMultiplier) * Time.deltaTime, ForceMode2D.Impulse);
    }

    #endregion
}