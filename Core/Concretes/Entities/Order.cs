using Core.Abstracts.Bases;
using Core.Concretes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concretes.Entities
{
    public class Order : BaseEntity
    {
        public string CustomerId { get; set; } = null!;
        public virtual Customer? Customer { get; set; }
        public int CartId { get; set; } //ForeignKey değil
        public ICollection<OrderItem> Items { get; set; } = [];
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
