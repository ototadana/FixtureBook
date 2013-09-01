using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XPFriend.FixtureTest.Cast.Temp.Datas
{
    public class OrderDetails : IEnumerable<OrderDetail>
    {
        private List<OrderDetail> details;

        public OrderDetails(List<OrderDetail> details)
        {
            this.details = details;
        }

        public IEnumerator<OrderDetail> GetEnumerator()
        {
            return details.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return details.GetEnumerator();
        }
    }
}
