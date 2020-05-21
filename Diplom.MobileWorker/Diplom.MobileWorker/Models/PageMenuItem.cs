using System;

namespace Diplom.MobileWorker.Models
{
    public class PageMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Type TargetType { get; set; }
    }
}