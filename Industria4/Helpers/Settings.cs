using Plugin.Settings.Abstractions;
using Plugin.Settings;
using System;
using System.Diagnostics;
using Industria4.Models.User;

namespace Industria4.Helpers
{
    public static class Settings
    {

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessToken", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("AccessToken", value);
            }
        }

        public static string Username
        {
            get
            {
                return AppSettings.GetValueOrDefault("Username", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("Username", value);
            }
        }

        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault("Password", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("Password", value);
            }
        }

        public static string idUser
        {
            get
            {
                return AppSettings.GetValueOrDefault("idUser", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("idUser", value);
            }
        }
        public static int id
        {
            get
            {
                return AppSettings.GetValueOrDefault("id", 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue("id", value);
            }
        }

        public static string Role
        {
            get
            {
                return AppSettings.GetValueOrDefault("Role", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("Role", value);
            }
        }

        public static string Name
        {
            get
            {
                return AppSettings.GetValueOrDefault("Name", string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue("Name", value);
            }
        }

        public static DateTime expires_on
        {
            get
            {
                return AppSettings.GetValueOrDefault("expires_on", DateTime.Now);
            }
            set
            {
                AppSettings.AddOrUpdateValue("expires_on", value);
            }
        }

        public static void RemoveAll()
        {
            AccessToken = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            idUser = string.Empty;
            id = 0;
            Role = string.Empty;
            Name = string.Empty;
            expires_on = DateTime.Now;

        }
    }
}
