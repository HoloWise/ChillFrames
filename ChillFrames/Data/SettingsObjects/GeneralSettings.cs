﻿namespace ChillFrames.Data.SettingsObjects
{
    public class GeneralSettings
    {
        public bool EnableLimiter = true;
        public int FrameRateLimit = 60;

        public bool DisableDuringCutscene = true;
        public bool DisableDuringCombat = true;
        public bool DisableDuringDuty = true;
        public bool DisableDuringQuestEvent = true;
    }
}
