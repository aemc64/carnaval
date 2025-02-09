using UnityEngine;
using UnityEngine.UI;

public class LoadSceneOnClick : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    
    private Button _button;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(() =>
        {
            SceneChanger.Instance.LoadScene(_sceneName);
            _button.onClick.RemoveAllListeners();
        });
    }
}
