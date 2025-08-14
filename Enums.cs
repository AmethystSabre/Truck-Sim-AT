namespace TruckSimAT
{
    public enum Trigger
    {
        Invalid = 0,
        Left = 1,
        Right = 2
    }

    public enum TriggerMode
    {
        Normal = 0,
        GameCube = 1,
        VerySoft = 2,
        Soft = 3,
        Hard = 4,
        VeryHard = 5,
        Hardest = 6,
        Rigid = 7,
        VibrateTrigger = 8,
        Choppy = 9,
        Medium = 10,
        VibrateTriggerPulse = 11,
        CustomTriggerValue = 12,
        Resistance = 13,
        Bow = 14,
        Galloping = 15,
        SemiAutomaticGun = 16,
        AutomaticGun = 17,
        Machine = 18,
        VIBRATE_TRIGGER_10Hz = 19,
        OFF = 20,
        FEEDBACK = 21,
        WEAPON = 22,
        VIBRATION = 23,
        SLOPE_FEEDBACK = 24,
        MULTIPLE_POSITION_FEEDBACK = 25,
        MULTIPLE_POSITION_VIBRATION = 26,
    }

    public enum InstructionType
    {
        Invalid = 0,
        TriggerUpdate = 1,
        RGBUpdate = 2,
        PlayerLED = 3,
        PlayerLEDNewRevision = 4,
        MicLED = 5,
        TriggerThreshold = 6,
        ResetToUserSettings = 7,
        GetDSXStatus = 8
    }
}
