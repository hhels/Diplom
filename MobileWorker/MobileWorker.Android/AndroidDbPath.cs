﻿using MobileWorker.Droid;

[assembly: Dependency(typeof(AndroidDbPath))]

namespace MobileWorker.Droid
{
    internal class AndroidDbPath : IPath
    {
        public string GetDatabasePath(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
        }
    }
}