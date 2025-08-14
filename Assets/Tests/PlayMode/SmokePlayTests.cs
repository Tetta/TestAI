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
        // ��������� ����� (�������� �������� ���)
        SceneManager.LoadScene("MainScene");
        yield return null; // ��� 1 ����

        // ���������, ��� ����� ���������
        Assert.AreEqual("MainScene", SceneManager.GetActiveScene().name);

        // ��������� ������� ��������� ������� (������ �� ��, ��� � ���� ���� ������)
        GameObject player = GameObject.FindWithTag("Player");
        Assert.IsNotNull(player, "Player object not found in scene");
    }
}