using System;
using System.Collections.Generic;
using System.Text;

namespace OICT.Infrastructure.Common
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
