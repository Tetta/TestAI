using NUnit.Framework;

public class CompileSmokeTests
{
    [Test]
    public void ProjectCompiles()
    {
        // ������ ����. ���� ������ �� �������������� � CI ����� ��� �� ��� �������.
        Assert.Pass("Compilation successful");
    }
}