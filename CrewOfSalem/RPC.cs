namespace CrewOfSalem
{
    public enum RPC
    {
        PlayAnimation    = 0,
        CompleteTask     = 1,
        SyncSettings     = 2,
        SetInfected      = 3,
        Exiled           = 4,
        CheckName        = 5,
        SetName          = 6,
        CheckColor       = 7,
        SetColor         = 8,
        SetHat           = 9,
        SetSkin          = 10,
        ReportDeadBody   = 11,
        MurderPlayer     = 12,
        SendChat         = 13,
        StartMeeting     = 14,
        SetScanner       = 15,
        SendChatNote     = 16,
        SetPet           = 17,
        SetStartCounter  = 18,
        EnterVent        = 19,
        ExitVent         = 20,
        SnapTo           = 21,
        Close            = 22,
        VotingComplete   = 23,
        CastVote         = 24,
        ClearVote        = 25,
        AddVote          = 26,
        CloseDoorsOfType = 27,
        RepairSystem     = 28,
        SetTasks         = 29,
        UpdateGameData   = 30,

        // --- Custom RPCs --- TODO:
        None,
        RequestSyncLobbyTime,
        SyncLobbyTime,
        ForceEnd,
        SetRole,
        AddKillAbility,
        SetLocalPlayers,
        SoloWin,

        Kill,
        
        Watch,
        WatchVisitor,

        AlertStart,
        AlertEnd,

        GuardStart,
        GuardEnd,

        ShieldStart,
        ShieldEnd,

        CrusaderGuard,
        
        BlockAoeStart,
        BlockAoeEnd,

        BlockStart,
        BlockEnd,
        
        Reveal,

        DisguiseStart,
        DisguiseEnd,

        Frame,

        HypnotizeStart,
        HypnotizeEnd,

        ForgeStart,
        ForgeEnd,

        Blackmail,

        ProtectStart,
        ProtectEnd,

        VestStart,
        VestEnd,

        VampireConvert,
        
        GuardianAngelTarget,

        ExecutionerTarget
    }
}