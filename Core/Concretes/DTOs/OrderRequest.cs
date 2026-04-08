using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concretes.DTOs
{
    public class OrderRequest
    {
        public string OrderNumber { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "TRY";
        public string Provider { get; set; } = "MockPay1";
        public string? Description { get; set; }
        public Dictionary<string, object>? MetaData { get; set; }
    }

    public class OrderResponse
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
