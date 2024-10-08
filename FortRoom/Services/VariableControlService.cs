﻿using Iot.Device.Mcp23xxx;
using Library;
using Library.GPIOLib;
using Library.Model;
using Library.RGBLib;

namespace FortRoom.Services
{
    public static class VariableControlService
    {
        public static bool IsTheGameStarted { get; set; } = false;
        public static bool IsTheGameFinished { get; set; } = false; // Make It True Will stop the game and enable going to the next room
        public static bool IsTheirAnyOneInTheRoom { get; set; } = false;
        public static int TimeOfPressureHit { get; set; } = 0;
        public static int ActiveButtonPressed { get; set; } = 0;
        public static Team TeamScore { get; set; } = new Team();
        public static bool IsOccupied { get; set; }
        public static bool EnableGoingToTheNextRoom = false;

        public static int RoomTiming = 360000;
        public static int CurrentTime = 0;

        public static bool IsRGBButtonServiceStarted = false;
        public static bool IsMatServiceStarted = false;
        public static bool IsObstructionServiceStarted = false;
        public static bool IsGameTimerStarted = false;

        public static bool IsThingsChangedForTheNewRound = false;

        public static RGBColor DefaultColor = RGBColor.purple;
        public static RGBColor CorrectColor = RGBColor.Blue;
        public static RGBColor WrongColor = RGBColor.Red;

        public static bool IsPressureMateActive = false;

        public static Round GameRound = Round.Round1;
        public static GameStatus GameStatus { get; set; } = GameStatus.Empty;
        public static DoorStatus CurrentDoorStatus { get; set; } = DoorStatus.Undefined;
        public static DoorStatus NewDoorStatus { get; set; } = DoorStatus.Open;

        public static string NextRoomURL = "http://shooting.local:5000/api/Shooting/RoomStatus";
        public static string SendScoreToTheNextRoom = "http://shooting.local:5000/api/Shooting/ReceiveScore";

        public static int DelayTimeBeforeInstructionInMs = 10000;
        public static int DelayTimeBeforeTurnPBOnInMs = 35000;

        public static void ResetTheGame()
        {
        }
    }
}
