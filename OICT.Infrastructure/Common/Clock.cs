using System;
using System.Collections.Generic;
using System.Text;

namespace OICT.Infrastructure.Common
{
    public class Clock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
