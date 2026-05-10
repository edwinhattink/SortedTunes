using SortedTunes.Mediator.Contracts;

namespace SortedTunes.Mediator.UnitTests;

[TestFixture]
public class UnitTests
{
    [Test]
    public void Value_ReturnsSameInstance()
    {
        // act
        ref readonly var value1 = ref Unit.Value;
        ref readonly var value2 = ref Unit.Value;

        // assert
        Assert.That(value1, Is.EqualTo(value2));
    }

    [Test]
    public void Task_ReturnsCompletedTask()
    {
        // act
        var task = Unit.Task;

        using (Assert.EnterMultipleScope())
        {
            // assert
            Assert.That(task.IsCompletedSuccessfully, Is.True);
            Assert.That(task.Result, Is.EqualTo(Unit.Value));
        }
    }

    [Test]
    public void GetHashCode_ReturnsZero()
    {
        Assert.That(Unit.Value.GetHashCode(), Is.Zero);
    }

    [Test]
    public void CompareTo_Unit_ReturnsZero()
    {
        // arrange
        var a = Unit.Value;
        var b = Unit.Value;

        // act & assert
        Assert.That(a.CompareTo(b), Is.Zero);
    }

    [Test]
    public void CompareTo_Object_ReturnsZero()
    {
        // arrange
        IComparable a = Unit.Value;

        // act & assert
        Assert.That(a.CompareTo(new Unit()), Is.Zero);
    }

    [Test]
    public void ToString_ReturnsParentheses()
    {
        Assert.That(Unit.Value.ToString(), Is.EqualTo("()"));
    }
}
