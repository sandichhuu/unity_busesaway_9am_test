namespace BA.Level
{
    public class LevelManager
    {
        private int currentLevelIndex;
        private LevelConfig currentLevelConfig;

        public void LoadLevel(int levelIndex)
        {
            //TODO: Load level from file
            this.currentLevelIndex = levelIndex;
            this.currentLevelConfig = new LevelConfig();
            this.currentLevelConfig.RegenerateSpawnQueues();
        }

        public int GetCurrentLevelIndex()
        {
            return this.currentLevelIndex;
        }

        public LevelConfig GetCurrentLevelConfig()
        {
            return this.currentLevelConfig;
        }
    }
}