using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    private void Awake()
    {
        CreateSingleton();
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic(Sounds.MusicBgLobby);
    }

    private void Update()
    {

    }

    private void CreateSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void LoadLevelByIndex(int bIndex)
    {
        SceneManager.LoadScene(bIndex);
    }
}
