using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Orders
{
    public enum OrderStatuse
    {
        pending = 0,
        PaymentReceived = 1,
        PaymentFailed = 2
    }
}
