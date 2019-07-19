using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameMasterBehavior : MonoBehaviour
{
    public GameObject player;
    public GameObject theCamera;

    AudioClip song;
    AudioSource audioPlayer;

    public AudioClip spaceTestSong;
    public AudioClip slimeLevelSong;
    public string audioState;

    float checkpoint;
    float musicStart;

    public GameObject background;
    public float backgroundSpeed;
    public bool backgroundState;

    // Awake is called even before Start
    void Awake()
    {
        string currentName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        checkpoint = 0f;

        if (currentName == "SpaceTest")
        {
            song = spaceTestSong;
        }
        else if (currentName == "SlimeLevel")
        {
            song = slimeLevelSong;
            backgroundSpeed = 0.25f;
            GameObject.Find("GameMaster/Background").GetComponent<Renderer>().material = (Material)Resources.Load("Sprites/Backgrounds/Materials/Slime Background");
        }

        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = song;
        audioPlayer.playOnAwake = false;
        audioPlayer.loop = false;
        audioState = "not played";

        backgroundState = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Load from checkpoint
        LoadFile();
        GameObject.Find("Player").transform.position = new Vector3(checkpoint, 0, 0);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameObject.Find("Player").GetComponent<Collider2D>());
        Camera.main.transform.position = new Vector3(checkpoint, 0, -10);
        audioPlayer.time = musicStart;
    }

    // Update is called once per frame
    void Update()
    {
        /*float t = audioPlayer.time;
        if (InRange(t, 66.673f) || InRange(t, 130.678f) || InRange(t, 200.025f) || InRange(t, 0f))
        {
            print("X: " + this.transform.position.x + " Time: " + audioPlayer.time);
        }*/
        
        player = GameObject.Find("Player");
        theCamera = GameObject.Find("Main Camera");
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

        if (theCamera != null)
        {
            this.transform.position = new Vector3(theCamera.transform.position.x, theCamera.transform.position.y, 0);
        }

        if (!audioPlayer.isPlaying)
        {
            if (audioState == "not played")
            {
                audioState = "playing";
            }
            else if (audioState == "playing")
            {
                audioState = "boss";
            }
            audioPlayer.Play();
        }

        background = GameObject.Find("GameMaster/Background");
        var mat = background.GetComponent<Renderer>().material;
        mat.mainTextureOffset = new Vector2(Time.time * backgroundSpeed, 0.5f);
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
        }
    }

    public void PlayerDied()
    {
        // f
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateCheckpoint(GameObject newCheck)
    {
        checkpoint = newCheck.transform.position.x;
        musicStart = newCheck.GetComponent<CheckpointBehavior>().timestamp;
        SaveFile();
    }

    public void ResetCheckpoint()
    {
        checkpoint = 0f;
        SaveFile();
    }

    void SaveFile()
    {
        string destination = Application.persistentDataPath + "/in_level.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        GameData data = new GameData(checkpoint, musicStart, backgroundState, audioState);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    void LoadFile()
    {
        string destination = Application.persistentDataPath + "/in_level.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        checkpoint = data.checkpoint;
        musicStart = data.musicStart;
        backgroundState = data.backgroundState;

        if (backgroundState)
        {
            background = GameObject.Find("GameMaster/Background");
            var mat = background.GetComponent<Renderer>().material;
            Color.RGBToHSV(mat.color, out float h, out float s, out float v);
            mat.color = Color.HSVToRGB(h, s, 1);
            background.transform.position = new Vector3(background.transform.position.x, background.transform.position.y, 18f);
        }

        audioState = data.audioState;
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
}

[System.Serializable()]
public class GameData
{
    public float checkpoint;
    public float musicStart;
    public bool backgroundState;
    public string audioState;

    public GameData(float check, float musicS, bool backgroundS, string audioS)
    {
        checkpoint = check;
        musicStart = musicS;
        backgroundState = backgroundS;
        audioState = audioS;
    }
}
