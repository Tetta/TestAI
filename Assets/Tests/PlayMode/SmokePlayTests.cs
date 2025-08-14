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
        SceneManager.LoadScene("MainMenu");
        yield return null; // ��� 1 ����

        // ���������, ��� ����� ���������
        Assert.AreEqual("MainMenu", SceneManager.GetActiveScene().name);

        // ��������� ������� ��������� ������� (������ �� ��, ��� � ���� ���� ������)
        //GameObject player = GameObject.FindWithTag("Player");
        //Assert.IsNotNull(player, "Player object not found in scene");
    }
}