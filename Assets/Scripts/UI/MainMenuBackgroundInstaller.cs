using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class MainMenuBackgroundInstaller
{
    private static bool _installed;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        if (_installed) return;
        var scene = SceneManager.GetActiveScene();
        if (scene.name != "MainMenu" && scene.name != "MainScene") return;

        _installed = true;
        EnsureBackground();
    }

    private static void EnsureBackground()
    {
        // Canvas (создаём, если нет)
        var canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 1f; // портрет
        }

        // Fullscreen Image
        var bgGO = GameObject.Find("Background") ?? new GameObject("Background", typeof(RectTransform), typeof(Image));
        bgGO.transform.SetParent(canvas.transform, false);
        var rt = bgGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;

        var img = bgGO.GetComponent<Image>();
        img.sprite = Sprite.Create(GenerateTropicalBG(1080, 1920), new Rect(0,0,1080,1920), new Vector2(0.5f,0.5f), 100f);
        img.preserveAspect = true;
        img.raycastTarget = false;
    }

    /// <summary>
    /// Генерирует «сочный» казуальный фон в духе Playrix: небо→море, «солнце», мягкие пятна.
    /// Никаких внешних ассетов не требуется.
    /// </summary>
    private static Texture2D GenerateTropicalBG(int w, int h)
    {
        var tex = new Texture2D(w, h, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear };

        Color skyTop   = new Color(0.36f, 0.78f, 1.00f); // #5CC7FF
        Color skyMid   = new Color(0.48f, 0.88f, 0.98f); // #7BE0FA
        Color seaLow   = new Color(0.18f, 0.79f, 0.76f); // #2ECC C2 примерно
        Color seaDeep  = new Color(0.02f, 0.59f, 0.70f); // #0997B3

        // 1) Вертикальный градиент небо → море
        for (int y = 0; y < h; y++)
        {
            float t = y / (float)(h - 1);
            Color top = Color.Lerp(skyTop, skyMid, Mathf.SmoothStep(0f, 0.6f, t));
            Color bot = Color.Lerp(seaDeep, seaLow, Mathf.SmoothStep(0.4f, 1f, t));
            Color c = (t < 0.5f) ? Color.Lerp(top, skyMid, t*2f) : Color.Lerp(skyMid, bot, (t-0.5f)*2f);

            for (int x = 0; x < w; x++) tex.SetPixel(x, y, c);
        }

        // 2) «Солнце» — мягкий жёлтый круг вверху справа
        DrawRadial(tex, cx: (int)(w*0.78f), cy: (int)(h*0.20f), r: (int)(h*0.22f),
                   inner: new Color(1.00f, 0.95f, 0.55f, 0.80f),
                   outer: new Color(1.00f, 0.95f, 0.55f, 0.00f));

        // 3) Мягкие «блики»/пятна (боке)
        for (int i = 0; i < 6; i++)
        {
            int cx = RandomRange((int)(w*0.15f), (int)(w*0.85f));
            int cy = RandomRange((int)(h*0.25f), (int)(h*0.85f));
            int r  = RandomRange(h/18, h/10);
            var colCenter = new Color(1f,1f,1f, 0.10f);
            var colEdge   = new Color(1f,1f,1f, 0.00f);
            DrawRadial(tex, cx, cy, r, colCenter, colEdge);
        }

        // 4) Силуэт «берега» внизу (мягкая тёмно-зелёная дуга)
        DrawHill(tex, baseY: (int)(h*0.86f), amplitude: (int)(h*0.06f), color: new Color(0.05f, 0.45f, 0.32f, 0.9f));

        tex.Apply(false);
        return tex;
    }

    private static void DrawRadial(Texture2D tex, int cx, int cy, int r, Color inner, Color outer)
    {
        int r2 = r * r;
        int x0 = Mathf.Max(0, cx - r), x1 = Mathf.Min(tex.width - 1, cx + r);
        int y0 = Mathf.Max(0, cy - r), y1 = Mathf.Min(tex.height - 1, cy + r);

        for (int y = y0; y <= y1; y++)
        {
            for (int x = x0; x <= x1; x++)
            {
                int dx = x - cx, dy = y - cy;
                int d2 = dx*dx + dy*dy;
                if (d2 > r2) continue;

                float t = Mathf.Sqrt(d2) / r; // 0..1
                Color c = Color.Lerp(inner, outer, Mathf.SmoothStep(0f, 1f, t));
                var dst = tex.GetPixel(x, y);
                tex.SetPixel(x, y, AlphaBlend(dst, c));
            }
        }
    }

    private static void DrawHill(Texture2D tex, int baseY, int amplitude, Color color)
    {
        int w = tex.width, h = tex.height;
        for (int x = 0; x < w; x++)
        {
            float nx = (x / (float)(w - 1)) * Mathf.PI;
            int yTop = baseY - (int)(Mathf.Sin(nx) * amplitude);
            for (int y = Mathf.Max(0, yTop); y < h; y++)
            {
                var dst = tex.GetPixel(x, y);
                tex.SetPixel(x, y, AlphaBlend(dst, color));
            }
        }
    }

    private static Color AlphaBlend(Color dst, Color src)
    {
        float a = src.a + dst.a * (1f - src.a);
        if (a < 1e-5f) return Color.clear;
        return new Color(
            (src.r * src.a + dst.r * dst.a * (1f - src.a)) / a,
            (src.g * src.a + dst.g * dst.a * (1f - src.a)) / a,
            (src.b * src.a + dst.b * dst.a * (1f - src.a)) / a,
            a
        );
    }

    private static int RandomRange(int min, int max)
    {
        // детерминированно, чтобы фон был стабильным между запусками
        unchecked
        {
            int seed = 123456789;
            seed = seed * 1103515245 + 12345;
            int v = Mathf.Abs(seed);
            return min + (v % (max - min + 1));
        }
    }
}
