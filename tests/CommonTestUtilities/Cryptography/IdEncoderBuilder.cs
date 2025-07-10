using Sqids;

namespace CommonTestUtilities.Cryptography;
public class IdEncoderBuilder
{
    public static SqidsEncoder<long> Build()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "BtzIArfXSkqLilMpocUZaWs7EQRd840GCTej5bJvhmPN19uH3Kw6gnFVxDyYO2",
        });
    }
}
