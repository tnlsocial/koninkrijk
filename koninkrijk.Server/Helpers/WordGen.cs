namespace koninkrijk.Server.Helpers
{
    public static class WordGen
    {
        public static readonly Dictionary<int, List<string>> WordsByLength = new Dictionary<int, List<string>>
        {
            { 2, new List<string> { "ik", "en", "op", "af", "de", "in", "ja", "je",
                                    "ze", "zo", "nu", "al", "as", "we", "is",
                                    "oh", "na" } },
            { 3, new List<string> { "kat", "kop", "das", "pen", "tas", "val", "weg",
                                    "bus", "net", "pot", "ram", "sap", "vos",
                                    "web", "zee", "zij", "zus", "lip", "mop", "rok",
                                    "hen", "bok", "bom", "kin", "zin", "mes",
                                    "top", "fop", "dij", "dik", "gil", "gum", "kak",
                                    "laf", "lof", "mol", "vla" } },
            { 4, new List<string> { "boom", "deur", "geel", "haar", "hond", "huis", "kaas", "kind",
                                    "koek", "lamp", "meer", "muur", "neus", "raam", "rood", "ster",
                                    "taal", "twee", "vuur", "wijn", "wolf", "zeil", "zoon", "zoek",
                                    "zien", "zout", "zand", "zwak", "bier", "snel", "prof", "pony",
                                    "rijm", "show", "toch", "vlak", "vlam", "vlag", "wilg", "wijf",
                                    "zuil", "rijp", "plus", "volk", "hasj", "warm", "jury", "plak",
                                    "kuif", "jazz", "lauw",} },
            { 5, new List<string> { "appel", "tafel", "stoel", "plant", "vogel", "bloem", 
                                    "klomp", "motor",  "kleed", "molen", "lepel",
                                    "plank", "dozen", "kaart", "koers", "deken", "regen", "links",
                                    "broek", "sjaal", "snoep", "fruit", "kleur", "steen", 
                                    "roest", "fiets", "kinky", "ijzel", "fluim", "exces", "focus",
                                    "essay", "havik", "kwijl", "exact", "lucht", "licht", "hecht",
                                    "vacht", "vinyl", "wezel", "bezem", "bivak", "botox", "macht"} },
            { 6, new List<string> { "wereld", "vogels", "vlucht", "puzzel", "strand",
                                    "quotum", "cowboy", "episch", "excuus", "lepels", 
                                    "muizen", "dekens", "jassen", "koekje", "kasten", 
                                    "vriend", "dieven", "schuim", "schijn", "brunch", "gezwel",
                                    "gifpil", "pyjama", "plicht", "vaccin", "vlecht", "vijzel", 
                                    "klacht", "laptop", "karton", "chique", "kachel", "jaszak", "object",
                                    "schijf", "rechts"} },
            { 7, new List<string> { "meineed", "jackpot", "jurylid", "nuchter", "inzicht", "magisch", "afwezig", "plofkip",
                                    "zijlijn", "zuiplap", "yoghurt", "zwempak", "keelgat", "knikker", "knuffel", 
                                    "koekjes", "dagmenu", "dakloze", "dambord", "dichter", "neusgat", "noodwet", "pamflet",
                                    "pelgrim", "pianist", "politie", "veranda", "verband", "vlaflip", "whiskey", "reclame", "rechtop", "restant"} },
            { 8, new List<string> { "huisarts", "bushalte", "stations", "mijlpaal", "filmzaal", "filosoof", "flexwerk", "denktank",
                                    "computer", "software", "websites", "document", "kamperen", "kijkbuis", "kipfilet", "klaplong", 
                                    "schilder", "techniek", "beenmerg", "beginner", "bejaarde", "markthal", "militair", "tandarts", "tentamen", "treinrit",
                                    "werkloos", "handicap", "hangslot", "hulphond", "laminaat", "landbouw", "licentie", "jaloezie", "journaal", "pashokje", "pijnlijk"
                                    } },
            { 9, new List<string> { "programma", "speeltuin", "netwerken", "kunstwerk", "sculptuur", "innovatie", "bedevaart", "bekeuring", "beschaafd", "diepgaand",
                                    "directeur", "dragqueen", "drijfzand", "formateur", "handelaar", "herhaling", "hypocriet", "koffiemok", "kokosnoot", "meertalig", "mokerslag", "noodbevel",
                                    "nutteloos", "ondergoed", "schnitzel", "sfeerloos", "sneltoets", "spaghetti", "spoorbrug", "steenkool", "stripclub", "wagenziek", "watermerk", "wenkbrauw", "wurggreep", 
                                    "tandpasta", "teletekst", "actieheld", "afkorting", "androgeen", "avondklok"} }
        };

        public static string GenerateWord(int length, int playerGuessSeed)
        {
            if (!WordsByLength.ContainsKey(length))
            {
                throw new InvalidOperationException("No words of that length available");
            }

            var words = WordsByLength[length];
            var random = new Random(playerGuessSeed);
            return words[random.Next(words.Count)];
        }
    }
}
