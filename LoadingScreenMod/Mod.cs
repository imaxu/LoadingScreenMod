﻿using ICities;
using ColossalFramework;
using System.IO;

namespace LoadingScreenMod
{
    public sealed class Mod : IUserMod, ILoadingExtension
    {
        static bool created = false;
        public string Name => "Loading Screen Mod";
        public string Description => "New loading options";

        public void OnEnabled() => Create();
        public void OnCreated(ILoading loading) => Create();
        public void OnDisabled() => Stopping();
        public void OnSettingsUI(UIHelperBase helper) => Settings.OnSettingsUI(helper);
        public void OnLevelUnloading() { }
        public void OnReleased() { }

        public void OnLevelLoaded(LoadMode mode)
        {
            Log();
            if (LevelLoader.instance.activated)
                Singleton<LoadingManager>.instance.LoadingAnimationComponent.enabled = false;

            Settings.helper = null;
        }

        void Create()
        {
            if (!created)
            {
                Stopping();
                new LevelLoader().Deploy();
                created = true;
            }
        }

        void Stopping()
        {
            LevelLoader.instance?.Dispose();
            created = false;
        }

        void Log()
        {
            LoadingProfiler[] pp = { LoadingManager.instance.m_loadingProfilerMain, LoadingManager.instance.m_loadingProfilerScenes,
                    LoadingManager.instance.m_loadingProfilerSimulation, LoadingManager.instance.m_loadingProfilerCustomContent, LoadingManager.instance.m_loadingProfilerCustomAsset };

            using (StreamWriter w = new StreamWriter(Util.GetFileName("profilers", "txt")))
                foreach (LoadingProfiler p in pp)
                {
                    w.WriteLine(); w.WriteLine(p.ToString());
                    FastList<LoadingProfiler.Event> events = ProfilerSource.GetEvents(p);

                    foreach (LoadingProfiler.Event e in events)
                        w.WriteLine((e.m_name ?? "").PadRight(32) + "  " + e.m_time + " \t" + e.m_type);
                }
        }
    }
}
