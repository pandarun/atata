﻿using _ = Atata.Tests.GoTo1Page;

namespace Atata.Tests
{
    [NavigateTo("GoTo1.html")]
    [VerifyTitle("GoTo 1")]
    public class GoTo1Page : Page<_>
    {
        public Link<GoTo2Page, _> GoTo2 { get; private set; }

        [GoTemporarily]
        public Link<GoTo2Page, _> GoTo2Temporarily { get; private set; }

        public Link<_> GoTo2Blank { get; private set; }
    }
}
