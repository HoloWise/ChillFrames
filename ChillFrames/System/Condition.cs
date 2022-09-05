﻿using System;
using System.Linq;
using ChillFrames.Data.Enums;
using ChillFrames.Data.SettingsObjects;
using Dalamud.Game.ClientState.Conditions;

namespace ChillFrames.System
{
    internal static class Condition
    {
        private static GeneralSettings Settings => Service.Configuration.General;
        private static BlacklistSettings Blacklist => Service.Configuration.Blacklist;

        public static bool DisableFramerateLimit()
        {
            if (Blacklist.Enabled && Blacklist.Mode == BlacklistMode.Exclusion && InFilteredZone()) return true;
            if (Blacklist.Enabled && Blacklist.Mode == BlacklistMode.Inclusion && InFilteredZone()) return false;
            if (Blacklist.Enabled && Blacklist.Mode == BlacklistMode.Inclusion && !InFilteredZone()) return true;

            if (BoundByDuty() && Settings.DisableDuringDuty) return true;
            if (InCombat() && Settings.DisableDuringCombat) return true;
            if (InCutscene() && Settings.DisableDuringCutscene) return true;
            if (InQuestEvent() && Settings.DisableDuringQuestEvent) return true;
            if (IsCrafting() && Settings.DisableDuringCrafting) return true;
            if (IsIslandSanctuary() && Settings.DisableIslandSanctuary) return true;
            if (IsBetweenAreas()) return true;

            return false;
        }

        private static bool InCutscene()
        {
            return Service.Condition[ConditionFlag.OccupiedInCutSceneEvent] ||
                   Service.Condition[ConditionFlag.WatchingCutscene] ||
                   Service.Condition[ConditionFlag.WatchingCutscene78];
        }

        private static bool BoundByDuty()
        {
            if (IsIslandSanctuary()) return false;

            return Service.Condition[ConditionFlag.BoundByDuty] ||
                   Service.Condition[ConditionFlag.BoundByDuty56] ||
                   Service.Condition[ConditionFlag.BoundByDuty95];
        }

        private static bool InCombat()
        {
            return Service.Condition[ConditionFlag.InCombat];
        }

        private static bool InFilteredZone()
        {
            return Blacklist.Territories.Any(territory => territory.TerritoryID == Service.ClientState.TerritoryType);
        }

        private static bool InQuestEvent()
        {
            if (IsIslandSanctuary() && IslandDoingSomethingMode()) return false;

            return Service.Condition[ConditionFlag.OccupiedInQuestEvent];
        }

        private static bool IsBetweenAreas()
        {
            return Service.Condition[ConditionFlag.BetweenAreas] ||
                   Service.Condition[ConditionFlag.BetweenAreas51];
        }

        private static bool IsCrafting()
        {
            return Service.Condition[ConditionFlag.Crafting] ||
                   Service.Condition[ConditionFlag.Crafting40];
        }

        private static bool IsIslandSanctuary()
        {
            var currentArea = Service.ClientState.TerritoryType;

            return currentArea == 1055;
        }

        private static bool IslandDoingSomethingMode()
        {
            return Service.GameGui.GetAddonByName("MJIPadGuide", 1) != IntPtr.Zero;
        }
    }
}
