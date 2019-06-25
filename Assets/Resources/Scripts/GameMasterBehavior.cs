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

    float checkpoint;
    int musicStart;

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
        LoadFile();
        GameObject.Find("Player").transform.position = new Vector3(checkpoint, 0, 0);
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameObject.Find("Player").GetComponent<Collider2D>());
        Camera.main.transform.position = new Vector3(checkpoint, 0, -10);
        audioPlayer.timeSamples = musicStart;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");
        theCamera = GameObject.Find("Main Camera");
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

        if (theCamera != null)
        {
            this.transform.position = new Vector3(theCamera.transform.position.x, theCamera.transform.position.y, 0);
        }

        if (!audioPlayer.isPlaying)
        {
            audioPlayer.Play();
        }
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
    }

    public void PlayerDied()
    {
        // f
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateCheckpoint(GameObject newCheck)
    {
        checkpoint = newCheck.transform.position.x;
        musicStart = audioPlayer.timeSamples;
        SaveFile();
    }

    public void ResetCheckpoint()
    {
        checkpoint = 0f;
        SaveFile();
    }

    void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        GameData data = new GameData(checkpoint, musicStart);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
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
    }
}

[System.Serializable()]
public class GameData
{
    public float checkpoint;
    public int musicStart;

    public GameData(float check, int musicS)
    {
        checkpoint = check;
        musicStart = musicS;
    }
}
