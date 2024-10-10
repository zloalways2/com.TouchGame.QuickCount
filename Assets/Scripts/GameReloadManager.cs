using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReloadManager : MonoBehaviour
{
    // Метод для перезагрузки игры
    public void ReloadGame()
    {
        // Получаю имя текущей сцены
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // Загружаю текущую сцену заново
        SceneManager.LoadScene(currentSceneName);
    }
}