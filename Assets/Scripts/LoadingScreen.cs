using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    // Время, на которое будет показан экран загрузки
    public float displayDuration = 3f;

    // Ссылка на объект меню, который нужно активировать после загрузки
    public GameObject objectToActivate;

    private void Start()
    {
        // Запускаем корутину для управления временем показа
        StartCoroutine(ShowLoadingScreen());
    }

    private System.Collections.IEnumerator ShowLoadingScreen()
    {
        // Показать экран загрузки
        gameObject.SetActive(true);

        // Ждать указанное время
        yield return new WaitForSeconds(displayDuration);

        // Скрыть экран загрузки
        gameObject.SetActive(false);

        // Активировать объект меню
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}