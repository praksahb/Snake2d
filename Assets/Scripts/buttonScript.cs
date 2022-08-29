using UnityEngine;
using UnityEngine.UI;

public class buttonScript : MonoBehaviour
{
    public int sceneIndex;

    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        UIManager.Instance.LoadLevelByIndex(sceneIndex);
    }
}
