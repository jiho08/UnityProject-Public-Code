namespace Core.GameEvent
{
    public static class PlayerEvents
    {
        public static PlayerSkillGaugeEvent playerSkillGaugeEvent = new PlayerSkillGaugeEvent();
        public static PlayerBulletShootEvent playerBulletShootEvent = new PlayerBulletShootEvent();
        public static PlayerReloadEvent playerReloadEvent = new PlayerReloadEvent();
        public static PlayerReloadStatusEvent playerReloadStatusEvent = new PlayerReloadStatusEvent();
        public static PlayerInputToggleEvent playerInputToggleEvent = new PlayerInputToggleEvent();
    }

    public class PlayerSkillGaugeEvent : GameEvent 
    {
        public float gauge;
        public PlayerSkillGaugeEvent Initialize(float gauge)
        {
            this.gauge = gauge;
            return this;
        }
        public PlayerSkillGaugeEvent AddGauge(float gauge)
        {
            this.gauge += gauge;
            return this;
        }
    }
    
    public class PlayerBulletShootEvent : GameEvent
    {
    }

    public class PlayerReloadEvent : GameEvent
    {
        public int currentAmmoCount;
        public PlayerReloadEvent Initialize(int ammoCount)
        {
            this.currentAmmoCount = ammoCount;
            return this;
        }
    }
    public class PlayerReloadStatusEvent : GameEvent
    {
        public bool isReloading;
        public PlayerReloadStatusEvent Initialize(bool isReloading)
        {
            this.isReloading = isReloading;
            return this;
        }
    }
    public class PlayerInputToggleEvent : GameEvent
    {
        public bool isEnabled;
        public PlayerInputToggleEvent Initialize(bool isEnabled)
        {
            this.isEnabled = isEnabled;
            return this;
        }
    }
}