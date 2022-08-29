using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    private void Awake()
    {
        CreateSingleton();
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
