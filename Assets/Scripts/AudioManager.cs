using UnityEngine;
using UnityEngine.UI; // Для работы с UI

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusicSource; // Источник фоновой музыки
    public AudioClip backgroundMusic; // Фоновая музыка
    public AudioClip correctButtonSound; // Звук для правильной кнопки
    public AudioClip wrongButtonSound; // Звук для неверной кнопки
    public AudioClip losePanelSound; // Звук для Lose Panel

    public Image musicToggleImage; // UI Image для фоновой музыки
    public Sprite musicOnSprite; // Спрайт для музыки включенной
    public Sprite musicOffSprite; // Спрайт для музыки выключенной

    public Image soundToggleImage; // UI Image для звуков эффектов
    public Sprite soundOnSprite; // Спрайт для звуков эффектов включенных
    public Sprite soundOffSprite; // Спрайт для звуков эффектов выключенных

    public Image wrongSoundToggleImage; // UI Image для звука неверной кнопки
    public Sprite wrongSoundOnSprite; // Спрайт для звука неверной кнопки включенного
    public Sprite wrongSoundOffSprite; // Спрайт для звука неверной кнопки выключенного

    private bool isMusicMuted = false; // Состояние музыки
    private bool areSoundsMuted = false; // Состояние звуков эффектов
    private bool isWrongSoundMuted = false; // Состояние звука неверной кнопки

    private void Start()
    {
        PlayBackgroundMusic();
        UpdateMusicIcon(); // Обновляем иконку музыки
        UpdateSoundIcon(); // Обновляем иконку звуков эффектов
        UpdateWrongSoundIcon(); // Обновляем иконку звука неверной кнопки
    }

    // Воспроизведение фоновой музыки
    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true; // Зацикливаем музыку
        backgroundMusicSource.Play();
    }

    // Переключение фоновой музыки
    public void ToggleBackgroundMusic()
    {
        isMusicMuted = !isMusicMuted;
        backgroundMusicSource.mute = isMusicMuted;
        UpdateMusicIcon(); // Обновляем иконку при переключении
    }

    // Обновление иконки музыки
    private void UpdateMusicIcon()
    {
        if (isMusicMuted)
        {
            musicToggleImage.sprite = musicOffSprite; // Спрайт "выключено"
        }
        else
        {
            musicToggleImage.sprite = musicOnSprite; // Спрайт "включено"
        }
    }

    // Переключение звуков эффектов
    public void ToggleSoundEffects()
    {
        areSoundsMuted = !areSoundsMuted;
        UpdateSoundIcon(); // Обновляем иконку при переключении
    }

    // Обновление иконки звуков эффектов
    private void UpdateSoundIcon()
    {
        if (areSoundsMuted)
        {
            soundToggleImage.sprite = soundOffSprite; // Спрайт "выключено"
        }
        else
        {
            soundToggleImage.sprite = soundOnSprite; // Спрайт "включено"
        }
    }

    // Переключение звука неверной кнопки
    public void ToggleWrongButtonSound()
    {
        isWrongSoundMuted = !isWrongSoundMuted;
        UpdateWrongSoundIcon(); // Обновляем иконку при переключении
    }

    // Обновление иконки звука неверной кнопки
    private void UpdateWrongSoundIcon()
    {
        if (isWrongSoundMuted)
        {
            wrongSoundToggleImage.sprite = wrongSoundOffSprite; // Спрайт "выключено"
        }
        else
        {
            wrongSoundToggleImage.sprite = wrongSoundOnSprite; // Спрайт "включено"
        }
    }

    // Воспроизведение звука для правильной кнопки
    public void PlayCorrectButtonSound()
    {
        if (!areSoundsMuted)
        {
            AudioSource.PlayClipAtPoint(correctButtonSound, Camera.main.transform.position);
        }
    }

    // Воспроизведение звука для неверной кнопки
    public void PlayWrongButtonSound()
    {
        if (!isWrongSoundMuted)
        {
            AudioSource.PlayClipAtPoint(wrongButtonSound, Camera.main.transform.position);
            Handheld.Vibrate();
            Debug.Log("Вибрация при неверной кнопке!"); // Лог для неверной кнопки
        }
    }

    // Воспроизведение звука для Lose Panel
    public void PlayLosePanelSound()
    {
        if (!areSoundsMuted)
        {
            AudioSource.PlayClipAtPoint(losePanelSound, Camera.main.transform.position);
        }
    }
}
