using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NumberGameManager : MonoBehaviour
{
    public GameObject buttonPrefab;          // Префаб кнопки
    public Transform buttonContainer;        // Контейнер для кнопок
    public TMP_Text scoreText;               // Текст для отображения счета
    public TMP_Text timerText;               // Текст для отображения таймера
    public TMP_Text bonusTimeText;           // Текст для отображения добавленного времени
    public GameObject endGamePanel;          // Панель завершения игры
    public TMP_Text finalScoreText;          // Текст финального счета
    public Button retryButton;                // Кнопка перезапуска
    public Button pauseButton;                // Кнопка для паузы
    public Button unpauseButton;              // Кнопка для анпауз
    public Button menuButton;                 // Кнопка меню
    public Button menuLoseButton;             // Кнопка меню при завершении игры
    public AudioManager audioManager;         // Ссылка на AudioManager

    private List<int> numbers;               // Список чисел
    private int currentNumber;               // Текущее число, которое нужно нажать
    private float timer = 15f;               // Таймер уровня
    private int score = 0;                   // Очки игрока
    private int levelSize = 2;               // Размер таблицы (2x2, 3x3 и т.д.)
    private float bonusDisplayDuration = 2f; // Время показа текста бонуса
    private bool isPaused = false;           // Флаг для отслеживания состояния паузы

    public TMP_Text[] bestScoreTexts; // Массив для отображения лучших результатов
    private List<int> bestScores = new List<int>(); // Список лучших результатов

    private void Start()
    {
        retryButton.onClick.AddListener(RestartGame);
        pauseButton.onClick.AddListener(TogglePause);
        unpauseButton.onClick.AddListener(TogglePause);
        menuButton.onClick.AddListener(ResetGame);
        menuLoseButton.onClick.AddListener(ResetGame);

        LoadBestScores(); // Загрузка лучших результатов при запуске игры
        StartGame();
        audioManager = FindObjectOfType<AudioManager>(); // Находим AudioManager в сцене

    }

// Загрузка лучших результатов из PlayerPrefs
    private void LoadBestScores()
    {
        bestScores.Clear();
        for (int i = 0; i < 8; i++)
        {
            bestScores.Add(PlayerPrefs.GetInt("BestScore" + i, 0));
        }
        UpdateBestScoreDisplay();
    }

// Сохранение лучших результатов в PlayerPrefs
    private void SaveBestScores()
    {
        for (int i = 0; i < bestScores.Count; i++)
        {
            PlayerPrefs.SetInt("BestScore" + i, bestScores[i]);
        }
    }

// Обновление отображения лучших результатов
    private void UpdateBestScoreDisplay()
    {
        for (int i = 0; i < bestScoreTexts.Length; i++)
        {
            bestScoreTexts[i].text = (i + 1) + ". " + bestScores[i] + " P";
        }
    }

    // Метод для сброса игры
    private void ResetGame()
    {
        levelSize = 2; // Сброс размера уровня
        score = 0; // Сброс счета
        timer = 15f; // Сброс таймера
        currentNumber = 0; // Сброс текущего числа
        UpdateScoreDisplay(); // Обновление отображения счета
        bonusTimeText.gameObject.SetActive(false); // Скрытие текста бонуса
        endGamePanel.SetActive(false); // Скрытие панели завершения игры

        // Если игра на паузе, сбрасываем паузу
        if (isPaused)
        {
            TogglePause(); // Переключаем паузу, чтобы продолжить игру
        }

        StartGame(); // Запуск игры заново
    }

    private void Update()
    {
        if (timer > 0 && !isPaused) // Обновляем таймер только если игра еще не закончена и не на паузе
        {
            UpdateTimer();
        }
        HideBonusTimeText();
    }

    // Метод для переключения паузы
    private void TogglePause()
    {
        isPaused = !isPaused; // Переключаем состояние паузы
        Time.timeScale = isPaused ? 0 : 1; // Останавливаем или запускаем игру

        // Здесь можно добавить логику для отображения/скрытия панели паузы
        if (isPaused)
        {
            // Отображаем панель паузы, если необходимо
        }
        else
        {
            // Скрываем панель паузы, если необходимо
        }
    }

    // Функция запуска игры
    private void StartGame()
    {
        endGamePanel.SetActive(false); // Скрыть панель завершения игры
        ClearButtons(); // Очистка кнопок
        currentNumber = 0; // Сброс текущего числа
        GenerateNumbers(); // Генерация новых чисел
        score = 0; // Сброс счета
        UpdateScoreDisplay(); // Обновление отображения счета
        timer = 15f; // Сброс таймера
        timerText.text = "Time: 00:00"; // Обновление текста таймера
    }

    // Генерация последовательных чисел на уровне
    private void GenerateNumbers()
    {
        ClearButtons();
        numbers = new List<int>();

        // Генерация последовательных чисел в зависимости от размера уровня
        for (int i = 0; i < levelSize * levelSize; i++)
        {
            numbers.Add(i + 1); // Добавляем числа от 1 до levelSize * levelSize
        }

        // Перемешивание чисел
        numbers = Shuffle(numbers);

        // Создание кнопок
        foreach (int number in numbers)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
            newButton.GetComponentInChildren<TMP_Text>().text = number.ToString();
            newButton.GetComponent<Button>().onClick.AddListener(() => OnNumberClick(number, newButton));
        }
    }

    // Обработка нажатия на кнопку
    private void OnNumberClick(int number, GameObject button)
    {
        if (number == currentNumber + 1)
        {
            currentNumber++;
            score += 10; // Начисление очков за правильное нажатие
            UpdateScoreDisplay();
            button.GetComponent<Button>().interactable = false; // Деактивировать нажатую кнопку

            // Воспроизведение звука правильного нажатия
            audioManager.PlayCorrectButtonSound();

            // Добавление 0.5 секунды к таймеру за каждое правильное нажатие
            AddBonusTime(0.5f);

            // Проверка, нажаты ли все числа
            if (currentNumber == levelSize * levelSize)
            {
                // Увеличение сложности и обновление поля с новыми числами
                IncreaseDifficulty();
                ResetLevel();
            }
        }
        else
        {
            // Воспроизведение звука неверного нажатия
            audioManager.PlayWrongButtonSound();
        }
    }


    // Увеличение сложности
    private void IncreaseDifficulty()
    {
        levelSize++; // Увеличиваем размер уровня
        if (levelSize > 4) // Ограничение до 4x4
        {
            levelSize = 4;
        }
    }

    // Функция добавления бонусного времени
    private void AddBonusTime(float amount)
    {
        timer += amount;
        bonusTimeText.text = "+0.5 sec";
        bonusTimeText.gameObject.SetActive(true); // Показываем текст

        // Запускаем корутину для анимации текста
        StartCoroutine(AnimateBonusTimeText());
    }

    // Корутину для анимации текста бонуса
    private IEnumerator AnimateBonusTimeText()
    {
        // Начальный размер текста
        bonusTimeText.rectTransform.localScale = Vector3.zero;

        // Анимация появления текста
        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float scale = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            bonusTimeText.rectTransform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедитесь, что текст достиг конечного размера
        bonusTimeText.rectTransform.localScale = Vector3.one;

        // Задержка перед исчезновением текста
        yield return new WaitForSeconds(1f);

        // Анимация исчезновения текста
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float scale = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            bonusTimeText.rectTransform.localScale = new Vector3(scale, scale, scale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Скрываем текст после исчезновения
        bonusTimeText.gameObject.SetActive(false);
    }

    // Обновление уровня
    private void ResetLevel()
    {
        currentNumber = 0;
        GenerateNumbers();
    }

    // Обновление таймера
    private void UpdateTimer()
    {
        timer -= Time.deltaTime;

        // Вычисление секунд и миллисекунд
        int seconds = Mathf.FloorToInt(timer);
        int milliseconds = Mathf.FloorToInt((timer - seconds) * 100);

        // Ограничение миллисекунд до 2 цифр (00 - 99)
        milliseconds = Mathf.Clamp(milliseconds, 0, 99);

        // Форматирование строки таймера в формате СС:ММ
        timerText.text = string.Format("Time: {0:D2}:{1:D2}", seconds, milliseconds);

        // Проверка на завершение времени
        if (timer <= 0)
        {
            EndGame();
        }
    }

    // Завершение игры
    // Завершение игры
    private void EndGame()
    {
        timer = 0;
        endGamePanel.SetActive(true);
        finalScoreText.text = "Score:\n" + score + " P";

        // Воспроизведение звука открытия Lose Panel
        audioManager.PlayLosePanelSound();

        // Проверяем, попадает ли текущий результат в топ-8
        UpdateBestScores(score);
    }

// Обновление лучших результатов
    private void UpdateBestScores(int newScore)
    {
        for (int i = 0; i < bestScores.Count; i++)
        {
            if (newScore > bestScores[i])
            {
                bestScores.Insert(i, newScore); // Вставляем новый результат
                if (bestScores.Count > 8)
                {
                    bestScores.RemoveAt(8); // Удаляем 9-й результат, если он есть
                }
                SaveBestScores(); // Сохраняем обновленный список
                UpdateBestScoreDisplay(); // Обновляем отображение
                break;
            }
        }
    }


    // Перезапуск игры
    private void RestartGame()
    {
        levelSize = 2; // Сброс размера уровня
        score = 0; // Сброс счета
        StartGame(); // Запуск игры заново
    }

    // Очистка кнопок на экране
    private void ClearButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    // Обновление отображения счета
    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score:\n" + score + " P";
    }

    // Скрытие текста бонуса, если он виден
    private void HideBonusTimeText()
    {
        if (bonusTimeText.gameObject.activeSelf && timer <= 0)
        {
            bonusTimeText.gameObject.SetActive(false);
        }
    }

    // Метод для перемешивания списка
    private List<T> Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
            // Если вы находитесь в редакторе, остановите воспроизведение
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // Если вы в сборке игры, выходим из приложения
        Application.Quit();
#endif
    }
}