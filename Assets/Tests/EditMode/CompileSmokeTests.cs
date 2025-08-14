using NUnit.Framework;

public class CompileSmokeTests
{
    [Test]
    public void ProjectCompiles()
    {
        // Пустой тест. Если проект не скомпилируется — CI упадёт ещё до его запуска.
        Assert.Pass("Compilation successful");
    }
}