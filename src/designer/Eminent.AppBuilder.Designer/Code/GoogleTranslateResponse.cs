using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Designer.Code
{
    public class GoogleTranslateResponse
    {
        public GoogleTranslateResponseData data {get;set;}
    }

    public class GoogleTranslateResponseData
    {
        public List<GoogleTranslateResponseTranslation> translations{get;set;}
    }

    public class GoogleTranslateResponseTranslation
    {
        public string translatedText{get;set;}
    }
}
