using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SmokePlayTests
{
    [UnityTest]
    public IEnumerator MainSceneLoadsAndHasKeyObjects()
    {
        // Загружаем сцену (название подставь своё)
        SceneManager.LoadScene("MainScene");
        yield return null; // ждём 1 кадр

        // Проверяем, что сцена загружена
        Assert.AreEqual("MainScene", SceneManager.GetActiveScene().name);

        // Проверяем наличие ключевого объекта (замени на то, что у тебя есть всегда)
        GameObject player = GameObject.FindWithTag("Player");
        Assert.IsNotNull(player, "Player object not found in scene");
    }
}