using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BDO_Spirit.Services
{
    public class ItemIconService
    {
        private static string IconPath = AppDomain.CurrentDomain.BaseDirectory + "Files\\";

        private static string IconName = null;

        public ItemIconService(string name)
        {
            IconName = name;
        }

        public void SaveImage(string url)
        {
            CreateFolder();

            if (IsImageExist(IconName))
            {
                return;
            }

            using(var client = new WebClient())
            {
                client.DownloadFile(url, IconPath + IconName + ".png");
            }
        }

        public string GetIconPath()
        {
            if (!IsImageExist())
            {
                return null;
            }


            return IconPath + IconName + ".png";
        }

        public static string GetIconPath(string name)
        {
            if (!IsImageExist(name))
            {
                return null;
            }


            return IconPath + name + ".png";
        }

        private static void CreateFolder()
        {
            if (!Directory.Exists(IconPath))
            {
                Directory.CreateDirectory(IconPath);
            }
        }

        private bool IsImageExist()
        {
            return File.Exists(IconPath + IconName + ".png");
        }

        private static bool IsImageExist(string name)
        {
            return File.Exists(IconPath + name + ".png");
        }
    }
}
