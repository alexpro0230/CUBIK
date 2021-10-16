using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class gameMenuScript : MonoBehaviour
{
    [Header("//main:")]
    public bool isMainMenu;

    [Header("//gameObjects:")]
    public GameObject pauseButton;
    public GameObject settingsMenu;
    public GameObject menu;
    public GameObject deathMenu;
    public GameObject weaponManager;
    public GameObject winMenu;
    public GameObject chooseSkinMenu;

    [Header("Skin system")]
    public GameObject[] skins;
    public GameObject skinsPorter;
    public int skinIndex;

    [Header("//scripts:")]
    public weapon_manager wm;
    public movement movementScript;
    public shootBullet shootBulletScript;

    [Header("//variables:")]
    private float timeScale;
    public bool escapeButton;
    public bool canOpenMenu;

    [Header("//components:")]
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider gunSfxVolume;
    public AudioMixer mixer;

    [Header("textures and images")]
    public Texture2D pointerOnMenu;
    public Texture2D cursorOnGame;

    #region startSettings

    void Start()
    {
        if(PlayerPrefs.GetInt("first time", 1) == 1)
        {
            PlayerPrefs.SetInt("first time", 0);

            PlayerPrefs.SetInt("vsync", 0);

            PlayerPrefs.SetFloat("master volume", -20); 
            PlayerPrefs.SetFloat("music volume", -20); 
            PlayerPrefs.SetFloat("Gun SFX volume", -20);
        }

#if UNITY_EDITOR
        PlayerPrefs.SetFloat("master volume", -10);
        PlayerPrefs.SetFloat("music volume", -10);
        PlayerPrefs.SetFloat("Gun SFX volume", -10);
#endif

        QualitySettings.vSyncCount = PlayerPrefs.GetInt("vsync");

        Debug.Log(QualitySettings.vSyncCount);

        cursorOnGame = movementScript.cursor;
        settingsMenu.SetActive(false);
        chooseSkinMenu.SetActive(false);

        startSkinSettings();

        canOpenMenu = true;

        if (escapeButton)
        {
            menu.SetActive(false);
        }

        setValueForSliders();
    }

    private void setValueForSliders()
    {
#if UNITY_EDITOR
        mixer.SetFloat("Master", PlayerPrefs.GetFloat("master volume"));

        mixer.SetFloat("Music", PlayerPrefs.GetFloat("music volume"));

        mixer.SetFloat("Gun SFX", PlayerPrefs.GetFloat("Gun SFX volume"));

#endif

        masterVolume.value = PlayerPrefs.GetFloat("master volume");
        musicVolume.value = PlayerPrefs.GetFloat("music volume");
        gunSfxVolume.value = PlayerPrefs.GetFloat("Gun SFX volume");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMainMenu)
        {
            GameObject canshootPorter = weaponManager.transform.GetChild(wm.selectedWeapon).gameObject;
            canshootPorter.TryGetComponent<shootBullet>(out shootBulletScript);
        }

        if (escapeButton)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {

                if (menu.activeInHierarchy == true)
                {
                    resume();
                }
                else
                {
                    OpenMenu(false);

                    settingsMenu.SetActive(false);
                    chooseSkinMenu.SetActive(false);
                }
            }
        }
    }

    #endregion

    #region Menu managment
    public void resume()
    {
        Cursor.SetCursor(cursorOnGame, Vector2.zero, CursorMode.ForceSoftware);
        wm.canSwitch = true;
        menu.SetActive(false);
        movementScript.slowMo();
        pauseButton.SetActive(true);

        try
        {
            shootBulletScript.canShoot = true;
            grenadeScript grenadeScript;
            weaponManager.transform.GetChild(wm.selectedWeapon).TryGetComponent<grenadeScript>(out grenadeScript);
            grenadeScript.canThrow = true;
        }
        catch
        {
            Debug.Log("no bullet script or grenade script found. Not an error if Current weapon doesn't have to have it");
        }
    }

    public void OpenMenu(bool openWithButton)
    {
        isMainMenu = true;
        if (canOpenMenu)
        {
            Cursor.SetCursor(pointerOnMenu, Vector2.zero, CursorMode.ForceSoftware);

            menu.SetActive(true);
            if (escapeButton)
            {
                try
                {
                    shootBulletScript.canShoot = false;

                }
                catch
                {
                }

                try
                {
                    grenadeScript grenadeScript;
                    weaponManager.transform.GetChild(wm.selectedWeapon).TryGetComponent<grenadeScript>(out grenadeScript);
                    grenadeScript.canThrow = false;
                }
                catch
                {
                }

                wm.canSwitch = false;
                Time.timeScale = 0;
                pauseButton.SetActive(false);
                timeScale = movementScript.currentTimeScale;
            }
        }
    }

    public void Quit()
    {
        Application.Quit(0);
    }

    public void settings(bool open)
    {
        if (open)
        {
            for(int i = 0; i < settingsMenu.transform.GetChild(0).childCount; i++)
            {
                settingsMenu.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }

            settingsMenu.SetActive(true);
            settingsMenu.transform.Find("first menu").gameObject.SetActive(true);
            menu.SetActive(false);
        }
        else
        {
            //shootBulletScript.canShoot = true;
            settingsMenu.SetActive(false);
            settingsMenu.transform.Find("first menu").gameObject.SetActive(false);
            menu.SetActive(true);
        }
    }

    public void openMenu(GameObject menu)
    {
        menu.gameObject.SetActive(true);
    }

    public void closeMenu(GameObject menu)
    {
        menu.gameObject.SetActive(false);
    }

    #endregion

    #region Game death-win managment
    public void win()
    {
        Time.timeScale = 0;
        wm.canSwitch = false;
        try
        {
            shootBulletScript.canShoot = false;

        }
        catch
        {
        }

        try
        {
            grenadeScript grenadeScript;
            weaponManager.transform.GetChild(wm.selectedWeapon).TryGetComponent<grenadeScript>(out grenadeScript);
            grenadeScript.canThrow = false;
        }
        catch
        {
        }
        winMenu.SetActive(true);
        canOpenMenu = false;
        Cursor.SetCursor(pointerOnMenu, Vector2.zero, CursorMode.ForceSoftware);
    }
    
    public void death()
    {
        canOpenMenu = false;
        wm.canSwitch = false;
        
        deathMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.SetCursor(pointerOnMenu, Vector2.zero, CursorMode.ForceSoftware);
        try
        {
            grenadeScript grenadeScript;
            weaponManager.transform.GetChild(wm.selectedWeapon).TryGetComponent<grenadeScript>(out grenadeScript);
            grenadeScript.canThrow = false;
        }
        catch
        {
        }

        try
        {
            shootBulletScript.canShoot = false;
        }
        catch
        {
        }
    }
    #endregion

    #region level loading sys
    public void restartLevel()
    {
        canOpenMenu = true;
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(levelIndex);
        Time.timeScale = 1;
        Cursor.SetCursor(cursorOnGame, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void startGame()
    {
        canOpenMenu = true;
        SceneManager.LoadScene(1);
        Time.timeScale = 1; 
        Cursor.SetCursor(cursorOnGame, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void loadScene(int i)
    {
        canOpenMenu = true;
        SceneManager.LoadScene(i);
        Time.timeScale = 1;
        Cursor.SetCursor(cursorOnGame, Vector2.zero, CursorMode.ForceSoftware);
    }

    #endregion

    #region skin

    public void startSkinSettings()
    {
        foreach(GameObject obj in skins)
        {
            obj.SetActive(false);
        }

        skinIndex = PlayerPrefs.GetInt("skinIndex");

        skins[skinIndex].SetActive(true);
        
    }
    
    public void next()
    {
        skins[skinIndex].SetActive(false);
        skinIndex = (skinIndex + 1) % skins.Length;
        skins[skinIndex].SetActive(true);
    }

    public void previous()
    {
        skins[skinIndex].SetActive(false);
        skinIndex--;
        if(skinIndex < 0)
        {
            skinIndex += skins.Length;
        }
        skins[skinIndex].SetActive(true);
    }

    public void saveSkin()
    {
        menu.SetActive(true);
        chooseSkinMenu.SetActive(false);
        PlayerPrefs.SetInt("skinIndex", skinIndex);
    }

    public void openSkinChoose()
    {
        menu.SetActive(false);
        chooseSkinMenu.SetActive(true);
    }

    #endregion

    #region audio
    public void changeMasterVolume(GameObject sender)
    {
        PlayerPrefs.SetFloat("master volume", sender.GetComponent<Slider>().value);        
        mixer.SetFloat("Master", PlayerPrefs.GetFloat("master volume"));
    }

    public void changeMusicVolume(GameObject sender)
    {
        PlayerPrefs.SetFloat("music volume", sender.GetComponent<Slider>().value);
        mixer.SetFloat("Music", PlayerPrefs.GetFloat("music volume"));
    }

    public void changeGunSfxVolume(GameObject sender)
    {
        PlayerPrefs.SetFloat("Gun SFX volume", sender.GetComponent<Slider>().value);
        mixer.SetFloat("Gun SFX", PlayerPrefs.GetFloat("Gun SFX volume"));
    }

    #endregion

    #region graphics

    public void activatePostProcessing(GameObject sender)
    {
        if (!sender.GetComponent<Toggle>().isOn)
        {
            GameObject.Find("Global Volume").GetComponent<Volume>().weight = .3f;
        }
        else
        {
            GameObject.Find("Global Volume").GetComponent<Volume>().weight = 1;
        }
    }

    public void activateVsync(GameObject sender)
    {
        if(sender.GetComponent<Toggle>().isOn)
        {
            PlayerPrefs.SetInt("vsync", 1);
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("vsync");
            Debug.Log(QualitySettings.vSyncCount);

        }
        else
        {
            PlayerPrefs.SetInt("vsync", 0);
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("vsync");
            Debug.Log(QualitySettings.vSyncCount);
        }
    }

    #endregion
}
