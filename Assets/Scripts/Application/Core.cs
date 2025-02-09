using UnityEngine;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour
{
    public static Core Instance { get; private set; }
    
    [SerializeField] private LoadingScreen _loadingScreen;

    private string _currentLoadingScene;

    public bool PlayerWonLastGame { get; set; } = true;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadScene(string sceneName)
    {
        _currentLoadingScene = sceneName;
        _loadingScreen.Show(LoadScene);
    }

    private void LoadScene()
    {
        var operationHandler = SceneManager.LoadSceneAsync(_currentLoadingScene, LoadSceneMode.Single);
        if (operationHandler != null)
        {
            operationHandler.completed += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(AsyncOperation operation)
    {
        _loadingScreen.Hide();
    }
}
