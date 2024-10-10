using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button hiddenButton; // Ссылка на скрытую кнопку

    // Метод, который вызывается при нажатии другой кнопки
    public void OnOtherButtonClick()
    {
        ShowHiddenButton();
    }

    // Метод для показа скрытой кнопки
    void ShowHiddenButton()
    {
        hiddenButton.gameObject.SetActive(true); // Показывает кнопку
      
    }
}
