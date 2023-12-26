using System.Text.RegularExpressions;

//This is mapper.cs
//this is dev branch

namespace MyTemplate
{
    public static class Mapper
    {
        public static string Method1(string name)
        {
            return "Здравсвтуйте @{name}, вы отчислены".Replace("@{name}", name);
        }

        public static string Method2(object obj)
        {
            var name = obj.GetType().GetProperty("name").GetValue(obj);
            var address = obj.GetType().GetProperty("address").GetValue(obj);
            return "Здравствуйте, @{name}. Вы прописаны по адресу @{address}.".Replace("@{name}", (string)name).Replace("@{address}", (string)address);
        }

        public static string DamirsMethod2(object obj)
        {
            string templateString = "Здравствуйте @{name} вы прописаны по адресу @{address}";

            string pattern = "@{([^}]*)}";
            var regex = new Regex(pattern);

            var matches = regex.Matches(templateString);

            var extractedValues = new Dictionary<string, object>();

            foreach (Match match in matches)
            {
                var extractedValue = match.Groups[1].Value;
                var value = obj.GetType()?.GetProperty(extractedValue)?.GetValue(obj);
                
                extractedValues[extractedValue] = value;
            }

            return FormatTemplate(templateString, extractedValues);
        }


        private static  string FormatTemplate(string template, Dictionary<string, object> values)
        {
            return values.Aggregate(template, (current, pair) => current.Replace("@{" + pair.Key + "}", pair.Value?.ToString() ?? ""));
        }
            

        public static string Method3(object? obj)
            {
                var templateString = "Здравствуйте, @{if(temperature >= 37)} @then{Выздоравливайте} @else{Прогульщица}";

                var pattern = @"@{if\(([^}]*)\)} @then{([^}]*)} @else{([^}]*)}";
                var regex = new Regex(pattern);

                var match = regex.Match(templateString);

                if (match.Success)
                {
                    var condition = match.Groups[1].Value.Trim();
                    var thenValue = match.Groups[2].Value.Trim();
                    var elseValue = match.Groups[3].Value.Trim();

                    if (obj is not null && obj.GetType().GetProperty("temperature") is not null)
                    {
                        int temperatureValue = (int) (obj.GetType().GetProperty("temperature").GetValue(obj) ?? 0) ;
                        var isConditionTrue = temperatureValue >= 37;
                        
                        return isConditionTrue ? thenValue : elseValue;
                    }
                }
                
                return templateString;
        }
    }
}