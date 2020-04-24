using System;
using Diplom.Mobile.Views.User;

namespace Diplom.Mobile.Models
{
    public class DetailPageMenuItem
    {
        public DetailPageMenuItem()
        {
            TargetType = typeof(MasterDetailPage1Detail);
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}