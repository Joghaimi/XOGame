﻿using Library;
using Library.Model;

namespace ShootingRoom.Services
{
    public static class VariableControlService
    {
        public static bool IsTheGameStarted { get; set; } = false;
        public static bool IsTheGameFinished { get; set; } = false;
        public static bool IsTheirAnyOneInTheRoom { get; set; } = false;
        public static int TimeOfGetTheTarget { get; set; } = 0;
        public static int ActiveTargetPressed { get; set; } = 0;
        public static bool EnableGoingToTheNextRoom = false;

        public static bool IsAirTargetServiceStarted = false;

        // IR PIN OUT
        public static int TimeOfPressureHit { get; set; } = 0;
        public static int ActiveButtonPressed { get; set; } = 0;
        public static Team TeamScore { get; set; } = new Team();

        public static bool IsOccupied { get; set; }
        public static int GameScore { get; set; } = 0;
        public static bool IsGameTimerStarted = false;
        public static int RoomTiming = 360000;// Time in Mill
        public static int CurrentTime = 360000;// Time in Mill

        public static int LevelScore = 0;


        public static Round GameRound = Round.Round1;
        
        public static RGBColor DefaultColor = RGBColor.White;



        public static GameStatus GameStatus { get; set; } = GameStatus.Empty;
        public static DoorStatus CurrentDoorStatus { get; set; } = DoorStatus.Undefined;
        public static DoorStatus NewDoorStatus { get; set; } = DoorStatus.Open;

        public static string NextRoomURL = "http://Diving.local:5000/api/Diving/RoomStatus";
        public static string SendScoreToTheNextRoom = "http://Diving.local:5000/api/Diving/ReceiveScore";

        public static int DelayTimeBeforeInstructionInMs = 10000;
        public static int DelayTimeBeforeTurnPBOnInMs = 35000;

    }
}
