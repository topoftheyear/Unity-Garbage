using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameMasterBehavior : MonoBehaviour
{
    public GameObject player;

    AudioClip song;
    AudioSource audioPlayer;

    public AudioClip spaceTestSong;

    float checkpoint;

    // Awake is called even before Start
    void Awake()
    {
        string currentName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        checkpoint = 0f;

        if (currentName == "SpaceTest")
        {
            song = spaceTestSong;
        }

        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = song;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Load from checkpoint
        LoadFile();
        GameObject.Find("Player").transform.position = new Vector3(checkpoint, 0, 0);
        Camera.main.transform.position = new Vector3(checkpoint, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player");

        if (player != null)
        {
            this.transform.position = player.transform.position;
        }

        if (!audioPlayer.isPlaying)
        {
            audioPlayer.Play();
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

        GameData data = new GameData(checkpoint);
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
    }
}

[System.Serializable()]
public class GameData
{
    public float checkpoint;

    public GameData(float check)
    {
        checkpoint = check;
    }
}
