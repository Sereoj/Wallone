﻿using Wallone.Core.Services;

namespace Wallone.Core.Builders
{
    public class AppPathBuilder : IAppSettings
    {
        public AppPathBuilder AppLocation(string path)
        {
            AppSettingsService.SetAppDirectoryLocation(path);

            LoggerService.Log(this, path);
            return this;
        }

        public AppPathBuilder ApplicationPath(string path)
        {
            AppSettingsService.SetApplicationPath(path);

            LoggerService.Log(this, path);
            return this;
        }

        public AppPathBuilder Build()
        {
            return this;
        }
    }
}