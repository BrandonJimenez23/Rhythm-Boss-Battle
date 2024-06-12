public static class DifficultySettings
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Nightmare
    }

    public static class Easy
    {
        public const float HealthGain = 0.01f;
        public const float HealthLoss = 0.02f;
        public const float PlayerIconMoveGain = -1.0f;
        public const float PlayerIconMoveLoss = 2.0f;
        public const float EnemyIconMoveGain = 1.0f;
        public const float EnemyIconMoveLoss = -2.0f;
    }

    public static class Normal
    {
        public const float HealthGain = 0.02f;
        public const float HealthLoss = 0.05f;
        public const float PlayerIconMoveGain = -2.0f;
        public const float PlayerIconMoveLoss = 5.0f;
        public const float EnemyIconMoveGain = 2.0f;
        public const float EnemyIconMoveLoss = -5.0f;
    }

    public static class Hard
    {
        public const float HealthGain = 0.04f;
        public const float HealthLoss = 0.08f;
        public const float PlayerIconMoveGain = -3.0f;
        public const float PlayerIconMoveLoss = 6.0f;
        public const float EnemyIconMoveGain = 3.0f;
        public const float EnemyIconMoveLoss = -6.0f;
    }

    public static class Nightmare
    {
        public const float HealthGain = 0.05f;
        public const float HealthLoss = 0.10f;
        public const float PlayerIconMoveGain = -4.0f;
        public const float PlayerIconMoveLoss = 8.0f;
        public const float EnemyIconMoveGain = 4.0f;
        public const float EnemyIconMoveLoss = -8.0f;
    }

    public static (float healthGain, float healthLoss, float playerIconMoveGain, float playerIconMoveLoss, float enemyIconMoveGain, float enemyIconMoveLoss) GetSettings(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return (Easy.HealthGain, Easy.HealthLoss, Easy.PlayerIconMoveGain, Easy.PlayerIconMoveLoss, Easy.EnemyIconMoveGain, Easy.EnemyIconMoveLoss);
            case Difficulty.Normal:
                return (Normal.HealthGain, Normal.HealthLoss, Normal.PlayerIconMoveGain, Normal.PlayerIconMoveLoss, Normal.EnemyIconMoveGain, Normal.EnemyIconMoveLoss);
            case Difficulty.Hard:
                return (Hard.HealthGain, Hard.HealthLoss, Hard.PlayerIconMoveGain, Hard.PlayerIconMoveLoss, Hard.EnemyIconMoveGain, Hard.EnemyIconMoveLoss);
            case Difficulty.Nightmare:
                return (Nightmare.HealthGain, Nightmare.HealthLoss, Nightmare.PlayerIconMoveGain, Nightmare.PlayerIconMoveLoss, Nightmare.EnemyIconMoveGain, Nightmare.EnemyIconMoveLoss);
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }
}
