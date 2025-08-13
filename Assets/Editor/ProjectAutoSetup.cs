#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class ProjectAutoSetup
{
    [MenuItem("AI Demo/Build Project Structure")]
    public static void BuildProject()
    {
        // MAIN MENU
        var main = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        main.name = "MainMenu";
        var cam = new GameObject("Main Camera").AddComponent<Camera>();
        cam.orthographic = true; cam.tag = "MainCamera";

        var canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);

        var titleGO = new GameObject("Title");
        titleGO.transform.SetParent(canvas.transform, false);
        var title = titleGO.AddComponent<Text>();
        //title.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        title.alignment = TextAnchor.MiddleCenter;
        title.fontSize = 72;
        title.text = "Unity Git+CI Template";
        var tr = titleGO.GetComponent<RectTransform>();
        tr.sizeDelta = new Vector2(900, 150);
        tr.anchoredPosition = new Vector2(0, 300);

        var btnGO = new GameObject("StartButton", typeof(Image), typeof(Button));
        btnGO.transform.SetParent(canvas.transform, false);
        var btnRect = btnGO.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(420, 140);
        btnRect.anchoredPosition = new Vector2(0, -80);
        var btnTextGO = new GameObject("Text");
        btnTextGO.transform.SetParent(btnGO.transform, false);
        var btnText = btnTextGO.AddComponent<Text>();
        //btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.fontSize = 48;
        btnText.text = "Start Game";
        var btnTextRect = btnTextGO.GetComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero; btnTextRect.anchorMax = Vector2.one;
        btnTextRect.offsetMin = Vector2.zero; btnTextRect.offsetMax = Vector2.zero;

        var menuLogic = new GameObject("MainMenuLogic").AddComponent<MainMenu>();
        menuLogic.startButton = btnGO.GetComponent<Button>();

        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");
        EditorSceneManager.SaveScene(main, "Assets/Scenes/MainMenu.unity");

        // GAME SCENE
        var game = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        game.name = "Game";
        var gcam = new GameObject("Main Camera").AddComponent<Camera>();
        gcam.orthographic = true; gcam.tag = "MainCamera";

        // Player
        var player = new GameObject("Player");
        var psr = player.AddComponent<SpriteRenderer>();
        psr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0,0,4,4), new Vector2(0.5f,0.5f));
        var rb = player.AddComponent<Rigidbody2D>(); rb.gravityScale = 0f;
        player.AddComponent<BoxCollider2D>();
        player.AddComponent<PlayerController>();

        // Coin prefab in-scene
        var coin = new GameObject("Coin");
        var csr = coin.AddComponent<SpriteRenderer>();
        csr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0,0,4,4), new Vector2(0.5f,0.5f));
        var ccol = coin.AddComponent<CircleCollider2D>(); ccol.isTrigger = true;
        coin.AddComponent<Coin>();

        // UI
        var canvas2 = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvas2.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler2 = canvas2.GetComponent<CanvasScaler>();
        scaler2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler2.referenceResolution = new Vector2(1080, 1920);
        var scoreGO = new GameObject("Score"); scoreGO.transform.SetParent(canvas2.transform, false);
        var score = scoreGO.AddComponent<Text>();
        //score.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        score.fontSize = 48; score.alignment = TextAnchor.UpperLeft; score.text = "Score: 0";
        var srt = scoreGO.GetComponent<RectTransform>();
        srt.anchorMin = new Vector2(0,1); srt.anchorMax = new Vector2(0,1); srt.pivot = new Vector2(0,1);
        srt.anchoredPosition = new Vector2(24,-24); srt.sizeDelta = new Vector2(600,120);

        var ui = new GameObject("UI").AddComponent<UIController>();
        ui.scoreText = score;

        var gc = new GameObject("GameController").AddComponent<GameController>();
        gc.ui = ui; gc.player = player.transform; gc.coinPrefab = coin;

        EditorSceneManager.SaveScene(game, "Assets/Scenes/Game.unity");
        AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("AI Demo", "Scenes created.\nOpen MainMenu and Play.", "OK");
    }
}
#endif
