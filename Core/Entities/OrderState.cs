using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public enum OrderState
    {
        Pendding = 0,
        OnWay=1,
        Delivered=2,
        Returned=3,
    }
}
