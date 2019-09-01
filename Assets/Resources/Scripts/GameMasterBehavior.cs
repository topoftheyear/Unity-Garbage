using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameMasterBehavior : MonoBehaviour
{
    public GameObject player;
    public PlayerBehavior pb;
    public GameObject theCamera;

    public bool paused;

    AudioClip song;
    AudioClip bossSong;
    AudioSource audioPlayer;
    string audioState;

    public AudioClip spaceTestSong;
    public AudioClip slimeLevelSong;
    public AudioClip bossIn11Song;

    public GameObject background;
    public float backgroundSpeed;
    bool backgroundState;

    public CanvasGroup UI, PauseScreen, EndScreen;
    public Button cont;

    public GameData data;

    // Awake is called even before Start
    void Awake()
    {
        string currentName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        paused = false;

        if (currentName == "SpaceTest")
        {
            song = spaceTestSong;
            bossSong = null;
        }
        else if (currentName == "SlimeLevel")
        {
            song = slimeLevelSong;
            bossSong = bossIn11Song;
            backgroundSpeed = 0.25f;
            GameObject.Find("GameMaster/Background").GetComponent<Renderer>().material = Resources.Load("Sprites/Backgrounds/Materials/Slime Background") as Material;
        }

        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = song;
        audioPlayer.playOnAwake = false;
        audioPlayer.loop = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Load from checkpoint
        player = GameObject.Find("Player");
        pb = player.GetComponent<PlayerBehavior>();
        theCamera = GameObject.Find("Main Camera");
        data = LoadFile();
        // If no data is found, set up some defaults
        if (data == null)
        {
            data = new GameData
            {
                speed = pb.speed,
                shootActivate = pb.shootActivate,
                damage = pb.damage,
                // Temp until a current out of level run save exists
                lives = 5,
                audioState = "not played",
            };

        }
        player.transform.position = new Vector3(data.checkpoint, 0, 0);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
        Camera.main.transform.position = new Vector3(data.checkpoint, 0, -10);
        audioPlayer.time = data.musicStart;

        backgroundState = data.backgroundState;
        audioState = data.audioState;

        if (backgroundState)
        {
            background = GameObject.Find("GameMaster/Background");
            var mat = background.GetComponent<Renderer>().material;
            Color.RGBToHSV(mat.color, out float h, out float s, out float v);
            mat.color = Color.HSVToRGB(h, s, 1);
            background.transform.position = new Vector3(background.transform.position.x, background.transform.position.y, 18f);
        }

        // Establish UI
        MainMenuBehavior.ShowCanvas(UI);
        MainMenuBehavior.HideCanvas(PauseScreen);
        MainMenuBehavior.HideCanvas(EndScreen);

        cont.onClick.AddListener(LeaveLevel);
    }

    // Update is called once per frame
    void Update()
    {
        /*float t = audioPlayer.time;
        if (InRange(t, 66.673f) || InRange(t, 130.678f) || InRange(t, 200.025f) || InRange(t, 0f))
        {
            print("X: " + this.transform.position.x + " Time: " + audioPlayer.time);
        }*/
        
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

        if (theCamera != null)
        {
            this.transform.position = new Vector3(theCamera.transform.position.x, theCamera.transform.position.y, 0);
        }

        // Pause checking
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;

            if (paused)
            {
                Time.timeScale = 0;
                MainMenuBehavior.HideCanvas(UI);
                MainMenuBehavior.ShowCanvas(PauseScreen);
                audioPlayer.Pause();
            }
            else
            {
                Time.timeScale = 1;
                MainMenuBehavior.HideCanvas(PauseScreen);
                MainMenuBehavior.ShowCanvas(UI);
                audioPlayer.Play();
            }
        }
        
        if (!audioPlayer.isPlaying)
        {
            if (audioState == "not played")
            {
                audioState = "playing";
                audioPlayer.Play();
            }
            else if (audioState == "playing")
            {
                audioState = "pause";
            }
            else if (audioState == "boss")
            {
                song = null;
                audioPlayer.loop = true;
                audioPlayer.Play();
            }
            else if (audioState == "post boss")
            {
                audioPlayer.loop = false;
                audioState = "no";
                //audioPlayer.Play();
            }
        }

        // Test for boss aliveness
        if (audioState == "boss")
        {
            audioPlayer.volume += 0.002f;
            audioPlayer.volume = Mathf.Min(1, audioPlayer.volume);
            
            // Check to see if the boss is still alive
            bool found = false;
            foreach (Enemy thing in GameObject.FindObjectsOfType<Enemy>())
            {
                if (thing.name.Contains("(Boss)"))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                // The boss has died :(
                audioState = "post boss";
                audioPlayer.Stop();

                audioPlayer.volume = 1;
            }
        }

        background = GameObject.Find("GameMaster/Background");
        var mat = background.GetComponent<Renderer>().material;
        mat.mainTextureOffset = new Vector2(Time.time * backgroundSpeed, 0.5f);

        DrawUI();
    }

    void DrawUI()
    {
        Text lifeText = GameObject.Find("GameMaster/Main/Lives").GetComponent<Text>();
        lifeText.text = data.lives + "";

        if (audioState == "post boss")
        {
            MainMenuBehavior.HideCanvas(UI);
            MainMenuBehavior.ShowCanvas(EndScreen);
        }
    }

    private bool InRange(float time, float target)
    {
        float ta = target - time;
        if (ta < 0.05 && ta > -0.05){
            return true;
        }
        return false;
    }

    // Check for game system updates
    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject);

        if (other.gameObject.ToString().Contains("Scene"))
        {
            ResetCheckpoint();
            UnityEngine.SceneManagement.SceneManager.LoadScene(other.gameObject.GetComponent<SceneTransfer>().scene);
        }
        else if (other.gameObject.ToString().Contains("Checkpoint"))
        {
            UpdateCheckpoint(other.gameObject);
        }
        else if (other.gameObject.ToString().Contains("Special Event"))
        {
            SpecialEventBehavior seb = other.GetComponent<SpecialEventBehavior>();
            if (seb.eventName == "Background On")
            {
                StartCoroutine(ShowBackground());
            }
            else if (seb.eventName == "Background Off")
            {
                StartCoroutine(HideBackground());
            }
            else if (seb.eventName == "Stop")
            {
                pb.baseRightSpeed = 0;
            }
            else if (seb.eventName == "Boss")
            {
                audioState = "boss";
                audioPlayer.clip = bossSong;
                audioPlayer.volume = 0;
            }
        }
    }

    public void PlayerDied()
    {
        // f
        data.lives--;
        if (data.lives <= 0)
        {
            DeleteSave();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        else
        {
            if (audioState == "playing")
            {
                data.audioState = "not played";
            }
            SaveFile();
            // Reload current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void UpdateCheckpoint(GameObject newCheck)
    {
        if (newCheck.transform.position.x != data.checkpoint)
        {
            // Display temporary text

            data.checkpoint = newCheck.transform.position.x;
            data.musicStart = newCheck.GetComponent<CheckpointBehavior>().timestamp;
            data.audioState = audioState;
            data.backgroundState = backgroundState;
            data.speed = pb.speed;
            data.shootActivate = pb.shootActivate;
            data.damage = pb.damage;
            SaveFile();
        }
    }

    public void ResetCheckpoint()
    {
        data.checkpoint = 0f;
        SaveFile();
    }

    void SaveFile()
    {
        string destination = Application.persistentDataPath + "/in_level.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    GameData LoadFile()
    {
        string destination = Application.persistentDataPath + "/in_level.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.Log("File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        print(data);

        pb.speed = data.speed;
        pb.shootActivate = data.shootActivate;
        pb.damage = data.damage;

        return data;
    }

    void DeleteSave()
    {
        string destination = Application.persistentDataPath + "/in_level.dat";

        if (File.Exists(destination))
        {
            File.Delete(destination);
        }
        else
        {
            Debug.Log("File: " + destination + " could not be deleted because it doesn't exist.");
        }
    }

    IEnumerator ShowBackground()
    {
        backgroundState = true;

        GameObject stars = GameObject.Find("PixelSpawner");
        if (stars != null)
        {
            Destroy(GameObject.Find("PixelSpawner"));
        }

        var mat = background.GetComponent<Renderer>().material;
        float v = 0;
        while (v < 1)
        {
            Color.RGBToHSV(mat.color, out float h, out float s, out v);
            v += 0.01f;
            mat.color = Color.HSVToRGB(h, s, v);
            background.transform.position = new Vector3(background.transform.position.x, background.transform.position.y, background.transform.position.z - 0.1f);
            yield return null;
        }  
    }

    IEnumerator HideBackground()
    {
        backgroundState = false;

        GameObject stars = GameObject.Find("PixelSpawner");
        if (stars == null)
        {
            Instantiate(Resources.Load("Objects/PixelSpawner"));
        }

        var mat = background.GetComponent<Renderer>().material;
        float v = 1;
        while (v > 0)
        {
            Color.RGBToHSV(mat.color, out float h, out float s, out v);
            v -= 0.01f;
            mat.color = Color.HSVToRGB(h, s, v);
            background.transform.position = new Vector3(background.transform.position.x, background.transform.position.y, background.transform.position.z + 0.1f);
            yield return null;
        }
    }

    void LeaveLevel()
    {
        DeleteSave();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}

[System.Serializable()]
public class GameData
{
    public float checkpoint;
    public float musicStart;
    public bool backgroundState;
    public string audioState;

    // Player data
    public int lives;
    public float speed;
    public float shootActivate;
    public int damage;

    public GameData()
    {
        checkpoint = 0;
        musicStart = 0;
        backgroundState = false;
        audioState = "not played";

        lives = 0;
        speed = 0;
        shootActivate = 0;
        damage = 0;
    }

    public GameData(float check, float musicS, bool backgroundS, string audioS, int liv, float spee, float shootA, int dam)
    {
        checkpoint = check;
        musicStart = musicS;
        backgroundState = backgroundS;
        audioState = audioS;

        lives = liv;
        speed = spee;
        shootActivate = shootA;
        damage = dam;
    }

    public override string ToString()
    {
        return checkpoint + " " + musicStart + " " + backgroundState + " " + audioState + " " + lives + " " + speed + " " + shootActivate + " " + damage;
    }
}
