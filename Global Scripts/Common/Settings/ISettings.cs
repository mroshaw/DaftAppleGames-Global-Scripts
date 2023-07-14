using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaftAppleGames.Common.Settings
{
    public interface ISettings
    {
        public void SaveSettings();
        public void LoadSettings();
    }
}