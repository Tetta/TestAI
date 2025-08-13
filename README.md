# Unity Git + CI Template (Android & iOS)

Готовый скелет Unity-проекта + GitHub Actions (GameCI) для мобильных платформ.

## Быстрый старт
1) Создай пустой Unity-проект (2D) или возьми свой. Скопируй сюда папку `Assets` и `.github/` (или просто залей этот шаблон в новый репозиторий).
2) Открой Unity → меню **AI Demo → Build Project Structure** (создаст сцены MainMenu и Game).
3) Закоммить и запушь в GitHub.

CI автоматически:
- соберёт Android (ubuntu) и отдаст APK/AAB в артефактах;
- соберёт iOS Xcode-проект (macos) и отдаст его в артефактах;
- приложит `Editor.log` к каждому билду.

## Настройка секретов (GitHub → Repo Settings → Secrets → Actions)
- `UNITY_LICENSE` — текст лицензии Unity (GameCI). См. инструкции GameCI по активации.
- (Android, опционально для релиза) `ANDROID_KEYSTORE_BASE64`, `ANDROID_KEYSTORE_PASSWORD`, `ANDROID_KEYALIAS_NAME`, `ANDROID_KEYALIAS_PASSWORD` — если хочешь подписывать релизные сборки. В базовом примере сборка будет дебажной/unsigned.
- (iOS) Подписание делается уже на стадии Xcode (Fastlane/Codemagic и т.п.). Этот workflow выдаёт готовый Xcode-проект.

## Папки
- `Assets/Editor/ProjectAutoSetup.cs` — создаёт сцены и базовые объекты.
- `Assets/Scripts/*.cs` — минимальная игровая логика.
- `.github/workflows/` — CI для Android и iOS.
- `.gitignore` — настроен под Unity.

## Примечания
- Пиновать точную версию Unity можно через параметр `unityVersion` в workflow.
- Для ускорения включён кэш папки `Library`.
- Можно добавить PlayMode/EditMode тесты и публиковать отчёты в артефакты.

Удачной разработки!
