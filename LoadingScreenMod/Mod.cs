﻿using ICities;

namespace LoadingScreenMod
{
    public sealed class Mod : IUserMod, ILoadingExtension
    {
        static bool created = false;
        public string Name => "Loading Screen Mod";
        public string Description => "New loading options";

        public void OnEnabled() => Create();
        public void OnDisabled() => Stopping();
        public void OnSettingsUI(UIHelperBase helper) => Settings.OnSettingsUI(helper);
        public void OnCreated(ILoading loading) { }
        public void OnReleased() { }
        public void OnLevelLoaded(LoadMode mode) { }
        public void OnLevelUnloading() { }

        void Create()
        {
            if (!created)
            {
                //Trace.Start();
                LevelLoader.Create().Deploy();
                //new PackageManagerFix().Deploy();
                created = true;
            }
        }

        void Stopping()
        {
            //Trace.Stop();
            LevelLoader.instance?.Dispose();
            Settings.helper = null;
            //PackageManagerFix.instance?.Dispose();
            created = false;
        }
    }
}
