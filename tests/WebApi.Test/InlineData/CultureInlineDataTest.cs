using System.Collections;

namespace WebApi.Test.InlineData;
// Provides culture codes as inline data for parameterized unit tests.
public class CultureInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "en" };
        yield return new object[] { "pt-BR" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
