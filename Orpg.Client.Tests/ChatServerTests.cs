using DryIoc;
using Tomlet;

namespace Orpg.Client.Tests;

internal class ChatServerTests: TestBase
{
    private class Language
    {
        public readonly Dictionary<string, string> Translations = new();
    }

    private const string LangConfig =
"""
[Translations]
"EN.ChatServer.Test" = "This is a test of {0}."

"EN.ChatServer.Test2" = "This is another test of {0}."
""";

    protected override void Setup(Container container)
    {
    }

    [Test]
    [Timeout(5000)]
    public void Test()
    {
        var lang = new Language();
        lang.Translations.Add("EN.ChatServer.Test", "This is a test of {0}.");
        lang.Translations.Add("EN.ChatServer.Test2", "This is another test of {0}.");

        var str = TomletMain.TomlStringFrom(lang);

        var lang2 = TomletMain.To<Language>(LangConfig);
    }
}